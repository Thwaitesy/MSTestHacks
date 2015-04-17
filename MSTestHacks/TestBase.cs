using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestHacks.RuntimeDataSource;
using System;

namespace MSTestHacks
{
    [AttachRuntimeDataSources]
    public class TestBase : ContextBoundObject
    {
        public TestContext TestContext { get; set; }
    }
}