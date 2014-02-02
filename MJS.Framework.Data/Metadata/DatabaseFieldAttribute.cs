using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MJS.Framework.Data.Interfaces;
using MJS.Framework.Base.Utils;

namespace MJS.Framework.Data.Metadata
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class DatabaseFieldAttribute : Attribute
    {
        public DatabaseFieldAttribute(string fieldName)
        {
            FieldName = fieldName;
            PropertyName = null;
        }

        public DatabaseFieldAttribute(string fieldName, string propertyName)
        {
            FieldName = fieldName;
            PropertyName = propertyName;
        }

        public string FieldName { get; set; }
        public string PropertyName { get; set; }

        public static DatabaseFieldAttribute[] GetDatabaseFields(Type dataClassType)
        {
            List<DatabaseFieldAttribute> result = new List<DatabaseFieldAttribute>();
            result.AddRange((DatabaseFieldAttribute[])DatabaseFieldAttribute.GetCustomAttributes(dataClassType, typeof(DatabaseFieldAttribute)));
            PropertyInfo[] propertyList = dataClassType.GetProperties();
            foreach (PropertyInfo property in propertyList)
            {
                DatabaseFieldAttribute[] tempList = (DatabaseFieldAttribute[])DatabaseFieldAttribute.GetCustomAttributes(property, typeof(DatabaseFieldAttribute));
                foreach (DatabaseFieldAttribute temp in tempList)
                {
                    temp.PropertyName = property.Name;
                }
                result.AddRange(tempList);
            }
            return result.ToArray();
        }

        public virtual object GetValue(IDataClass dataClass)
        {
            PropertyInfo property = dataClass.GetType().GetProperty(PropertyName);
            return ToSqlValue(property.GetValue(dataClass, null));
        }

        public virtual void SetValue(IDataClass dataClass, DataRow row)
        {
            PropertyInfo property = dataClass.GetType().GetProperty(PropertyName);
            object value = FromSqlValue(property.PropertyType, row[FieldName]);
            property.SetValue(dataClass, value, null);
        }

        private object FromSqlValue(Type type, object sqlValue)
        {
            return SqlUtils.FromSqlValue(type, sqlValue);
        }

        private object ToSqlValue(object value)
        {
            return SqlUtils.ToSqlValue(value);
        }
    }
}
