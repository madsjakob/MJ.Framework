using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MJS.Framework.Base.Utils
{
    public static class FileUtils
    {
        public static string GetTempPath()
        {
            return Path.Combine(GetApplicationDataPath(), @"Temp\");
        }

        public static string GetApplicationDataPath()
        {
            string result = null;
            Assembly assembly = Assembly.GetEntryAssembly();
            if (assembly != null)
            {
                result = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), assembly.GetName().Name);
            }
            return result;
        }
    }
}
