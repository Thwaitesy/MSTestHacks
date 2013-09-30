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
    [AttachRuntimeDataSources(typeof(SimpleObjects2))]
    public class SimpleObjects2 : TestBase
    {
        private int _publicPropertyCount = 55;
        public IEnumerable<int> PublicProperty
        {
            get
            {
                return Enumerable.Range(0, this._publicPropertyCount);
            }
        }

        [TestMethod]
        [DataSource("PublicProperty")]
        public void Test_Simple_Objects_Injected_From_PublicProperty()
        {
            //Make sure the object can be retrieved.
            var x = this.TestContext.GetRuntimeDataSourceObject<int>();

            Assert.AreEqual(this._publicPropertyCount, this.TestContext.DataRow.Table.Rows.Count);
        }
    }
}
