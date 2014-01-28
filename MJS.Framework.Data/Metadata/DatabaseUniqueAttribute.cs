using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJS.Framework.Data.Metadata
{
    public class DatabaseUniqueAttribute : DatabaseFieldAttribute
    {
        public DatabaseUniqueAttribute(string fieldName) : base(fieldName)
        {
        }
    }
}
