using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MJS.Framework.Base.Types
{
    public class IniFile
    {
        public IniFile(string filename)
        {
            _filename = filename;
        }

        private string _filename;

        public void WriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, _filename);
        }

        public string ReadValue(string section, string key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, _filename);
            return temp.ToString();
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
    }
}
