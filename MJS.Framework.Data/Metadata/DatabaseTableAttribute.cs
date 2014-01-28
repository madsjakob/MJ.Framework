using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MJS.Framework.Base.Types;
using MJS.Framework.Data.CO;
using MJS.Framework.Data.Interfaces;

namespace MJS.Framework.Data.Metadata
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DatabaseTableAttribute : Attribute
    {
        public DatabaseTableAttribute(string tablename)
        {
            Tablename = tablename;
            BlobField = null;
        }

        public DatabaseTableAttribute(string tablename, string blobField)
        {
            Tablename = tablename;
            BlobField = blobField;
        }

        public string Tablename { get; set; }
        public string BlobField { get; set; }

        public static DatabaseTableAttribute GetTableAttribute(Type dataClassType)
        {
            return (DatabaseTableAttribute)DatabaseTableAttribute.GetCustomAttribute(dataClassType, typeof(DatabaseTableAttribute));
        }

        public static string BuildSelectQuery(IDataClass dataClass, ParameterTable parameterList)
        {
            DatabaseTableAttribute table = GetTableAttribute(dataClass.GetType());
            StringBuilder result = new StringBuilder();
            result.Append("SELECT * FROM ");
            result.Append(table.Tablename);
            DatabaseKeyAttribute key = DatabaseKeyAttribute.GetKeyAttribute(dataClass.GetType());
            if (key != null)
            {
                result.Append(" WHERE ");
                result.Append(key.FieldName);
                result.Append(" = @");
                result.Append(key.FieldName);
                parameterList.Add(key.FieldName, key.GetValue(dataClass));
            }
            parameterList.Add("0", Guid.NewGuid());
            return result.ToString();
        }

        public static string BuildSelectListQuery(Type dataClassType, string where = null, string order = null, bool includeBlob = false)
        {
            DatabaseTableAttribute table = GetTableAttribute(dataClassType);
            StringBuilder result = new StringBuilder();
            result.Append("SELECT ");
            if (includeBlob)
            {
                result.Append("* FROM ");
            }
            else
            {
                StringBuilder fieldSql = new StringBuilder();
                DatabaseFieldAttribute[] fieldList = DatabaseFieldAttribute.GetDatabaseFields(dataClassType);
                foreach (DatabaseFieldAttribute field in fieldList)
                {
                    if (fieldSql.Length > 0)
                    {
                        fieldSql.Append(", ");
                    }
                    fieldSql.Append(field.FieldName);   
                }
                result.Append(fieldSql.ToString());
                result.Append(" FROM ");
            }
            result.Append("[");
            result.Append(table.Tablename);
            result.Append("]");
            if (where != null)
            {
                result.Append(" WHERE ");
                result.Append(where);
            }
            if (order != null)
            {
                result.Append(" ORDER BY ");
                result.Append(order);
            }
            return result.ToString();
        }


        public static string BuildInsertQuery(IDataClass dataClass, ParameterTable parameterList)
        {
            DatabaseTableAttribute table = GetTableAttribute(dataClass.GetType());
            DatabaseKeyAttribute.SetKeyValue(dataClass);
            StringBuilder result = new StringBuilder();
            DatabaseFieldAttribute[] fieldList = DatabaseFieldAttribute.GetDatabaseFields(dataClass.GetType());
            StringBuilder fields = new StringBuilder();
            StringBuilder values = new StringBuilder();
            foreach (DatabaseFieldAttribute field in fieldList)
            {
                if (fields.Length > 0)
                {
                    fields.Append(", ");
                    values.Append(", ");
                }
                fields.Append(field.FieldName);
                values.Append("@");
                values.Append(field.FieldName);
                parameterList.Add(field.FieldName, field.GetValue(dataClass));
            }

            if (!string.IsNullOrWhiteSpace(table.BlobField))
            {
                if (fields.Length > 0)
                {
                    fields.Append(", ");
                    values.Append(", ");
                }
                fields.Append(table.BlobField);
                values.Append("@");
                values.Append(table.BlobField);
                using (MemoryStream ms = new MemoryStream())
                {
                    CODataMapper.DataClassToBlob(dataClass, ms);
                    parameterList.Add(table.BlobField, ms.ToArray());
                }
            }
            result.Append("INSERT INTO ");
            result.Append("[");
            result.Append(table.Tablename);
            result.Append("]");
            result.Append(" (");
            result.Append(fields.ToString());
            result.Append(") VALUES (");
            result.Append(values.ToString());
            result.Append(")");
            return result.ToString();
        }

        public static string BuildUpdateQuery(IDataClass dataClass, ParameterTable parameterList)
        {
            DatabaseTableAttribute table = GetTableAttribute(dataClass.GetType());
            StringBuilder result = new StringBuilder();
            StringBuilder update = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(table.BlobField))
            {
                update.Append(table.BlobField);
                update.Append(" = @");
                update.Append(table.BlobField);
                using (MemoryStream ms = new MemoryStream())
                {
                    CODataMapper.DataClassToBlob(dataClass, ms);
                    parameterList.Add(table.BlobField, ms.ToArray());
                }
            }
            DatabaseFieldAttribute[] fieldList = DatabaseFieldAttribute.GetDatabaseFields(dataClass.GetType());
            foreach (DatabaseFieldAttribute field in fieldList)
            {
                if (!(field is DatabaseKeyAttribute))
                {
                    if (update.Length > 0)
                    {
                        update.Append(", ");
                    }
                    update.Append(field.FieldName);
                    update.Append(" = @");
                    update.Append(field.FieldName);
                    parameterList.Add(field.FieldName, field.GetValue(dataClass));
                }
            }
            
            result.Append("UPDATE ");
            result.Append(table.Tablename);
            result.Append(" SET ");
            result.Append(update.ToString());
            DatabaseKeyAttribute key = DatabaseKeyAttribute.GetKeyAttribute(dataClass.GetType());
            if (key != null)
            {
                result.Append(" WHERE ");
                result.Append(key.FieldName);
                result.Append(" = @");
                result.Append(key.FieldName);
                parameterList.Add(key.FieldName, key.GetValue(dataClass));
            }
            
            return result.ToString();
        }

        public static string BuildDeleteQuery(IDataClass dataClass, ParameterTable parameterList)
        {
            DatabaseTableAttribute table = GetTableAttribute(dataClass.GetType());
            StringBuilder result = new StringBuilder();
            result.Append("DELETE FROM ");
            result.Append(table.Tablename);
            DatabaseKeyAttribute key = DatabaseKeyAttribute.GetKeyAttribute(dataClass.GetType());
            if (key != null)
            {
                result.Append(" WHERE ");
                result.Append(key.FieldName);
                result.Append(" = @");
                result.Append(key.FieldName);
                parameterList.Add(key.FieldName, key.GetValue(dataClass));
            }
            else
            {
                throw new Exception("I Don't feel like automatically delete all data from the table " + table.Tablename + "!");
            }
            
            return result.ToString();
        }

        public static void ReadData(DataRow row, IDataClass dataClass)
        {
            DatabaseTableAttribute table = GetTableAttribute(dataClass.GetType());
            DatabaseFieldAttribute[] fieldList = DatabaseFieldAttribute.GetDatabaseFields(dataClass.GetType());
            foreach (DatabaseFieldAttribute field in fieldList)
            {
                field.SetValue(dataClass, row);
            }
            if (!string.IsNullOrWhiteSpace(table.BlobField) && row.Table.Columns.Contains(table.BlobField))
            {
                object blobValue = row[table.BlobField];
                if (blobValue != null && blobValue != DBNull.Value)
                {
                    using (MemoryStream ms = new MemoryStream((byte[])row[table.BlobField]))
                    {
                        CODataMapper.BlobToDataClass(dataClass, ms);
                    }
                }
            }
        }
    }
}
