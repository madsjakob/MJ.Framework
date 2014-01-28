using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MJS.Framework.Data.Interfaces;
using MJS.Framework.Data.Types;

namespace MJS.Framework.Data.Extensions
{
    public static class DataClassKernel
    {
        public static object GetPropertyByName(this IDataClass dataClass, string propertyName)
        {
            string property = GetFirstPropertyName(ref propertyName);
            if (dataClass is IDataClassList)
            {
                IDataClassList dataClassList = (IDataClassList)dataClass;
                string stringIndex = property.Substring(1, property.Length - 2);
                int index;
                if (!int.TryParse(stringIndex, out index))
                {
                    throw new DataClassException("Index must be an integer");
                }
                if (index < 0 || index >= dataClassList.Count)
                {
                    throw new DataClassException("Index out of range");
                }
                IDataClass child = dataClassList.GetItem(index);
                if (propertyName != "")
                {
                    return child.GetPropertyByName(propertyName);
                }
                else
                {
                    return child;
                }
            }
            else
            {
                PropertyInfo info = dataClass.GetType().GetProperty(property);
                if (info == null)
                {
                    throw new DataClassException("Unknown property \'" + property + "\' for type \'" + dataClass.GetType().Name + "\'");
                }
                object value = info.GetValue(dataClass, null);
                if (propertyName == "")
                {
                    return value;
                }
                else
                {
                    return ((IDataClass)value).GetPropertyByName(propertyName);
                }
            }
        }

        public static void SetPropertyByName(this IDataClass dataClass, string propertyName, object value)
        {
            string property = GetFirstPropertyName(ref propertyName);
            if (dataClass is IDataClassList)
            {
                IDataClassList dataClassList = (IDataClassList)dataClass;
                string index = property.Substring(1, property.Length - 2);
                IDataClass child = dataClassList.GetItem(int.Parse(index));
                if (propertyName != "")
                {
                    child.SetPropertyByName(propertyName, value);
                }
                else
                {
                    dataClassList.GetItem(int.Parse(index)).Assign((IDataClass)value);
                }
            }
            else
            {
                if (value is System.DBNull)
                {
                    value = null;
                }
                PropertyInfo info = dataClass.GetType().GetProperty(property);
                if (propertyName == "")
                {
                    if (info.CanWrite)
                    {
                        info.SetValue(dataClass, value, null);
                    }
                }
                else if (typeof(IDataClass).IsAssignableFrom(info.PropertyType))
                {
                    IDataClass propertyValue = (IDataClass)info.GetValue(dataClass, null);
                    propertyValue.SetPropertyByName(propertyName, value);
                }
            }
        }

        private static string GetFirstPropertyName(ref string propertyName)
        {
            if (propertyName == null || propertyName.Trim() == "")
            {
                throw new DataClassException("DataClass Error: Kan ikke finde en property uden angivelse af navn!");
            }
            int dotIndex = propertyName.IndexOf('.');
            int leftBracketIndex = propertyName.IndexOf('[');
            int rightBracketIndex = propertyName.IndexOf(']');
            string property;
            string more;
            if (dotIndex > 0 && ((rightBracketIndex > 0 && dotIndex < rightBracketIndex) || rightBracketIndex < 0))
            {
                property = propertyName.Substring(0, dotIndex);
                more = propertyName.Substring(dotIndex + 1);

            }
            else if (leftBracketIndex == 0)
            {
                property = propertyName.Substring(0, rightBracketIndex + 1);
                more = propertyName.Substring(rightBracketIndex + 1);
                if (more.Length > 0 && more[0] == '.')
                {
                    more = more.Substring(1);
                }
            }
            else if (rightBracketIndex > 0)
            {
                property = propertyName.Substring(0, leftBracketIndex);
                more = propertyName.Substring(leftBracketIndex);
            }
            else
            {
                property = propertyName;
                more = "";
            }
            propertyName = more;
            return property;
        }

    }
}
