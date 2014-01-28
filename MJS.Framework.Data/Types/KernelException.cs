using System;
using System.Collections.Generic;
using System.Text;

namespace MJS.Framework.Data.Types
{
    public class DataClassException : Exception
    {
        public DataClassException() : base()
        {
        }

        public DataClassException(string message) : base(message)
        {
        }

        public DataClassException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
