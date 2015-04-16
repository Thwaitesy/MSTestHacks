using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

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