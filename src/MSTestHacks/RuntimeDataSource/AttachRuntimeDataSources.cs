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

                //If the test tools section doest exist, add it.
                if (!appConfig.Sections.Cast<ConfigurationSection>().Any(x => x.SectionInformation.Name == "microsoft.visualstudio.testtools"))
                {
                    appConfig.Sections.Add("microsoft.visualstudio.testtools", new Microsoft.VisualStudio.TestTools.UnitTesting.TestConfigurationSection());
                }

                var section = (TestConfigurationSection)appConfig.Sections["microsoft.visualstudio.testtools"];
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
                            //Note: This may remove a datasource that was wanted....
                            section.DataSources.Remove(attribute.DataSourceSettingName);

                            //Add datasource
                            var dataSource = new DataSourceElement()
                            {
                                Name = attribute.DataSourceSettingName,
                                ConnectionString = "RuntimeDataSource",
                                DataTableName = type.FullName + "." + method.Name,
                                DataAccessMethod = "Sequential"
                            };
                            section.DataSources.Add(dataSource);
                            configChanged = true;

                            //Get the source data
                            var sourceData = new List<object>();
                            foreach (var x in new ProviderReference(type, dataSource.Name).GetInstance())
                            {
                                sourceData.Add(x);
                            }

                            //Create file if not there.

                            if (!File.Exists("RuntimeDataSources.xml"))
                            {
                                XDocument x = new XDocument(new XDeclaration("1.0", "utf-8", "true"), new XElement("DataSources"));

                                File.WriteAllText("RuntimeDataSources.xml", x.ToString());
                            }

                            var document = XDocument.Load("RuntimeDataSources.xml");
                            document.Element("DataSources").Add(

                                from data in sourceData
                                select new XElement(dataSource.DataTableName,
                                       new XElement("Payload", JsonConvert.SerializeObject(data))));

                            document.Save("RuntimeDataSources.xml");
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
