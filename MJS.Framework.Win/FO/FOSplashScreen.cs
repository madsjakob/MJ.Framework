using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MJS.Framework.Win.SO;

namespace MJS.Framework.Win.FO
{
    public class FOSplashScreen
    {
        public static void ShowSplash()
        {
            Assembly main = Assembly.GetEntryAssembly();
            AssemblyName mainName = main.GetName();
            AssemblyName[] refAssemblies = main.GetReferencedAssemblies();
            SOSplashScreen form = new SOSplashScreen();
            form.SetData(mainName, refAssemblies);
            form.Show();
            for (int index = 0; index < 100; index++)
            {
                Application.DoEvents();
                Thread.Sleep(50);
            }
            form.Close();
        }
    }
}
