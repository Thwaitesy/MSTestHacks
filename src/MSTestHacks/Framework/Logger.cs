using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSTestHacks.Framework
{
    internal static class Logger
    {
        internal static string LOG_PATH = Path.Combine(Tools.GetBaseDirectory(), "debug.txt");

        internal static void WriteLine(string line)
        {
            using (var writer = File.AppendText(LOG_PATH))
            {
                writer.WriteLine(string.Format("[{0}] {1}", DateTime.Now.ToString("dd-MM-yy hh:mm:ss"), line));
            }
        }

        internal static void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }
    }
}