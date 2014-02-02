using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJS.Framework.View.Types
{
    public class DataObjectAttribute : Attribute
    {
        public DataObjectAttribute(string table, string keyField)
        {
            Table = table;
            KeyField = keyField;
        }

        public string Table { get; set; }
        public string KeyField { get; set; }
        public string BlobField { get; set; }
    }
}
