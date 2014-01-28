using MJS.Framework.Base.CO;
using MJS.Framework.Communication.CO;
using MJS.Framework.Data.CO;
using MJS.Framework.Data.Metadata;
using MJS.Framework.Web.DO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MJS.Framework.Web.CO
{
    public class COUser
    {
        public static bool LoadByUsername(DOUser user, string username)
        {
            string sql = "SELECT * FROM [user] WHERE username = @0";
            DataTable table = CODataAccess.Main.Endpoint.ExecuteReader(sql, username);
            bool result = table.Rows.Count == 1;
            if (result)
            {
                ReadData(user, table.Rows[0]);
            }
            return result;
        }

        private static void ReadData(DOUser user, DataRow dataRow)
        {
            DatabaseTableAttribute.ReadData(dataRow, user);
        }

        public static string GetPassword(Guid userID)
        {
            string result = null;
            string sql = "SELECT password FROM [user] WHERE id = @0";
            DataTable table = CODataAccess.Main.Endpoint.ExecuteReader(sql, userID);
            if (table.Rows.Count > 0)
            {
                result = table.Rows[0]["password"] as string;
            }
            return result;
        }

        public static void SetPassword(Guid userID, string hashedPassword)
        {
            string sql = "UPDATE [user] SET password = @0 WHERE id = @1";
            CODataAccess.Main.Endpoint.ExecuteNonQuery(sql, hashedPassword, userID);
        }
    }
}
