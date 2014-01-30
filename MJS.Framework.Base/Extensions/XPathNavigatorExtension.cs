using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace MJS.Framework.Base.Extensions
{
    public static class XPathNavigatorExtension
    {
        public static XPathNavigator Add(this XPathNavigator me, string path, string value = null)
        {
            try
            {
                while (path.StartsWith("/"))
                {
                    path = path.Substring(1);
                }
                if (path.StartsWith("@"))
                {
                    me.CreateAttribute(null, path.Substring(1), null, value);
                }
                else if (path.EndsWith("]"))
                {
                    string tempPath = path.Substring(0, path.IndexOf("["));
                    while (me.SelectSingleNode(path) == null)
                    {
                        me.AppendChildElement(null, tempPath, null, null);
                    }
                    if (value != null)
                    {
                        me.SelectSingleNode(path).SetValue(value);
                    }
                }
                else
                {
                    me.AppendChildElement(null, path, null, value);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("{0}\n{1}", ex.Message, ex.StackTrace);
            }
            return me.SelectSingleNode(path);
        }

        public static string GetValue(this XPathNavigator me, string path)
        {
            string result = null;
            XPathNavigator nav = me.SelectSingleNode(path);
            if (nav != null)
            {
                result = nav.Value;
            }
            return result;
        }
    }
}
