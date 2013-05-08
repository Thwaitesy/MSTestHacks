using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestHacks.RuntimeDataSource;
using System.Collections.Generic;

namespace MSTestHacks.Tests
{
    [TestClass]
    [AttachRuntimeDataSources(typeof(SimpleObjects))]
    public class SimpleObjects : TestBase
    {
        private IEnumerable<int> Simple
        {
            get
            {
                return new List<int> { 1, 2, 3, 4, 5 };
            }
        }

        [TestMethod]
        [DataSource("Simple")]
        public void TestMethod1()
        {
            Assert.IsNotNull(this.TestContext.GetRuntimeDataSourceObject<int>());
        }
    }
}
