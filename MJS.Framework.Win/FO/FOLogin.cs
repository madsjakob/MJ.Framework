using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MJS.Framework.Base.Utils;
using MJS.Framework.Win.Delegates;
using MJS.Framework.Win.Enums;
using MJS.Framework.Win.SO;

namespace MJS.Framework.Win.FO
{
    public class FOLogin
    {
        private static string CredentialsFilename
        {
            get { return Path.Combine(FileUtils.GetApplicationDataPath(), "credentials"); }
        }

        private static string Username
        {
            get
            {
                string result = "";
                if (File.Exists(CredentialsFilename))
                {
                    using (StreamReader sr = new StreamReader(CredentialsFilename))
                    {
                        result = sr.ReadLine().Trim();
                    }
                }
                return result;
            }
            set
            {
                using (StreamWriter sr = new StreamWriter(CredentialsFilename, false))
                {
                    sr.WriteLine(value);
                }
            }
        }

        public static bool Login(string username, string password)
        {
            FOLogin loginFlow = new FOLogin();
            username = Username;
            return loginFlow.DoLogin(username, password);
        }

        private SOLogin _loginForm;
        private bool DoLogin(string username, string password)
        {
            _loginForm = new SOLogin();
            _loginForm.Username = username;
            _loginForm.EventHandler += LoginHandler;
            bool result = false;
            if (_loginForm.ShowDialog() == DialogResult.OK)
            {
                Username = _loginForm.Username;
                result = true;
            }
            return result;
        }

        private void LoginHandler(SOBaseControl sender, Enum action)
        {
            if (action is LoginEvent)
            {
                LoginEvent eventID = (LoginEvent)action;
                switch (eventID)
                {
                    case LoginEvent.Login:
                        bool cancel = true;
                        if (_loginFunction != null)
                        {
                            cancel = !_loginFunction(_loginForm.Username, _loginForm.Password);
                        }
                        _loginForm.Cancel = cancel;
                        break;
                }
            }
        }

        private static event LoginFunction _loginFunction;
        public static event LoginFunction LoginFunction
        {
            add { _loginFunction += value; }
            remove { _loginFunction -= value; }
        }

    }
}
