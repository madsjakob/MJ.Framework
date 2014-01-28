using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace MJS.Framework.Base.Utils
{
    public static class ZLibUtils
    {
        public static Stream CompressStream(Stream stream)
        {
            return new DeflaterOutputStream(stream);
        }

        public static Stream DeCompressStream(Stream stream)
        {
            return new InflaterInputStream(stream);
        }


        public static byte[] CompressData(byte[] data)
        {
            byte[] result;
            MemoryStream ms = new MemoryStream();
            try
            {
                Stream zipstream = CompressStream(ms);
                try
                {
                    zipstream.Write(data, 0, data.Length);
                }
                finally
                {
                    zipstream.Close();
                }
                result = ms.ToArray();
            }
            finally
            {
                ms.Close();
            }
            return result;
        }

        public static byte[] DecompressData(byte[] data)
        {
            byte[] result;
            MemoryStream ms = new MemoryStream(data);
            Stream zipstream = DeCompressStream(ms);
            try
            {
                result = StreamUtils.ReadStreamContent(zipstream);
            }
            finally
            {
                zipstream.Close();
                ms.Close();
            }
            return result;
        }
    }
}
