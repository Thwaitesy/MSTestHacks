using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTestHacks
{
    public class TestBase : ContextBoundObject
    {
        public TestContext TestContext { get; set; }
    }
}