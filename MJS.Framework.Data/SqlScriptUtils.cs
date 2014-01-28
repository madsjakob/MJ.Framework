using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MJS.Framework.Data.Metadata;
using MJS.Framework.Data.Types;

namespace MJS.Framework.Data
{
    public static class SqlScriptUtils
    {
        public static string CreateTableScript(Type dataClassType)
        {
            StringBuilder result = new StringBuilder();
            if (typeof(DataClass).IsAssignableFrom(dataClassType))
            {
                DatabaseTableAttribute tableAttribute = (DatabaseTableAttribute)Attribute.GetCustomAttribute(dataClassType, typeof(DatabaseTableAttribute));
                if (tableAttribute != null)
                {
                    Dictionary<string, string> fieldList = new Dictionary<string, string>();
                    result.AppendLine("CREATE TABLE " + tableAttribute.Tablename);
                    result.AppendLine("(");
                    // Append fields
                    AddFields(fieldList, dataClassType);
                    if(!string.IsNullOrWhiteSpace(tableAttribute.BlobField))
                    {
                        // SqlDbType.Image
                        fieldList.Add(tableAttribute.BlobField, "Image");
                    }
                    foreach (string key in fieldList.Keys)
                    {
                        result.AppendFormat("\t{0} {1}", key, fieldList[key]);
                        if (key != fieldList.Keys.Last())
                        {
                            result.Append(",");
                        }
                        result.AppendLine();
                    }
                    // Keys and other stuff
                    result.AppendLine(")");
                    
                }
            }
            return result.ToString();
        }

        private static void AddFields(Dictionary<string, string> fieldList, Type dataClassType)
        {
            PropertyInfo[] propertyList = dataClassType.GetProperties();
            foreach (PropertyInfo info in propertyList)
            {
                DatabaseFieldAttribute[] fieldAttributes = (DatabaseFieldAttribute[])DatabaseFieldAttribute.GetCustomAttributes(info, typeof(DatabaseFieldAttribute));
                foreach (DatabaseFieldAttribute field in fieldAttributes)
                {
                    fieldList.Add(field.FieldName, GetSqlType(info.PropertyType));
                }
            }
        }

        private static string GetSqlType(Type type)
        {
            string result = "varchar(50)";
            if (type == typeof(string))
            {
                result = "VARCHAR(255)";
            }
            else if (type == typeof(int))
            {
                result = "INT";
            }
            else if (type == typeof(double))
            {
                result = "FLOAT";
            }
            else if (type == typeof(Guid))
            {
                result = "UNIQUEIDENTIFIER";
            }
            return result;
        }
    }
}
