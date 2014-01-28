using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJS.Framework.Base.Utils
{
    public static class ProcessUtils
    {
        public static int Run(string program, string parameters = "")
        {
            string stdout;
            string stderr;
            return Run(program, parameters, out stdout, out stderr, false);
        }

        public static int Run(string program, string parameters, out string stdout, out string stderr, bool waitForExit = true)
        {
            stdout = "";
            stderr = "";
            ProcessStartInfo startInfo = new ProcessStartInfo(program, parameters);
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            if (waitForExit)
            {
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
            }
            int result = 0;
            try
            {
                Process process = Process.Start(startInfo);
                if (waitForExit)
                {
                    stdout = process.StandardOutput.ReadToEnd();
                    stderr = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                    result = process.ExitCode;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                Console.WriteLine("\t" + program);
                Console.WriteLine("\t" + parameters);
                result = -1;
            }
            return result;
        }
    }
}
