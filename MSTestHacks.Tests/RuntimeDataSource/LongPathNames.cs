using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTestHacks.Tests.RuntimeDataSource
{
    [TestClass]
    public class LongPathNames : TestBase
    {
        const string PREFIX = "MSTestHacks.Tests.RuntimeDataSource.LongPathNames.";

        /// <summary>
        /// 260 Chars (Including directory) - NOTE THIS WILL CHANGE ON USERS PC..
        /// Tested Path: C:\Git\Digital\MSTestHacks\MSTestHacks.Tests\bin\Debug\MSTestHacks\RuntimeDataSources\MSTestHacks.Tests.RuntimeDataSource.LongPathNames.  +  PropertyName
        /// </summary>
        public IEnumerable<int> XXXXXX_12ataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataData
        {
            get
            {
                return new List<int> { 1, 2, 3 };
            }
        }

        /// <summary>
        /// 261 Chars (Including directory) - NOTE THIS WILL CHANGE ON USERS PC..
        /// Tested Path: C:\Git\Digital\MSTestHacks\MSTestHacks.Tests\bin\Debug\MSTestHacks\RuntimeDataSources\MSTestHacks.Tests.RuntimeDataSource.LongPathNames.  +  PropertyName
        /// </summary>
        public IEnumerable<int> XXXXXXX_12ataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataData
        {
            get
            {
                return new List<int> { 4, 5, 6 };
            }
        }

        /// <summary>
        /// 259 Chars (Including directory) - NOTE THIS WILL CHANGE ON USERS PC..
        /// Tested Path: C:\Git\Digital\MSTestHacks\MSTestHacks.Tests\bin\Debug\MSTestHacks\RuntimeDataSources\MSTestHacks.Tests.RuntimeDataSource.LongPathNames.  +  PropertyName
        /// </summary>
        public IEnumerable<int> XXXXX_12ataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataData
        {
            get
            {
                return new List<int> { 7, 8, 9 };
            }
        }

        [TestMethod]
        [DataSource(PREFIX + "XXXXX_12ataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataData")]
        public void NamesBelow255CharsShouldSucceed()
        {
            var value = this.TestContext.GetRuntimeDataSourceObject<int>();
            Assert.IsTrue(value > 0);
        }

        [TestMethod]
        [DataSource(PREFIX + "XXXXXX_12ataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataData")]
        public void NamesWith255CharsShouldSucceed()
        {
            var value = this.TestContext.GetRuntimeDataSourceObject<int>();
            Assert.IsTrue(value > 0);
        }

        [TestMethod]
        [DataSource(PREFIX + "XXXXXXX_12ataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataDataData")]
        public void NamesAbove255CharsShouldSucceed()
        {
            var value = this.TestContext.GetRuntimeDataSourceObject<int>();
            Assert.IsTrue(value > 0);
        }
    }
}
