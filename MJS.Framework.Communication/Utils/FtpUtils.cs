using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MJS.Framework.Communication.Utils
{
    public static class FtpUtils
    {
        public static void Upload(string server, string subpath, MemoryStream data, string username, string password)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(server + subpath);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(username, password);
            request.UsePassive = false;
            request.ContentLength = data.Length;
            Stream requestStream = request.GetRequestStream();
            {
                requestStream.Write(data.ToArray(), 0, (int)data.Length);
                requestStream.Close();
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                {
                    response.Close();
                }
            }
        }

        public static bool Exists(string server, string subpath, string username, string password)
        {
            bool result = true;
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(server + subpath);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(username, password);
                request.UsePassive = false;
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    response.Close();
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static void CreateDirectory(string server, string subpath, string username, string password)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(server + subpath);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            request.Credentials = new NetworkCredential(username, password);
            request.UsePassive = false;
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                response.Close();
            }
        }
    }
}
