using MJS.Framework.Win.Delegates;
using MJS.Framework.Win.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MJS.Framework.Win.SO
{
    public partial class SODialogForm : SOBaseForm
    {
        public SODialogForm()
        {
            InitializeComponent();
        }

        public static bool Run(string title, Type soType, CustomEventHandler eventHandler, object data)
        {
            SODialogForm form = new SODialogForm();
            form.Text = title;
            form.EventHandler += eventHandler;
            form.Data = data;
            form.SetControl(soType);
            return (form.ShowDialog() == DialogResult.OK);
        }

        private void SetControl(Type soType)
        {
            Control = (SOBaseControl)Activator.CreateInstance(soType);
            Control.Parent = pnlMain;
            Control.Dock = DockStyle.Fill;
            Control.SetEventHandler(DispatchEvent);
            Control.DispatchEvent(ControlEvent.Create);
            Control.SetData(Data);
            Control.DispatchEvent(ControlEvent.Update);
        }

        private SOBaseControl Control { get; set; }
        private object Data { get; set; }
    }
}
