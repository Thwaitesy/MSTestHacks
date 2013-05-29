using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestHacks.RuntimeDataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTestHacks.Tests
{
    [TestClass]
    [AttachRuntimeDataSources(typeof(InheritedTests))]
    public class InheritedTests : BaseClass
    {
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
        [DataSource("Simple2")]
        public void MyTestMethod()
        {
            Testa();
        }
    }
}
