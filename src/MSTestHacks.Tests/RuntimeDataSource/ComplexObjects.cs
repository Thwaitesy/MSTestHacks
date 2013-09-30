using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestHacks.RuntimeDataSource;
using System.Collections.Generic;

namespace MSTestHacks.Tests.RuntimeDataSource
{
    [TestClass]
    [AttachRuntimeDataSources(typeof(ComplexObjects))]
    public class ComplexObjects : TestBase
    {
        const string PREFIX = "MSTestHacks.Tests.RuntimeDataSource.ComplexObjects.";

        public IEnumerable<XYZ> ComplexField = new List<XYZ> 
        { 
            new XYZ(1, "test 1"),
            new XYZ(2, "test 2"),
            new XYZ(3, "test 3")
        };

        public IEnumerable<XYZ> ComplexProperty
        {
            get
            {
                return new List<XYZ> 
                { 
                    new XYZ(1, "test 1"),
                    new XYZ(2, "test 2"),
                    new XYZ(3, "test 3")
                };
            }
        }

        public IEnumerable<XYZ> ComplexMethod()
        {
            return new List<XYZ> 
            { 
                new XYZ(1, "test 1"),
                new XYZ(2, "test 2"),
                new XYZ(3, "test 3")
            };
        }

        [TestMethod]
        [DataSource(PREFIX + "ComplexField")]
        public void Complete_Object_Can_Be_Injected_From_Field()
        {
            var x = this.TestContext.GetRuntimeDataSourceObject<XYZ>();
            
            Assert.IsNotNull(x);
        }

        [TestMethod]
        [DataSource(PREFIX + "ComplexProperty")]
        public void Complete_Object_Can_Be_Injected_From_Property()
        {
            var x = this.TestContext.GetRuntimeDataSourceObject<XYZ>();

            Assert.IsNotNull(x);
        }

        [TestMethod]
        [DataSource(PREFIX + "ComplexMethod")]
        public void Complete_Object_Can_Be_Injected_From_Method()
        {
            var x = this.TestContext.GetRuntimeDataSourceObject<XYZ>();

            Assert.IsNotNull(x);
        }
    }

    public class XYZ
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public XYZ(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
