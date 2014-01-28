using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MJS.Framework.Data.Metadata
{
    public class DatabaseForeignKeyAttribute : DatabaseFieldAttribute
    {
        public DatabaseForeignKeyAttribute(string fieldname) : base(fieldname)
        {
        }

        public DatabaseForeignKeyAttribute(string fieldname, string propertyName) : base(fieldname, propertyName)
        { 
        }
    }
}
