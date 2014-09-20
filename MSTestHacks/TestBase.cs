using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestHacks.RuntimeDataSource;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTestHacks
{
    [AttachRuntimeDataSources]
    public class TestBase : ContextBoundObject
    {
        public TestContext TestContext { get; set; }
    }
}