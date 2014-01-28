using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MJS.Framework.Data.Interfaces;

namespace MJS.Framework.Data.Metadata
{
    [AttributeUsage(AttributeTargets.All, Inherited=true)]
    public class DatabaseKeyAttribute : DatabaseFieldAttribute
    {
        public DatabaseKeyAttribute(string fieldName) : base(fieldName)
        {
        }

        public static DatabaseKeyAttribute GetKeyAttribute(Type dataClassType)
        {
            DatabaseKeyAttribute result = null;
            foreach (DatabaseFieldAttribute temp in GetDatabaseFields(dataClassType))
            {
                if (temp is DatabaseKeyAttribute)
                {
                    result = (DatabaseKeyAttribute)temp;
                }
            }
            return result;
        }

        public static Guid GetKeyValue(IDataClass dataClass)
        {
            Guid result = Guid.Empty;
            DatabaseKeyAttribute temp = GetKeyAttribute(dataClass.GetType());
            if (temp != null)
            {
                result = (Guid)temp.GetValue(dataClass);
            }
            return result;
        }

        public static void SetKeyValue(IDataClass dataClass)
        {
            DatabaseKeyAttribute key = GetKeyAttribute(dataClass.GetType());
            if (key != null)
            {
                if ((Guid)key.GetValue(dataClass) == Guid.Empty)
                {
                    PropertyInfo property = dataClass.GetType().GetProperty(key.PropertyName);
                    property.SetValue(dataClass, Guid.NewGuid(), null);
                }
            }
        }

    }
}
