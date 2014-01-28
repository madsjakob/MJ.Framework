using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace MJS.Framework.Base.Utils
{
    public class CryptUtils
    {
        public static void Crypt(byte[] data, byte[] key)
        {
            bool keyOk = false;
            foreach (byte b in key)
            {
                keyOk = keyOk || b != 0;
            }
            if (!keyOk)
            {
                throw new Exception("Key must have other values than 0");
            }
            for (int index = 0; index < data.Length; index++)
            {
                data[index] = (byte)(data[index] ^ key[(index % key.Length)]);
            }
        }

        public static byte[] GetKey(string keyBase)
        {
            using(MD5 md5 = MD5.Create())
            {
                return md5.ComputeHash(Encoding.Default.GetBytes(keyBase));
            }
        }
    }
}
