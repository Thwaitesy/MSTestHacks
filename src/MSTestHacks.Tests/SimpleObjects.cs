using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestHacks.RuntimeDataSource;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;
using System.Linq;

namespace MSTestHacks.Tests
{
    [TestClass]
    [AttachRuntimeDataSources(typeof(SimpleObjects))]
    public class SimpleObjects : TestBase
    {
        public IEnumerable<int> Simple
        {
            get
            {
                return new List<int> { 1, 2, 3, 4, 5 };
            }
        }

        public IEnumerable<int> Simple2
        {
            get
            {
                return new List<int> { 1, 22, 33 };
            }
        }

        [TestMethod]
        [DataSource("Simple2")]
        public void TestMethod1()
        {
            var x = this.TestContext.GetRuntimeDataSourceObject<int>();
        }

        [TestMethod]
        [DataSource("Simple")]
        public void TestMethod12()
        {
            var x = this.TestContext.GetRuntimeDataSourceObject<int>();
        }
    }
}
