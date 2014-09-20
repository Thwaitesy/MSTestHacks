using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTestHacks
{
    public static class Extensions
    {
        public static T GetRuntimeDataSourceObject<T>(this TestContext testContext)
        {
            return JsonConvert.DeserializeObject<T>(testContext.DataRow["Payload"].ToString());
        }
    }
}