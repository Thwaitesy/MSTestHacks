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
using System.Xml.Linq;

namespace MSTestHacks.RuntimeDataSource
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AttachRuntimeDataSources : ContextAttribute
    {
        private string DATASOURCES_PATH = Path.Combine(Path.GetDirectoryName(new UriBuilder(Assembly.GetExecutingAssembly().GetName().CodeBase).Uri.LocalPath), "RuntimeDataSources");
        private static HashSet<Type> typesInitalized = new HashSet<Type>();

        public AttachRuntimeDataSources(Type type)
            : base("AttachRuntimeDataSources")
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

            if (!Directory.Exists(DATASOURCES_PATH))
                Directory.CreateDirectory(DATASOURCES_PATH);

            var configChanged = false;

            if (!typesInitalized.Contains(type))
            {
                typesInitalized.Add(type);

                //Go through all the methods
                foreach (var method in type.GetMethods())
                {
                    try
                    {
                        //Get the datasource attribute
                        var attribute = method.GetCustomAttribute<DataSourceAttribute>();
                        if (attribute != null && !string.IsNullOrWhiteSpace(attribute.DataSourceSettingName))
                        {
                            var dataSourceName = attribute.DataSourceSettingName;
                            var connectionStringName = attribute.DataSourceSettingName + "_RuntimeDataSource";
                            var dataSourceFilePath = Path.Combine(DATASOURCES_PATH, dataSourceName + ".xml");
                            var lastIndexOfDot = dataSourceName.LastIndexOf(".");
                            if (lastIndexOfDot == -1)
                                throw new Exception("Please specify the fully qualified type + property.");

                            var refernceName = dataSourceName.Substring(lastIndexOfDot + 1);
                            var typeName = dataSourceName.Substring(0, lastIndexOfDot);

                            if (typeName != type.FullName)
                                continue;

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

                            //Get the source data
                            var sourceData = new List<object>();

                            //var lastIndexOfDot = dataSourceName.LastIndexOf(".");
                            //var refernceName = dataSourceName.Substring(lastIndexOfDot);
                            //var typeName = dataSourceName.Substring(0, lastIndexOfDot);
                            //var typex = Type.GetType(typeName);
                            foreach (var x in new ProviderReference(type, refernceName).GetInstance())
                            {
                                sourceData.Add(x);
                            }

                            //Create the file (if not there)
                            var fileName = dataSourceFilePath;
                            if (!File.Exists(fileName))
                                File.WriteAllText(fileName, new XDocument(new XDeclaration("1.0", "utf-8", "true"), new XElement("Rows")).ToString());

                            //Load the file
                            var doc = XDocument.Load(fileName);

                            //Remove all elements with the same name
                            doc.Element("Rows").Elements(dataSource.DataTableName).Remove();

                            //Add the iterations
                            doc.Element("Rows").Add(

                                from data in sourceData
                                select new XElement(dataSource.DataTableName,
                                       new XElement("Data", JsonConvert.SerializeObject(data))));

                            //Save the file
                            doc.Save(fileName);
                        }

                        if (configChanged)
                        {
                            appConfig.Save(ConfigurationSaveMode.Modified);
                            ConfigurationManager.RefreshSection("connectionStrings");
                            ConfigurationManager.RefreshSection("microsoft.visualstudio.testtools");
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex);
                    }
                }
            }
        }
    }
}
