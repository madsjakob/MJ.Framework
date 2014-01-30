using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MJS.Framework.View.Types
{
    public class ViewObject
    {
        // Must be able to load both indexfields and the blob
        // Must be able to save both indexfields and the blob

        // We need an ID to save to
        // Properties must be marked with an xpath and if necessary an indexfield

        private Guid _id;
        private Type _entityType;
        private byte[] _blob;
        private XmlDocument _data;

        public bool Dirty
        {
            get { return false; }
        }
        public DateTime LoadTime
        {
            get { return DateTime.Now; }
        }
        public bool EditMode
        {
            get { return false; }
        }
        public bool Locked
        {
            get { return false; }
        }

        public bool Load()
        {
            return false;
        }

        public void Save()
        {
        }
    }
}
