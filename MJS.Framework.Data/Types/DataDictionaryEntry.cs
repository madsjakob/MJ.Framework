using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MJS.Framework.Data.Enums;

namespace MJS.Framework.Data.Types
{
    public class DataDictionaryEntry
    {
        public DataType EntryDataType { get; set; }
        public object Value { get; set; }
    }
}
