using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

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

        private bool _changed = false;
        public bool Changed
        {
            get { return _changed;  }
        }

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

        private void SetValue(string xpath, string value)
        {
            XmlNode node = _data.SelectSingleNode(xpath);
            if (node == null)
            {
                AddValue(xpath, value);
            }
            else
            {
                node.InnerText = value;
                _changed = true;
            }
        }

        private void AddValue(string xpath, string value)
        {
            string[] xpathParts = xpath.Split('/');

            int index = xpathParts.Length;
            XPathNavigator dataNav = _data.CreateNavigator();
            XPathNavigator nav = null;
            while (nav == null && index > 0)
            {
                nav = dataNav.SelectSingleNode(string.Join("/", xpathParts, 0, index));
            }
            if (nav == null)
            {
            }

            //XPathNavigator nav = _data.CreateNavigator();
            //XPathNavigator temp = nav;
            //while (temp != null)
            //{
            //    temp = temp.SelectSingleNode()
            //}
        }
    }
}
