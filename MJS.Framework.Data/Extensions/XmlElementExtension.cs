
using System;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using MJS.Framework.Base.Utils;
using MJS.Framework.Data.Interfaces;

namespace MJS.Framework.Data.Extensions
{
    public static class XmlElementExtension
    {

        public static XmlElement Add(this XmlElement me, string name, object value = null)
        {
            XmlElement element = me.OwnerDocument.CreateElement(name);
            if (value != null)
            {
                if (value is IDataClass)
                {
                    element.AddDataClass(value as IDataClass);
                }
                else
                {
                    element.SetText(value);
                }
            }
            me.AppendChild(element);
            return element;
        }

        public static void SetText(this XmlElement me, object value)
        {
            XmlText text = me.OwnerDocument.CreateTextNode(XmlUtils.ObjectToXmlValue(value));
            me.AppendChild(text);
        }

        public static void AddDataClass(this XmlElement me, IDataClass dataClass)
        {
            Type dataClassType = dataClass.GetType();
            if (dataClass is IDataClassList)
            {
                IDataClassList list = dataClass as IDataClassList;
                foreach (IDataClass child in list)
                {
                    XmlElement item = me.Add("item");
                    item.AddDataClass(child);
                }
            }
            if (dataClass is IDataDictionary)
            {
                IDataDictionary dictionary = dataClass as IDataDictionary;
                foreach (string key in dictionary.Keys)
                {
                    XmlElement item = me.Add("item");
                    item.Add("key", key);
                    item.Add("type", dictionary[key].EntryDataType);
                    item.Add("value", dictionary[key].Value);
                }
            }
            PropertyInfo[] propertyList = dataClassType.GetProperties();
            foreach (PropertyInfo property in propertyList)
            {
                if (Attribute.IsDefined(property, typeof(XmlIgnoreAttribute)))
                {
                    continue;
                }
                if (dataClass is IDataClassList && property.Name.ToLower() == "item")
                {
                    continue;
                }
                if (dataClass is IDataDictionary && XmlUtils.BannedDictionaryProperties.Contains(property.Name.ToLower()))
                {
                    continue;
                }
                object value = property.GetValue(dataClass, null);
                XmlElement element = me.Add(property.Name.ToLower(), value);
            }

        }
    }
}
