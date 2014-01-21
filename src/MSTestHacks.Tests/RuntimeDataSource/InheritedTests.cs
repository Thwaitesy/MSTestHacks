using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestHacks.RuntimeDataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTestHacks.Tests.RuntimeDataSource
{
    [TestClass]
    public class InheritedTests : BaseClass
    {
        const string PREFIX = "MSTestHacks.Tests.RuntimeDataSource.InheritedTests.";

        public InheritedTests()
        {

        }

        public IEnumerable<int> Simple2
        {
            get
            {
                return new List<int> { 1, 22, 33 };
            }
        }

        [TestMethod]
        [DataSource(PREFIX + "Simple2")]
        public void InheritedTest()
        {
            Testa();
        }
    }
}
