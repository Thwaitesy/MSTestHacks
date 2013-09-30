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
        private static HashSet<Type> typesInitalized = new HashSet<Type>();

        public AttachRuntimeDataSources(Type type)
            : base("AttachRuntimeDataSources")
        {
            if (!typesInitalized.Contains(type))
            {
                typesInitalized.Add(type);

                var appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                //If the connectionStrings section doest exist, add it.
                if (!appConfig.Sections.Cast<ConfigurationSection>().Any(x => x.SectionInformation.Name == "connectionStrings"))
                {
                    appConfig.Sections.Add("connectionStrings", new ConnectionStringsSection());
                }

                //Add in the runtimeDataSource connection string.
                var connectionStringsSection = (ConnectionStringsSection)appConfig.Sections["connectionStrings"];
                //connectionStringsSection.ConnectionStrings.Add(new ConnectionStringSettings("RuntimeDataSource", "RuntimeDataSources.xml", "Microsoft.VisualStudio.TestTools.DataSource.XML"));

                //If the test tools section doest exist, add it.
                if (!appConfig.Sections.Cast<ConfigurationSection>().Any(x => x.SectionInformation.Name == "microsoft.visualstudio.testtools"))
                {
                    appConfig.Sections.Add("microsoft.visualstudio.testtools", new Microsoft.VisualStudio.TestTools.UnitTesting.TestConfigurationSection());
                }

                var testConfigurationSection = (TestConfigurationSection)appConfig.Sections["microsoft.visualstudio.testtools"];
                var configChanged = false;

                //Go through all the methods
                foreach (var method in type.GetMethods())
                {
                    try
                    {
                        //Get the datasource attribute
                        var attribute = method.GetCustomAttribute<DataSourceAttribute>();
                        if (attribute != null && !string.IsNullOrWhiteSpace(attribute.DataSourceSettingName))
                        {

                            //Add connection string
                            connectionStringsSection.ConnectionStrings.Add(new ConnectionStringSettings(type.FullName + "." + method.Name + "_RuntimeDataSource", type.FullName + "." + method.Name + ".xml", "Microsoft.VisualStudio.TestTools.DataSource.XML"));

                            //Note: This may remove a datasource that was wanted....
                            //testConfigurationSection.DataSources.Remove(attribute.DataSourceSettingName);

                            //Add datasource
                            var dataSource = new DataSourceElement()
                            {
                                Name = attribute.DataSourceSettingName,
                                ConnectionString = type.FullName + "." + method.Name + "_RuntimeDataSource",
                                DataTableName = "Row",
                                DataAccessMethod = "Sequential"
                            };
                            testConfigurationSection.DataSources.Add(dataSource);
                            configChanged = true;

                            //Get the source data
                            var sourceData = new List<object>();
                            foreach (var x in new ProviderReference(type, dataSource.Name).GetInstance())
                            {
                                sourceData.Add(x);
                            }

                            //Create the file (if not there)
                            var fileName = "RuntimeDataSources.xml";
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
                            appConfig.Save(ConfigurationSaveMode.Modified, true);
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
