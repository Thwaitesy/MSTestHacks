using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSTestHacks.Framework
{
    internal static class Tools
    {
        internal static string GetBaseDirectory()
        {
            var dir = Path.Combine(Path.GetDirectoryName(new UriBuilder(Assembly.GetExecutingAssembly().GetName().CodeBase).Uri.LocalPath), "MSTestHacks");
            
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return dir;
        }
    }
}
