using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.XPath;
using MJS.Framework.Base.Utils;
using MJS.Framework.Data.Enums;
using MJS.Framework.Data.Interfaces;
using MJS.Framework.Data.Types;

namespace MJS.Framework.Data.Extensions
{
    public static class XPathNavigatorExtension
    {
        public static void ReadDataClass(this XPathNavigator me, IDataClass dataClass)
        {
            Type dataClassType = dataClass.GetType();
            PropertyInfo[] propertyList = dataClassType.GetProperties();
            XPathNodeIterator childList = me.SelectChildren(XPathNodeType.Element);
            if (dataClass is IDataClassList)
            {
                IDataClassList list = (dataClass as IDataClassList);
                XPathNodeIterator itemList = me.Select("item");
                IDataClass temp;
                foreach (XPathNavigator item in itemList)
                {
                    temp = list.GetInstance();
                    item.ReadDataClass(temp);
                    list.Add(temp);
                }
            }
            if (dataClass is IDataDictionary)
            {
                IDataDictionary dictionary = (dataClass as IDataDictionary);
                XPathNodeIterator itemList = me.Select("item");
                DataDictionaryEntry entry;
                foreach (XPathNavigator item in itemList)
                {
                    entry = new DataDictionaryEntry();
                    XPathNavigator key = item.SelectSingleNode("key");
                    XPathNavigator type = item.SelectSingleNode("type");
                    XPathNavigator value = item.SelectSingleNode("value");
                    entry.EntryDataType = (DataType)XmlUtils.XmlValueToObject(type.Value, typeof(DataType));
                    entry.Value = value.GetValue(entry.EntryDataType);
                    dictionary.Add(key.Value, entry);
                }
            }
            foreach (XPathNavigator child in childList)
            {
                PropertyInfo property = null;
                foreach (PropertyInfo temp in propertyList)
                {
                    if (child.Name.ToLower() == temp.Name.ToLower())
                    {
                        property = temp;
                        break;
                    }
                }
                if (property != null && property.CanRead)
                {
                    if (dataClass is IDataClassList && XmlUtils.BannedListProperties.Contains(property.Name.ToLower()))
                    {
                        continue;
                    }
                    if (dataClass is IDataDictionary && XmlUtils.BannedDictionaryProperties.Contains(property.Name.ToLower()))
                    {
                        continue;
                    }
                    object value = null;
                    if (typeof(IDataClass).IsAssignableFrom(property.PropertyType))
                    {
                        value = property.GetValue(dataClass, null);
                        if (value == null)
                        {
                            value = Activator.CreateInstance(property.PropertyType);
                        }
                        child.ReadDataClass((IDataClass)value);
                    }
                    else
                    {
                        value =  XmlUtils.XmlValueToObject(child.Value, property.PropertyType);
                    }
                    if (property.CanWrite)
                    {
                        property.SetValue(dataClass, value, null);
                    }
                }
            }
        }

        private static object GetValue(this XPathNavigator me, DataType dataType)
        {
            Type type = null;
            switch (dataType)
            {
                case DataType.String:
                    type = typeof(string);
                    break;
                case DataType.Integer:
                    type = typeof(int);
                    break;
                case DataType.Double:
                    type = typeof(double);
                    break;
                case DataType.Guid:
                    type = typeof(Guid);
                    break;
                case DataType.DateTime:
                    type = typeof(DateTime);
                    break;
                case DataType.Boolean:
                    type = typeof(bool);
                    break;
            }
            return XmlUtils.XmlValueToObject(me.Value, type);
        }
    }
}
