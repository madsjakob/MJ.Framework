using MJS.Framework.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MJS.Framework.Win.DO
{
    public class DOFormSettings : DataClass
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public FormWindowState State { get; set; }
    }
}
