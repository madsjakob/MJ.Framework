using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MJS.Framework.Base.CO;
using MJS.Framework.Base.Types;
using MJS.Framework.Base.Utils;
using MJS.Framework.Communication.Enums;
using MJS.Framework.Communication.Interfaces;
using System.Data.Linq;

namespace MJS.Framework.Communication.CO
{
    public class COServer : ICommunicationEndpoint
    {
        public COServer(string connectionString)
        {
            _database = new CODatabase(connectionString);
            _context = new CODataContext(connectionString);
        }

        private CODataContext _context;
        public CODataContext Context
        {
            get { return _context; }
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

        private CODatabase _database;

        public void Call(MemoryStream inputStream, MemoryStream outputStream)
        {
            ServerAction call = (ServerAction)StreamUtils.ReadValue(inputStream);
            try
            {
                switch (call)
                {
                    case ServerAction.ExecuteNonQuery:
                        DoExecuteNonQuery(inputStream, outputStream);
                        break;
                    case ServerAction.ExecuteReader:
                        DoExecuteReader(inputStream, outputStream);
                        break;
                    case ServerAction.Copy:
                        DoCopy(inputStream, outputStream);
                        break;
                    case ServerAction.Delete:
                        DoDelete(inputStream, outputStream);
                        break;
                    case ServerAction.Exists:
                        DoExists(inputStream, outputStream);
                        break;
                    case ServerAction.GetSize:
                        DoGetSize(inputStream, outputStream);
                        break;
                    case ServerAction.GetTime:
                        DoGetTime(inputStream, outputStream);
                        break;
                    case ServerAction.Load:
                        DoLoad(inputStream, outputStream);
                        break;
                    case ServerAction.Rename:
                        DoRename(inputStream, outputStream);
                        break;
                    case ServerAction.Save:
                        DoSave(inputStream, outputStream);
                        break;
                    case ServerAction.Search:
                        DoSearch(inputStream, outputStream);
                        break;
                    case ServerAction.SetTime:
                        DoSetTime(inputStream, outputStream);
                        break;
                    case ServerAction.CreateDirectory:
                        DoCreateDirectory(inputStream, outputStream);
                        break;
                    default:
                        throw new Exception("Unknown controller call!");
                }
            }
            catch
            {
                throw;
            }
        }

        private void DoExecuteNonQuery(MemoryStream inputStream, MemoryStream outputStream)
        {
            using (MemoryStream sqlStream = new MemoryStream())
            {
                StreamUtils.ReadStreamFromStream(inputStream, sqlStream);
                int result = ExecuteNonQuery(sqlStream);
                StreamUtils.WriteValue(outputStream, result);
            }
        }

        private void DoExecuteReader(MemoryStream inputstream, MemoryStream outputStream)
        {
            using (MemoryStream sqlStream = new MemoryStream())
            {
                StreamUtils.ReadStreamFromStream(inputstream, sqlStream);
                DataTable result = ExecuteReader(sqlStream);
                DataTableUtils.DataTableToStream(outputStream, result);
                outputStream.Position = 0;
            }
        }

        private void DoCopy(MemoryStream inputStream, MemoryStream outputStream)
        {
            throw new NotImplementedException();
        }

        private void DoDelete(MemoryStream inputStream, MemoryStream outputStream)
        {
            throw new NotImplementedException();
        }

        private void DoExists(MemoryStream inputStream, MemoryStream outputStream)
        {
            throw new NotImplementedException();
        }

        private void DoGetSize(MemoryStream inputStream, MemoryStream outputStream)
        {
            throw new NotImplementedException();
        }

        private void DoGetTime(MemoryStream inputStream, MemoryStream outputStream)
        {
            throw new NotImplementedException();
        }

        private void DoLoad(MemoryStream inputStream, MemoryStream outputStream)
        {
            throw new NotImplementedException();
        }

        private void DoRename(MemoryStream inputStream, MemoryStream outputStream)
        {
            throw new NotImplementedException();
        }

        private void DoSave(MemoryStream inputStream, MemoryStream outputStream)
        {
            throw new NotImplementedException();
        }

        private void DoSearch(MemoryStream inputStream, MemoryStream outputStream)
        {
            throw new NotImplementedException();
        }

        private void DoSetTime(MemoryStream inputStream, MemoryStream outputStream)
        {
            throw new NotImplementedException();
        }

        private void DoCreateDirectory(MemoryStream inputStream, MemoryStream outputStream)
        {
            throw new NotImplementedException();
        }


        public int ExecuteNonQuery(string sql, params object[] parameters)
        {
            return _database.ExecuteNonQuery(sql, parameters);
        }

        public int ExecuteNonQuery(string sql, ParameterTable parameters)
        {
            return _database.ExecuteNonQuery(sql, parameters);
        }

        public int ExecuteNonQuery(Stream sqlStream)
        {
            string sql = (string)StreamUtils.ReadValue(sqlStream);
            int parameterCount = (int)StreamUtils.ReadValue(sqlStream);
            ParameterTable parameterList = new ParameterTable();
            string parameterName;
            object parameterValue;
            for (int index = 0; index < parameterCount; index++)
            {
                parameterName = (string)StreamUtils.ReadValue(sqlStream);
                parameterValue = StreamUtils.ReadValue(sqlStream);
                parameterList.Add(parameterName, parameterValue);
            }
            return ExecuteNonQuery(sql, parameterList);
        }

        public DataTable ExecuteReader(string sql, params object[] parameters)
        {
            return _database.ExecuteReader(sql, parameters);
        }

        public DataTable ExecuteReader(string sql, ParameterTable parameters)
        {
            return _database.ExecuteReader(sql, parameters);
        }

        public DataTable ExecuteReader(Stream sqlStream)
        {
            string sql = (string)StreamUtils.ReadValue(sqlStream);
            int parameterCount = (int)StreamUtils.ReadValue(sqlStream);
            ParameterTable parameterList = new ParameterTable();
            string parameterName;
            object parameterValue;
            for (int index = 0; index < parameterCount; index++)
            {
                parameterName = (string)StreamUtils.ReadValue(sqlStream);
                parameterValue = StreamUtils.ReadValue(sqlStream);
                parameterList.Add(parameterName, parameterValue);
            }
            return ExecuteReader(sql, parameterList);
        }
    }
}
