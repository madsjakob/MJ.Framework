using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace MJS.Framework.Win.SO
{
    public partial class SOSplashScreen : Form
    {
        public SOSplashScreen()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
        }

        public void SetData(AssemblyName main, AssemblyName[] refs)
        {
            scSplash.SetData(main, refs);
        }
    }
}
