using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSTestHacks.RuntimeDataSource;

namespace MSTestHacks.Tests.RuntimeDataSource
{
    public class BaseClass : TestBase
    {
        public void Testa()
        {
            var data = this.TestContext.GetRuntimeDataSourceObject<object>();

            Assert.IsNotNull(data);
        }
    }
}
