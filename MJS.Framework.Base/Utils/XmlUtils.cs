using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Xml;
using System.IO;
using System.Xml.XPath;

namespace MJS.Framework.Base.Utils
{
    public class XmlUtils
    {
        private const string _w3DateTimeFormat = "yyyy-MM-ddThh:mm:ss";
        private const string _standardEncoding = "iso-8859-1";

        private static string[] _bannedDictionaryProperties = new string[] { "item", "comparer", "keys", "values" };
        public static string[] BannedDictionaryProperties
        {
            get { return _bannedDictionaryProperties; }
        }

        private static string[] _bannedListProperties = new string[] { "item" };
        public static string[] BannedListProperties
        {
            get { return _bannedListProperties; }
        }

        
        public static string ObjectToXmlValue(object value)
        {
            string xmlValue = null;
            if (value != null)
            {
                if (value is DateTime)
                {
                    xmlValue = ((DateTime)value).ToString(_w3DateTimeFormat);
                }
                else if (value is double)
                {
                    CultureInfo enUS = new CultureInfo("en-US");
                    xmlValue = ((double)value).ToString(enUS);
                }
                else
                {
                    xmlValue = value.ToString();
                }
            }
            return xmlValue;
        }

        public static object XmlValueToObject(string xmlValue, Type type)
        {
            object value = null;
            if (type.IsEnum)
            {
                value = Enum.Parse(type, xmlValue);
            }
            else if (type == typeof(int))
            {
                int intValue;
                int.TryParse(xmlValue, out intValue);
                value = intValue;
            }
            else if (type == typeof(double))
            {
                CultureInfo enUS = new CultureInfo("en-US");
                double doubleValue;
                double.TryParse(xmlValue, NumberStyles.Any, enUS, out doubleValue);
                value = doubleValue;
            }
            else if (type == typeof(Guid))
            {
                value = new Guid(xmlValue);
            }
            else if (type == typeof(DateTime))
            {
                DateTime dateTimeValue;
                DateTime.TryParse(xmlValue, out dateTimeValue);
                value = dateTimeValue;
            }
            else if (type == typeof(bool))
            {
                bool boolValue;
                bool.TryParse(xmlValue, out boolValue);
                value = boolValue;
            }
            else if (type == typeof(string))
            {
                value = xmlValue;
            }
            return value;
        }

        public static void XmlToStream(XmlDocument xmlDoc, Stream stream)
        {
            using (XmlTextWriter xtw = new XmlTextWriter(stream, Encoding.GetEncoding(_standardEncoding)))
            {
                xmlDoc.Save(xtw);
            }
        }

        public static XPathDocument StreamToXPathDocument(Stream stream)
        {
            StreamReader sr = new StreamReader(stream, Encoding.GetEncoding(_standardEncoding));
            try
            {
                using (XmlReader xr = XmlReader.Create(sr, null))
                {
                    sr = null;
                    return new XPathDocument(xr);
                }
            }
            finally
            {
                if (sr != null)
                {
                    sr.Dispose();
                }
            }
        }
    }
}
