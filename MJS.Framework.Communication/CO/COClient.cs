using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MJS.Framework.Base.Types;
using MJS.Framework.Base.Utils;
using MJS.Framework.Communication.Enums;
using MJS.Framework.Communication.Interfaces;
using MJS.Framework.Communication.Utils;
using System.Data.Linq;
using MJS.Framework.Base.CO;
using System.Runtime.InteropServices;

namespace MJS.Framework.Communication.CO
{
    public class COClient : ICommunicationEndpoint
    {
        public string Server { get; set; }

        private byte[] _session = new byte[SessionUtils.SessionDataLength];
        private int _attempts = 3;

        public Guid ApplicationKey
        {
            get
            {
                Assembly assembly = WindowsUtils.GetEntryAssembly();
                GuidAttribute attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
                return new Guid(attribute.Value);
            }
        }

        public byte[] UserCredentials
        {
            get { return null; }
        }

        public COClient(string server)
        {
            Server = server;
        }

        public CODataContext Context
        {
            get { return null; }
        }

        public Table<T> GetTable<T>() where T : class
        {
            Table<T> table = null;
            if (Context != null)
            {
                table = Context.GetTable<T>();
            }
            return table;
        }

        public void Call(MemoryStream input, MemoryStream output)
        {
            byte result;
            int count = 0;
            do
            {
                try
                {
                    result = HttpUtils.HttpBinaryRequest(Server + "Data/Index/", _session, input, output);
                }
                catch (WebException e)
                {
                    throw new WebException("Kommunikation med HDAP fejlede", e);
                }
                if (result == 10)
                {
                    result = Authenticate(output, result);
                }
                count++;
            }
            while (count < _attempts && result == 10);
            if (result != 0)
            {
                string error = Encoding.Default.GetString(output.ToArray());
                throw new Exception(error);
            }
        }

        private byte Authenticate(MemoryStream output, byte result)
        {
            using (MemoryStream authStream = new MemoryStream())
            {
                StreamUtils.WriteValue(authStream, ApplicationKey);
                using (MemoryStream authResponse = new MemoryStream())
                {
                    byte res = HttpUtils.HttpBinaryRequest(Server + "Data/Authenticate/", authStream, authResponse);
                    if (res == 0)
                    {
                        authResponse.Read(_session, 0, _session.Length);
                    }
                    else if (res == 255)
                    {
                        result = 255;
                        output.Write(authResponse.ToArray(), 0, (int)authResponse.Length);
                    }
                }
            }
            return result;
        }

        private MemoryStream CreateCallStream(ServerAction call)
        {
            MemoryStream stream = new MemoryStream();
            StreamUtils.WriteValue(stream, (int)call);
            return stream;
        }



        public int ExecuteNonQuery(string sql, params object[] parameters)
        {
            int result = 0;
            using (MemoryStream sqlStream = new MemoryStream())
            {
                StreamUtils.WriteValue(sqlStream, sql);
                StreamUtils.WriteValue(sqlStream, parameters.Length);
                for (int index = 0; index < parameters.Length; index++)
                {
                    StreamUtils.WriteValue(sqlStream, "" + index);
                    StreamUtils.WriteValue(sqlStream, parameters[index]);
                }
                result = ExecuteNonQuery(sqlStream);
            }
            return result;
        }

        public int ExecuteNonQuery(string sql, ParameterTable parameters)
        {
            int result = 0;
            using (MemoryStream sqlStream = new MemoryStream())
            {
                StreamUtils.WriteValue(sqlStream, sql);
                StreamUtils.WriteValue(sqlStream, parameters.Count);
                foreach (string key in parameters.Keys)
                {
                    StreamUtils.WriteValue(sqlStream, key);
                    StreamUtils.WriteValue(sqlStream, parameters[key]);
                }
                result = ExecuteNonQuery(sqlStream);
            }

            return result;
        }

        public int ExecuteNonQuery(Stream sqlStream)
        {
            int result = 0;
            using (MemoryStream stream = CreateCallStream(ServerAction.ExecuteNonQuery))
            {
                StreamUtils.WriteStreamToStream(stream, sqlStream);
                using (MemoryStream resultStream = new MemoryStream())
                {
                    Call(stream, resultStream);
                    stream.Position = 0;
                    result = (int)StreamUtils.ReadValue(resultStream);
                }
            }
            return result;
        }

        public DataTable ExecuteReader(string sql, params object[] parameters)
        {
            DataTable result;
            using (MemoryStream sqlStream = new MemoryStream())
            {
                StreamUtils.WriteValue(sqlStream, sql);
                StreamUtils.WriteValue(sqlStream, parameters.Length);
                for (int index = 0; index < parameters.Length; index++)
                {
                    StreamUtils.WriteValue(sqlStream, "" + index);
                    StreamUtils.WriteValue(sqlStream, parameters[index]);
                }
                result = ExecuteReader(sqlStream);
            }
            return result;
        }

        public DataTable ExecuteReader(string sql, ParameterTable parameters)
        {
            DataTable result;
            using (MemoryStream sqlStream = new MemoryStream())
            {
                StreamUtils.WriteValue(sqlStream, sql);
                StreamUtils.WriteValue(sqlStream, parameters.Count);
                foreach (string key in parameters.Keys)
                {
                    StreamUtils.WriteValue(sqlStream, key);
                    StreamUtils.WriteValue(sqlStream, parameters[key]);
                }
                result = ExecuteReader(sqlStream);
            }
            return result;
        }


        public DataTable ExecuteReader(Stream sqlStream)
        {
            DataTable table = new DataTable();
            using (MemoryStream stream = CreateCallStream(ServerAction.ExecuteReader))
            {
                sqlStream.Position = 0;
                StreamUtils.WriteStreamToStream(stream, sqlStream);
                using (MemoryStream resultStream = new MemoryStream())
                {
                    Call(stream, resultStream);
                    stream.Position = 0;
                    DataTableUtils.StreamToDataTable(resultStream, table);
                }
            }
            return table;
        }

        private void LoadTable(DataTable table, MemoryStream stream)
        {
            string fieldString = (string)StreamUtils.ReadValue(stream);
            table.Columns.Clear();

            string[] fieldNames = fieldString.Split(new char[] { '\n' });
            foreach(string fieldName in fieldNames)
            {
                DataColumn column = table.Columns.Add();
                column.ColumnName = fieldName;
            }
            int rowCount = StreamUtils.ReadIntValue(stream);
            for (int index = 0; index < rowCount; index++)
            {
                int fieldCount = StreamUtils.ReadIntValue(stream);
                object[] data = new object[fieldCount];
                for(int columnIndex = 0; columnIndex < fieldCount; columnIndex++)
                {
                    object value = StreamUtils.ReadValue(stream);
                    if (index == 0)
                    {
                        table.Columns[columnIndex].DataType = value.GetType();
                    }
                    data[columnIndex] = value;
                }
                table.Rows.Add(data);
            }
        }

    }
}
