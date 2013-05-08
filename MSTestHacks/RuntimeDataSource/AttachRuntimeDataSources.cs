using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestHacks.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;

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
                                DataTableName = Guid.NewGuid().ToString(),
                                DataAccessMethod = "Sequential"
                            };
                            section.DataSources.Add(dataSource);
                            configChanged = true;

                            //Get the source data
                            var sourceData = new ProviderReference(type, dataSource.Name).GetInstance();
                            using (var db = new RuntimeDataSourceDataContext(ConfigurationManager.ConnectionStrings["RuntimeDataSource"].ConnectionString))
                            {
                                //Add table
                                var addTableQuery = string.Format(@"IF OBJECT_ID('{0}', 'U') IS NOT NULL
                                                                        DROP TABLE [{0}]
                                                                    CREATE TABLE [{0}] 
                                                                        ([Payload] [varchar](MAX) NOT NULL)", dataSource.DataTableName);
                                db.ExecuteQuery<object>(addTableQuery);

                                //Insert data
                                foreach (var data in sourceData)
                                {
                                    var json = JsonConvert.SerializeObject(data);
                                    var insertDataQuery = string.Format(@"INSERT INTO [{0}]
			                                                                ([Payload]) 
		                                                                  VALUES ('{1}')",
                                                                            dataSource.DataTableName, json);
                                    db.ExecuteQuery<object>(insertDataQuery);
                                }

                                //Save all changes
                                db.SubmitChanges();
                            }
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
