﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using MJS.Framework.Base.Extensions;

namespace MJS.Framework.View.Types
{
    public class ViewObject<T> 
    {
        // Must be able to load both indexfields and the blob
        // Must be able to save both indexfields and the blob

        // We need an ID to save to
        // Properties must be marked with an xpath and if necessary an indexfield


        private DataCacheObject _data;

        public ViewObject(Guid id)
        {
            _data = DataCache.Cache.GetEntity(typeof(T), id);
        }

        public bool Edit()
        {
            return DataCache.Cache.LockEntity(typeof(T), _data.ID);
        }

        public void Save()
        {
            DataCache.Cache.SaveEntity(typeof(T), _data.ID);
        }

        public void Close()
        {
            DataCache.Cache.UnlockEntity(typeof(T), _data.ID);
        }


        
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

        public void SetValue(string xpath, string value)
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

        public string GetValue(string xpath)
        {
            XPathNavigator nav = _data.CreateNavigator();
            return nav.GetValue(xpath);
        }

        private void AddValue(string xpath, string value)
        {
            List<string> xpathParts = new List<string>();
            xpathParts.AddRange(xpath.Split('/'));
            if (string.IsNullOrWhiteSpace(xpathParts[0]))
            {
                xpathParts.RemoveAt(0);
                xpathParts[0] = "/" + xpathParts[0];
            }

            int index = xpathParts.Count;
            XPathNavigator dataNav = _data.CreateNavigator();
            XPathNavigator nav = null;
            while (nav == null && index > 0)
            {
                string tempXpath = string.Join("/", xpathParts.ToArray(), 0, index);
                nav = dataNav.SelectSingleNode(tempXpath);
                if (nav == null)
                {
                    index--;
                }
            }
            
            if (nav == null)
            {
                nav = dataNav;
            }
            while (index < xpathParts.Count)
            {
                nav = nav.Add(xpathParts[index]);
                index++;
            }
            if (nav != null)
            {
                nav.SetValue(value);
            }
            
        }
    }
}
