using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MJS.Framework.Base.Types;
using MJS.Framework.Communication.CO;
using MJS.Framework.Communication.Interfaces;
using MJS.Framework.Base.Utils;

namespace MJS.Framework.Communication.CO
{
    public class CODataAccess
    {
        public static string ConfigFile { get; set; }

        private static CODataAccess _main;
        public static CODataAccess Main
        {
            get
            {
                if (_main == null)
                {
                    _main = new CODataAccess();
                    if (string.IsNullOrWhiteSpace(ConfigFile))
                    {
                        ConfigFile = Path.Combine(WindowsUtils.GetApplicationDir(), "data.ini");
                    }
                    _main.Configure();
                }
                return _main;
            }
        }

        public void Configure()
        {
            if (File.Exists(ConfigFile))
            {
                IniFile config = new IniFile(ConfigFile);
                string type = config.ReadValue("Main", "type");
                if (type.ToLower() == "server")
                {
                    _endpoint = new COServer(config.ReadValue("Server", "connectionstring"));
                }
                else if (type.ToLower() == "client")
                {
                    _endpoint = new COClient(config.ReadValue("Client", "serveraddress"));
                }
            }
            else
            {
                throw new Exception(ConfigFile  + " does not exist!");
            }
        }

        private ICommunicationEndpoint _endpoint;
        public ICommunicationEndpoint Endpoint
        {
            get { return _endpoint; }
        }
    }
}
