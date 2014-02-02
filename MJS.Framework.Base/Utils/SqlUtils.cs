using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MJS.Framework.Base.Utils
{
    public static class SqlUtils
    {
        public static object FromSqlValue(Type type, object sqlValue)
        {
            object value = null;
            if (sqlValue == DBNull.Value)
            {
                value = null;
            }
            else if (type.IsEnum)
            {
                value = Enum.Parse(type, sqlValue as String);
            }
            else if (type == typeof(Guid))
            {
                value = new Guid(sqlValue.ToString());
            }
            else
            {
                value = sqlValue;
            }
            return value;
        }

        public static object ToSqlValue(object value)
        {
            object sqlValue = null;
            if (value != null)
            {
                if (value.GetType().IsEnum)
                {
                    sqlValue = value.ToString();
                }
                else
                {
                    sqlValue = value;
                }
            }
            return sqlValue;
        }
    }
}
