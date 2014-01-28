using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MJS.Framework.Win.Enums;

namespace MJS.Framework.Win.SO
{
    public partial class SOLogin : SOBaseForm
    {
        public SOLogin()
        {
            InitializeComponent();
        }

        public string Username
        {
            get { return tbUsername.Text; }
            set { tbUsername.Text = value; }
        }

        public string Password
        {
            get { return tbPassword.Text; }
        }

        private void SOLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                DispatchEvent(null, LoginEvent.Login);
                e.Cancel = Cancel;
            }
        }

        public bool Cancel { get; set; }

        private void SOLogin_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbUsername.Text))
            {
                tbUsername.Focus();
            }
            else
            {
                tbPassword.TabIndex = 0;
            }
        }

        
    }
}
