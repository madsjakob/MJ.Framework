using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MJS.Framework.Base.Utils;

namespace MJS.Framework.Base.Types
{
    public class StreamNode
    {
        private object _id;
        public object ID
        {
            get { return _id; }
            set { _id = value; }
        }
        private object[] _children;
        public object[] Children
        {
            get { return _children; }
            set { _children = value; }
        }

        public StreamNode()
        {
        }
        
        public StreamNode(object id, params object[] children)
        {
            _id = id;
            _children = children;
        }

        private StreamNode(object id, int childCount)
        {
            _id = id;
            _children = new object[childCount];
        }

        public void LoadFromStream(Stream stream)
        {
            _id = StreamUtils.ReadValue(stream);
            int childCount = (int)StreamUtils.ReadValue(stream);
            _children = new object[childCount];
            for (int index = 0; index < childCount; index++)
            {
                _children[index] = StreamUtils.ReadValue(stream);
                if (_children[index] == typeof(StreamNode))
                {
                    _children[index] = CreateChildNode();
                    ((StreamNode)_children[index]).LoadFromStream(stream);
                }
            }
        }

        protected virtual StreamNode CreateChildNode()
        {
            return new StreamNode();
        }

        public void SaveToStream(Stream stream)
        {
            StreamUtils.WriteValue(stream, _id);
            StreamUtils.WriteValue(stream, _children.Length);
            foreach (object currentObject in _children)
            {
                StreamUtils.WriteValue(stream, currentObject);
                if (currentObject is StreamNode)
                {
                    (currentObject as StreamNode).SaveToStream(stream);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(StreamNode))
            {
                return false;
            }
            else
            {
                StreamNode temp = (StreamNode)obj;
                bool result = ((ID == null && temp.ID == null) || ID.Equals(temp.ID));
                result = result && (Children.Length == temp.Children.Length);                    
                if (result)
                {
                    for (int index = 0; index < Children.Length; index++)
                    {
                        if (Children[index] == null)
                        {
                            result = result && (temp.Children[index] == null);
                        }
                        else
                        {
                            result = result && Children[index].Equals(temp.Children[index]);
                        }
                        if (!result)
                        {
                            break;
                        }
                    }
                }
                return result;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
