using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace MJS.Framework.Win.SO
{
    public partial class SOSplashControl : UserControl
    {
        public SOSplashControl()
        {
            InitializeComponent();
        }

        public void SetData(AssemblyName main, AssemblyName[] refs)
        {
            _main = main;
            _refs = refs;
            Measure();
            Invalidate();
        }

        private AssemblyName _main;
        private AssemblyName[] _refs;
        private int _y;


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_main != null)
            {
                _y = 0;
                Font mainFont = new Font(Font.FontFamily, 14f, FontStyle.Bold);
                
                DrawAssembly(e.Graphics, _main, mainFont);
                if (_refs != null)
                {
                    foreach (AssemblyName name in _refs)
                    {
                        if (name.Name.StartsWith("System") || name.Name == "mscorlib")
                        {
                            continue;
                        }
                        DrawAssembly(e.Graphics, name, Font);
                    }
                }
            }
        }

        private void Measure()
        {
            if(_main!= null)
            {
                _y = 0;
                Font mainFont = new Font(Font.FontFamily, 14f, FontStyle.Bold);
                MeasureAssembly(_main, mainFont);
                if (_refs != null)
                {
                    foreach (AssemblyName name in _refs)
                    {
                        if (name.Name.StartsWith("System") || name.Name == "mscorlib")
                        {
                            continue;
                        }
                        MeasureAssembly(name, Font);
                    }
                }
                Parent.ClientSize = new Size(Parent.Width, _y);
            }
        }

        private void MeasureAssembly(AssemblyName name, Font font)
        {
            string text = name.Name + " v" + name.Version;
            Size size = TextRenderer.MeasureText(text, font);
            _y += size.Height;
        }

        private void DrawAssembly(Graphics graphics, AssemblyName name, Font font)
        {
            string text = name.Name + " v" + name.Version;
            Size size = TextRenderer.MeasureText(text, font);
            Point p = new Point(0, _y);
            _y += size.Height;
            TextRenderer.DrawText(graphics, text, font, p, ForeColor);

        }
    }
}
