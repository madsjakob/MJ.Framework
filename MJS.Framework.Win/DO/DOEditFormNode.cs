using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MJS.Framework.Win.DO
{
    [Serializable]
    public class DOEditFormNode : TreeNode
    {
        public DOEditFormNode()
        {
        }
        public DOEditFormNode(string text, Type editControlType, object data)
        {
            Text = text;
            EditControlType = editControlType;
            Data = data;
        }
        public Type EditControlType { get; set; }
        public object Data { get; set; }
        public Enum[] ExtraActions { get; set; }
    }
}
