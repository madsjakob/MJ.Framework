using MJS.Framework.Base.Utils;
using MJS.Framework.Data.CO;
using MJS.Framework.Web.CO;
using MJS.Framework.Web.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MJS.Framework.Web.BO
{
    public static class BOUser
    {
        public static bool OneUserRegistered()
        {
            DOUserList userList = new DOUserList();
            CODataClass.LoadList(userList);
            return userList.Count > 0;
        }

        public static bool Login(string username, string password, bool passwordHashed = false)
        {
            DOUser user = new DOUser();
            bool result = COUser.LoadByUsername(user, username);
            if (result)
            {
                result = Login(user, password, passwordHashed);
            }
            return result;
        }

        public static bool Login(DOUser user, string password, bool passwordHashed = false)
        {
            bool result = false;
            if (!passwordHashed)
            {
                password = StringUtils.MD5Hash(password);
            }
            string storedPassword = COUser.GetPassword(user.ID);
            result = storedPassword != null && storedPassword == password;
            return result;
        }

        public static void SetPassword(DOUser user, string password)
        {
            COUser.SetPassword(user.ID, StringUtils.MD5Hash(password));
        }
    }
}
