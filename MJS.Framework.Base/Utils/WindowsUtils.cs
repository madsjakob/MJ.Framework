using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace MJS.Framework.Base.Utils
{
    public static class WindowsUtils
    {
        public static Assembly GetEntryAssembly()
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null && HttpContext.Current != null && HttpContext.Current.ApplicationInstance != null)
            {
                Type type = HttpContext.Current.ApplicationInstance.GetType();
                while (type != null && type.Namespace == "ASP")
                {
                    type = type.BaseType;
                }
                entryAssembly = type.Assembly;
            }
            return entryAssembly;
        }

        public static string GetApplicationDir()
        {
            string result = null;
            try
            {
                string extra = null;
                Assembly entryAssembly = Assembly.GetEntryAssembly();
                if (entryAssembly == null && HttpContext.Current != null && HttpContext.Current.ApplicationInstance != null)
                {
                    Type type = HttpContext.Current.ApplicationInstance.GetType();
                    while (type != null && type.Namespace == "ASP")
                    {
                        type = type.BaseType;
                    }
                    entryAssembly = type.Assembly;
                    extra = "..";
                }
                string codebase = entryAssembly.CodeBase;
                UriBuilder uri = new UriBuilder(codebase);
                result = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
                if (extra != null)
                {
                    result = Path.Combine(result, extra);
                }
            }
            catch
            {
                throw new Exception("Kan ikke finde ud af hvor entryassembly er!");
            }
            return result;
        }
    }
}
