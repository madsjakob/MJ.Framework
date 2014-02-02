using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJS.Framework.View.Types
{
    public class DataObjectAttribute : Attribute
    {
        public DataObjectAttribute(string table, string keyField, string updatedField)
        {
            Table = table;
            KeyField = keyField;
            UpdatedField = updatedField;
        }

        public string Table { get; set; }
        public string KeyField { get; set; }
        public string UpdatedField { get; set; }
        public string BlobField { get; set; }
    }
}
