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
            me.AppendChildElement(null, path, null, value);
            return me.SelectSingleNode(path);
        }
    }
}
