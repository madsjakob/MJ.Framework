using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MJS.Framework.Win.Delegates;
using MJS.Framework.Win.Enums;
using MJS.Framework.Base.Utils;
using System.IO;
using System.Xml.XPath;
using MJS.Framework.Data.CO;
using MJS.Framework.Win.DO;

namespace MJS.Framework.Win.SO
{
    public partial class SOBaseForm : Form
    {
        public SOBaseForm()
        {
            InitializeComponent();
            _openForms.Add(this);
        }

        ~SOBaseForm()
        {
        }

        public string Filename
        {
            get
            {
                string result = null;
                string appDataPath = FileUtils.GetApplicationDataPath();
                if (appDataPath != null)
                {
                    string dir = Path.Combine(appDataPath, "Forms");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    result = Path.Combine(dir, Name + ".xml");
                }
                return result;
            }
        }

        public void LoadSettings()
        {
            string filename = Filename;
            if (filename != null && !IsMdiChild && File.Exists(filename))
            {
                DOFormSettings settings = new DOFormSettings();
                CODataMapper.XmlFileToDataClass(settings, filename);
                if (settings.State == FormWindowState.Normal)
                {
                    Width = settings.Width;
                    Height = settings.Height;
                    Location = new Point(settings.X, settings.Y);
                }
                WindowState = settings.State;
            }
            else
            {
                WindowState = FormWindowState.Maximized;
            }
        }

        public void SaveSettings()
        {
            string filename = Filename;
            if (!IsMdiChild && filename != null)
            {
                DOFormSettings settings = new DOFormSettings();
                if (WindowState == FormWindowState.Normal)
                {
                    settings.Width = Width;
                    settings.Height = Height;
                    settings.X = Location.X;
                    settings.Y = Location.Y;
                }
                settings.State = WindowState;
                CODataMapper.DataClassToXmlFile(settings, filename);
            }
        }

        public void SetEventHandler(CustomEventHandler eventHandler)
        {
            EventHandler += eventHandler;
            foreach(Control control in Controls)
            {
                if (control is SOBaseControl)
                {
                    (control as SOBaseControl).SetEventHandler(eventHandler);
                }
            }
        }

        protected event CustomEventHandler _eventHandler;
        public event CustomEventHandler EventHandler
        {
            add { _eventHandler += value; }
            remove { _eventHandler -= value; }
        }

        public virtual void DispatchEvent(SOBaseControl control, Enum action)
        {
            if (_eventHandler != null)
            {
                _eventHandler(control, action);
            }
        }

        private void SOBaseForm_Load(object sender, EventArgs e)
        {
            LoadSettings();
            DispatchEvent(null, ControlEvent.Create);
            DispatchEvent(null, ControlEvent.Update);
        }

        private void SOBaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DispatchEvent(null, ControlEvent.Destroy);
            if (!e.Cancel)
            {
                SaveSettings();
            }
        }

        private static List<SOBaseForm> _openForms = new List<SOBaseForm>();

        public static void UpdateAll()
        {
            foreach (SOBaseForm form in _openForms)
            {
                form.DispatchEvent(null, ControlEvent.Update);
            }
        }

        private void SOBaseForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _openForms.Remove(this);
        }
    }
}
