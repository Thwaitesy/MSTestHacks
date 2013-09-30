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
    [AttachRuntimeDataSources(typeof(SimpleObjects))]
    public class SimpleObjects : TestBase
    {
        const string PREFIX = "MSTestHacks.Tests.RuntimeDataSource.SimpleObjects.";

        private int _publicFieldCount = 221;
        public IEnumerable<int> PublicField = Enumerable.Range(0, 221);

        private int _publicPropertyCount = 11;
        public IEnumerable<int> PublicProperty
        {
            get
            {
                return Enumerable.Range(0, this._publicPropertyCount);
            }
        }

        private int _privatePropertyCount = 15;
        private IEnumerable<int> PrivateProperty
        {
            get
            {
                return Enumerable.Range(0, this._privatePropertyCount);
            }
        }

        private int _publicMethodCount = 111;
        public IEnumerable<string> PublicMethod()
        {
            return Enumerable.Repeat("some thing", this._publicMethodCount);
        }

        private int _privateMethodCount = 13;
        private IEnumerable<string> PrivateMethod()
        {
            return Enumerable.Repeat("some one", this._privateMethodCount);
        }

        #region Field Tests
        [TestMethod]
        [DataSource(PREFIX + "PublicField")]
        public void Test_Simple_Objects_Injected_From_PublicField()
        {
            //Make sure the object can be retrieved.
            var x = this.TestContext.GetRuntimeDataSourceObject<int>();

            Assert.AreEqual(this._publicFieldCount, this.TestContext.DataRow.Table.Rows.Count);
        }
        #endregion

        #region Property Tests
        [TestMethod]
        [DataSource(PREFIX + "PublicProperty")]
        public void Test_Simple_Objects_Injected_From_PublicProperty()
        {
            //Make sure the object can be retrieved.
            var x = this.TestContext.GetRuntimeDataSourceObject<int>();

            Assert.AreEqual(this._publicPropertyCount, this.TestContext.DataRow.Table.Rows.Count);
        }

        [TestMethod]
        [DataSource(PREFIX + "PrivateProperty")]
        public void Test_Simple_Objects_Injected_From_PrivateProperty()
        {
            //Make sure the object can be retrieved.
            var x = this.TestContext.GetRuntimeDataSourceObject<int>();

            Assert.AreEqual(this._privatePropertyCount, this.TestContext.DataRow.Table.Rows.Count);
        } 
        #endregion

        #region Method Tests
        [TestMethod]
        [DataSource(PREFIX + "PublicMethod")]
        public void Test_Simple_Objects_Injected_From_PublicMethod()
        {
            //Make sure the object can be retrieved.
            var x = this.TestContext.GetRuntimeDataSourceObject<string>();


            Assert.AreEqual("some thing", x);
            Assert.AreEqual(this._publicMethodCount, this.TestContext.DataRow.Table.Rows.Count);
        }

        [TestMethod]
        [DataSource(PREFIX + "PrivateMethod")]
        public void Test_Simple_Objects_Injected_From_PrivateMethod()
        {
            //Make sure the object can be retrieved.
            var x = this.TestContext.GetRuntimeDataSourceObject<string>();

            Assert.AreEqual("some one", x);
            Assert.AreEqual(this._privateMethodCount, this.TestContext.DataRow.Table.Rows.Count);
        } 
        #endregion
    }
}
