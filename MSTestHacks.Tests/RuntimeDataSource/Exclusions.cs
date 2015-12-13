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
    public class Exclusions : TestBase
    {
        const string PREFIX = "MSTestHacks.Tests.RuntimeDataSource.Exclusions.";

        public IEnumerable<int> PublicProperty { get { return new[] { 1, 2, 3, 4, 5 }; } }

        [TestMethod]
        [DataSource(PREFIX + "PublicProperty")]
        public void Exclusions_ExcludedProperty_GeneratesDataSourceNotFoundError()
        {
            //After uncommenting out the app.config ExclusionRegEx key this test should fail with a
            //Data source 'xxxxxxx' cannot be found in the test configuration settings error
            var x = this.TestContext.GetRuntimeDataSourceObject<int>();
            Assert.IsTrue(x > 0);
        }
    }
}
