using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MJS.Framework.View.Types
{
    public class DataCacheObject
    {
        public Guid ID { get; set; }
        public Type DataType { get; set; }
        public DateTime Loaded { get; set; }
        public byte[] Blobdata { get; set; }
        public XmlDocument XmlData { get; private set; }
    }
}
