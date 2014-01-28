using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace MJS.Framework.Base.CO
{
    public class CODataContext : DataContext
    {

        public CODataContext(string connectionString) : base(connectionString)
        {
            
        }

        [Function(Name = "NEWID", IsComposable = true)]
        public Guid Random()
        { // to prove not used by our C# code... 
            throw new NotImplementedException();
        }
    }
}