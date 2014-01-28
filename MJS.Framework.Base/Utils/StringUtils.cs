using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MJS.Framework.Base.Utils
{
    public static class StringUtils
    {
        private const string _encoding = "ISO-8859-1";

        public static byte[] StringToByteArray(string source)
        {
            return Encoding.GetEncoding(_encoding).GetBytes(source);
        }

        public static string ByteArrayToString(byte[] source)
        {
            return Encoding.GetEncoding(_encoding).GetString(source);
        }

        public static string ByteArrayToHexString(byte[] byteArray)
        {
            StringBuilder result = new StringBuilder();
            for (int index = 0; index < byteArray.Length; index++)
            {
                result.Append(byteArray[index].ToString("x2"));
            }
            return result.ToString();
        }

        public static byte[] HexStringToByteArray(string text)
        {
            List<byte> result = new List<byte>();
            string temp;
            for (int stringIndex = 0; stringIndex < text.Length; stringIndex += 2)
            {
                temp = text.Substring(stringIndex, 2);
                result.Add(byte.Parse(temp, NumberStyles.AllowHexSpecifier));
            }
            return result.ToArray();
        }

        public static string MD5Hash(string stringToHash)
        {
            return MD5Hash(stringToHash, "");
        }

        public static string MD5Hash(string stringToHash, string salt)
        {
            return MD5Hash(stringToHash, salt, 1);
        }

        public static string MD5Hash(string stringToHash, int hashCount)
        {
            return MD5Hash(stringToHash, "", hashCount);
        }

        public static string MD5Hash(string stringToHash, string salt, int hashCount)
        {
            string hash = stringToHash;
            MD5 md5 = MD5.Create();
            for (int index = 0; index < hashCount; index++)
            {
                hash = ByteArrayToHexString(md5.ComputeHash(StringToByteArray(hash + salt)));
            }
            return hash;
        }

        public static bool VerifyEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return true;
            }
            else
            {
                Regex regex = new Regex(@"^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,4})$");
                return regex.IsMatch(email);
            }

            //else if (email.Contains("@"))
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }
    }
}
