using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Cache;

namespace MJS.Framework.Base.Utils
{
    public class HttpUtils
    {
        public static byte HttpBinaryRequest(string url, byte[] session, MemoryStream dataStream, MemoryStream outputStream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(session, 0, session.Length);
                ms.Write(dataStream.ToArray(), 0, (int)dataStream.Length);
                ms.Position = 0;
                return HttpBinaryRequest(url, ms, outputStream);
            }
        }

        public static byte HttpBinaryRequest(string url, MemoryStream dataStream, MemoryStream outputStream)
        {
            byte result = 0;
            byte[] data = dataStream.ToArray();
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            try
            {
                request.Method = "POST";
                request.ContentType = "binary/octetstream";
                request.ServicePoint.Expect100Continue = false;
                request.ServicePoint.UseNagleAlgorithm = false;
                request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                request.ContentLength = data.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(data, 0, data.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    int responseLength = (int)response.ContentLength;
                    if (responseLength > 0)
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            result = (byte)responseStream.ReadByte();
                            data = StreamUtils.ReadStreamContentLength(responseStream, responseLength);
                        }
                    }
                    else
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            result = (byte)responseStream.ReadByte();
                            data = StreamUtils.ReadStreamContent(responseStream);
                        }
                    }
                    response.Close();
                }
            }
            catch (WebException e)
            {
                throw new WebException("Failed to communicate with url: " + url, e);
            }
            outputStream.SetLength(data.Length);
            outputStream.Write(data, 0, data.Length);
            outputStream.Position = 0;
            return result;
        }
    }
}
