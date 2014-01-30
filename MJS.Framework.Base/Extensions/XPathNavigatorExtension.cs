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
            while (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }
            if (path.StartsWith("@"))
            {
                me.CreateAttribute(null, path.Substring(1), null, value);
            }
            else
            {
                me.AppendChildElement(null, path, null, value);
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
