using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using MSTestHacks.RuntimeDataSource;

namespace MSTestHacks.Tests
{
    [TestClass]
    [AttachRuntimeDataSources(typeof(IterationTests))]
    public class IterationTests : TestBase
    {
        private int _expectedCount1 = 30;
        private int _expectedCount2 = 44;
        private int _expectedCount3 = 17;

        public IEnumerable<int> One
        {
            get
            {
                return Enumerable.Repeat(1, _expectedCount1);
            }
        }

        public IEnumerable<int> Two
        {
            get
            {
                return Enumerable.Repeat(1, _expectedCount2);
            }
        }

        public IEnumerable<int> Three
        {
            get
            {
                return Enumerable.Repeat(1, _expectedCount3);
            }
        }

        [TestMethod]
        [DataSource("One")]
        public void IterationCount_is_correct1()
        {
            Assert.AreEqual(_expectedCount1, this.TestContext.DataRow.Table.Rows.Count);
        }

        [TestMethod]
        [DataSource("Two")]
        public void IterationCount_is_correct2()
        {
            Assert.AreEqual(_expectedCount2, this.TestContext.DataRow.Table.Rows.Count);
        }

        [TestMethod]
        [DataSource("Three")]
        public void IterationCount_is_correct3()
        {
            Assert.AreEqual(_expectedCount3, this.TestContext.DataRow.Table.Rows.Count);
        }
    }
}
