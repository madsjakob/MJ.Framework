using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MJS.Framework.Communication.CO
{
    public class COFtp
    {
        public COFtp(string server, string username, string password)
        {
            _server = server;
            _username = username;
            _password = password;

        }

        private string _server;
        private string _username;
        private string _password;
        private string _currentSubPath;
        private bool _usePassive;

        public bool Upload(string localFilename, string targetFilename)
        {
            using (FileStream fs = new FileStream(localFilename, FileMode.Open))
            {
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                return Upload(targetFilename, data);
            }
        }

        public bool Upload(string filename, MemoryStream data)
        {
            return Upload(filename, data.ToArray());
        }

        public bool Upload(string filename, byte[] data)
        {
            bool result = false;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_server + _currentSubPath + filename);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            request.Credentials = new NetworkCredential(_username, _password);
            request.UsePassive = _usePassive;
            request.ContentLength = data.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
            }

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                result = response.StatusCode == FtpStatusCode.CommandOK;
                response.Close();
            }
            return result;
        }
    }
}
