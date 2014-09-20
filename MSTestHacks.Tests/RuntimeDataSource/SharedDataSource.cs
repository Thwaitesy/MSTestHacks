using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestHacks.RuntimeDataSource;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;
using System.Linq;

namespace MSTestHacks.Tests.RuntimeDataSource
{
    [TestClass]
    public class SharedDataSource : TestBase
    {
        const string SHARED_DATASOURCE = "MSTestHacks.Tests.RuntimeDataSource.SharedDataSource.SharedData";

        private int _sharedDataCount = 145;
        public IEnumerable<int> SharedData
        {
            get
            {
                return Enumerable.Range(0, this._sharedDataCount);
            }
        }

        [TestMethod]
        [DataSource(SHARED_DATASOURCE)]
        public void Test_Shared_DataSource1()
        {
            //Make sure the object can be retrieved.
            var x = this.TestContext.GetRuntimeDataSourceObject<int>();

            Assert.AreEqual(this._sharedDataCount, this.TestContext.DataRow.Table.Rows.Count);
        }

        [TestMethod]
        [DataSource(SHARED_DATASOURCE)]
        public void Test_Shared_DataSource2()
        {
            //Make sure the object can be retrieved.
            var x = this.TestContext.GetRuntimeDataSourceObject<int>();

            Assert.AreEqual(this._sharedDataCount, this.TestContext.DataRow.Table.Rows.Count);
        } 
    }
}
