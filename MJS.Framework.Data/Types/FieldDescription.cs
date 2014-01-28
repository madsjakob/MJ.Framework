using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJS.Framework.Data.Types
{
    public class FieldDescription
    {
        public string Name { get; set; }
        public SqlDbType DataType { get; set; }
        public int Length { get; set; }
        public bool AllowNull { get; set; }
    }

    public class FieldDescriptionList : List<FieldDescription>
    {
    }
}
