using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestHacks.RuntimeDataSource;
using System.Collections.Generic;

namespace MSTestHacks.Tests
{
    [TestClass]
    [AttachRuntimeDataSources(typeof(ComplexObjects))]
    public class ComplexObjects : TestBase
    {
        private IEnumerable<XYZ> Complex
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

        [TestMethod]
        [DataSource("Complex")]
        public void TestMethod1()
        {
            var x = this.TestContext.GetRuntimeDataSourceObject<XYZ>();
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
