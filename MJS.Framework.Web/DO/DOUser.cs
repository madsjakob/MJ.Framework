using MJS.Framework.Data.Metadata;
using MJS.Framework.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJS.Framework.Web.DO
{
    [DatabaseTable("user")]
    public class DOUser : Entity
    {
        [DatabaseKey("id")]
        public Guid ID
        {
            get { return GetID(); }
            set { SetID(value); }
        }
        [DatabaseField("username")]
        public string Username { get; set; }
        [DatabaseField("email")]
        public string Email { get; set; }
    }

    public class DOUserList : DataClassList<DOUser>
    {
    }
}
