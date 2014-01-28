using MJS.Framework.Base.Types;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MJS.Framework.Base.CO
{
    public class CODatabase
    {
        public CODatabase(string connectionString)
        {
            _connectionString = connectionString;
        }

        private readonly string _connectionString;

        public int ExecuteNonQuery(string sql, params object[] parameterList)
        {
            return ExecuteNonQuery(sql, ParameterListToTable(parameterList));
        }

        public int ExecuteNonQuery(string sql, ParameterTable parameterTable)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    AddParameters(command, parameterTable);
                    return command.ExecuteNonQuery();
                }
            }
        }

        public DataTable ExecuteReader(string sql, params object[] parameterList)
        {
            return ExecuteReader(sql, ParameterListToTable(parameterList));
        }

        public DataTable ExecuteReader(string sql, ParameterTable parameterTable)
        {
            
            DataTable table = new DataTable();
                
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    AddParameters(command, parameterTable);
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        table.Load(reader);
                    }
                }
            }
            return table;
        }

        public DataTable ExecuteReaderSP(string sql, ParameterTable parameterTable)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = sql;
                    AddParameters(command, parameterTable);
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);
                        return table;
                    }
                }
            }       
        }

        private static ParameterTable ParameterListToTable(object[] parameterList)
        {
            ParameterTable parameterTable = new ParameterTable();
            for (int index = 0; index < parameterList.Length; index++)
            {
                parameterTable.Add(index.ToString(), parameterList[index]);
            }
            return parameterTable;
        }

        private void AddParameters(IDbCommand command, ParameterTable parameterTable)
        {
            foreach (string parameterName in parameterTable.Keys)
            {
                IDbDataParameter parameter = command.CreateParameter();
                parameter.ParameterName = "@" + parameterName;
                parameter.Value = parameterTable[parameterName];
                Console.WriteLine(parameter.ParameterName + " = " + parameter.Value);
                command.Parameters.Add(parameter);
            }
        }
    }
}
