using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestHacks.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Web.Compilation;
using System.Xml.Linq;

namespace MSTestHacks.RuntimeDataSource
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class AttachRuntimeDataSources : ContextAttribute
    {
        private static string DATASOURCES_PATH = Path.Combine(Tools.GetBaseDirectory(), "RuntimeDataSources");
        private static List<string> dataSourcesInitalized = new List<string>();

        internal AttachRuntimeDataSources()
            : base("AttachRuntimeDataSources")
        { }

        static AttachRuntimeDataSources()
        {
            var appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            //If the connectionStrings section doest exist, add it.
            if (!appConfig.Sections.Cast<ConfigurationSection>().Any(x => x.SectionInformation.Name == "connectionStrings"))
                appConfig.Sections.Add("connectionStrings", new ConnectionStringsSection());

            //If the test tools section doest exist, add it.
            if (!appConfig.Sections.Cast<ConfigurationSection>().Any(x => x.SectionInformation.Name == "microsoft.visualstudio.testtools"))
                appConfig.Sections.Add("microsoft.visualstudio.testtools", new Microsoft.VisualStudio.TestTools.UnitTesting.TestConfigurationSection());

            var connectionStringsSection = (ConnectionStringsSection)appConfig.Sections["connectionStrings"];
            var testConfigurationSection = (TestConfigurationSection)appConfig.Sections["microsoft.visualstudio.testtools"];

            //Remove all connection strings that have the "_RuntimeDataSources" in the name.
            var connectionsToRemove = connectionStringsSection.ConnectionStrings.Cast<ConnectionStringSettings>().Where(x=> x.Name.Contains("RuntimeDataSource")).ToList();
            foreach (var con in connectionsToRemove)
            {
                connectionStringsSection.ConnectionStrings.Remove(con);
            }

            //Make sure dir exists
            if (!Directory.Exists(DATASOURCES_PATH))
                Directory.CreateDirectory(DATASOURCES_PATH);

            var assembliesToSearch = AppDomain.CurrentDomain.GetAssemblies()
                                                            .SelectMany(assembly => assembly.GetTypes())
                                                            .Select(x => x.Assembly)
                                                            .Distinct();

            var dataSourceNames = assembliesToSearch.SelectMany(assembly => assembly.GetTypes())
                                                    .Where(type => type.IsSubclassOf(typeof(TestBase)))
                                                    .SelectMany(x => x.GetMethods())
                                                    .SelectMany(a => a.GetCustomAttributes<DataSourceAttribute>())
                                                    .Select(x => x.DataSourceSettingName)
                                                    .Where(x => !string.IsNullOrWhiteSpace(x))
                                                    .Distinct()
                                                    .ToList();

            Logger.WriteLine("Found {0} DataSources.", dataSourceNames.Count);

            var configChanged = false;
            foreach (var dataSourceName in dataSourceNames)
            {
                try
                {
                    var lastDotIndex = dataSourceName.LastIndexOf(".");
                    if (lastDotIndex == -1)
                        throw new Exception("Please specify the fully qualified type + property, method or field.");

                    var dataName = dataSourceName.Substring(lastDotIndex + 1);
                    var typeName = dataSourceName.Substring(0, lastDotIndex);

                    var type = GetBusinessEntityType(assembliesToSearch, typeName);
                    var data = new List<object>();
                    foreach (var x in new ProviderReference(type, dataName).GetInstance())
                    {
                        data.Add(x);
                    }

                    var connectionStringName = dataSourceName + "_RuntimeDataSource";
                    var dataSourceFilePath = Path.Combine(DATASOURCES_PATH, dataSourceName + ".xml");

                    //Add connection string
                    connectionStringsSection.ConnectionStrings.Add(new ConnectionStringSettings(connectionStringName, dataSourceFilePath, "Microsoft.VisualStudio.TestTools.DataSource.XML"));

                    //Add datasource
                    var dataSource = new DataSourceElement()
                    {
                        Name = dataSourceName,
                        ConnectionString = connectionStringName,
                        DataTableName = "Row",
                        DataAccessMethod = "Sequential"
                    };

                    testConfigurationSection.DataSources.Add(dataSource);
                    configChanged = true;

                    //Create the file
                    File.WriteAllText(dataSourceFilePath, new XDocument(new XDeclaration("1.0", "utf-8", "true"), new XElement("Iterations")).ToString());

                    //Load the file
                    var doc = XDocument.Load(dataSourceFilePath);

                    //Add the iterations
                    doc.Element("Iterations").Add(

                        from iteration in data
                        select new XElement(dataSource.DataTableName,
                               new XElement("Payload", JsonConvert.SerializeObject(iteration))));

                    //Save the file
                    doc.Save(dataSourceFilePath);

                    Logger.WriteLine("Successfully created datasource: {0}, Iteration Count: {1}", dataSourceName, data.Count);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(dataSourceName + ": " + ex.ToString());
                }
            }

            if (configChanged)
            {
                appConfig.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");
                ConfigurationManager.RefreshSection("microsoft.visualstudio.testtools");
            }
        }

        private static Type GetBusinessEntityType(IEnumerable<Assembly> assemblies, string typeName)
        {
            Debug.Assert(typeName != null);

            foreach (var assembly in assemblies)
            {
                Type t = assembly.GetType(typeName, false);
                if (t != null)
                    return t;
            }

            throw new ArgumentException("Type " + typeName + " doesn't exist in the current app domain");
        }
    }
}