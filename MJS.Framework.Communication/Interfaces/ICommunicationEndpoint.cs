using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MJS.Framework.Base.Types;
using MJS.Framework.Communication.CO;
using System.Data.Linq;
using MJS.Framework.Base.CO;

namespace MJS.Framework.Communication.Interfaces
{
    public interface ICommunicationEndpoint
    {
        void Call(MemoryStream inputStream, MemoryStream outputStream);
        int ExecuteNonQuery(string sql, params object[] parameters);
        int ExecuteNonQuery(string sql, ParameterTable parameters);
        int ExecuteNonQuery(Stream sqlStream);
        DataTable ExecuteReader(string sql, params object[] parameters);
        DataTable ExecuteReader(string sql, ParameterTable parameters);
        DataTable ExecuteReader(Stream sqlStream);
        CODataContext Context { get; }
        Table<T> GetTable<T>() where T: class;
    }
}
