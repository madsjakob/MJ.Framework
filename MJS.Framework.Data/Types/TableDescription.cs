using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJS.Framework.Data.Types
{
    public class TableDescription
    {
        public string Name { get; set; }
        private FieldDescriptionList _fields = new FieldDescriptionList();
        public FieldDescriptionList Fields
        {
            get { return _fields; }
        }
    }
}
    