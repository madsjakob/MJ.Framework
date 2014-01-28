using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MJS.Framework.Base.Types;
using MJS.Framework.Communication.CO;
using MJS.Framework.Data.Interfaces;
using MJS.Framework.Data.Metadata;

namespace MJS.Framework.Data.CO
{
    public static class CODataClass
    {
        public static bool Load(IDataClass dataClass)
        {
            ParameterTable parameterList = new ParameterTable();
            string sql = DatabaseTableAttribute.BuildSelectQuery(dataClass, parameterList);
            DataTable table = CODataAccess.Main.Endpoint.ExecuteReader(sql, parameterList); 
            bool result = (table.Rows.Count == 1);
            if (result)
            {
                DatabaseTableAttribute.ReadData(table.Rows[0], dataClass);
            }
            return result;
        }

        public static void LoadList(IDataClassList dataClassList, string where = null, ParameterTable parameterTable = null, string order = null)
        {
            string sql = DatabaseTableAttribute.BuildSelectListQuery(dataClassList.GetInstanceType(), where, order, true);
            DataTable table;
            if (parameterTable == null)
            {
                table = CODataAccess.Main.Endpoint.ExecuteReader(sql);
            }
            else
            {
                table = CODataAccess.Main.Endpoint.ExecuteReader(sql, parameterTable);
            }
            foreach (DataRow row in table.Rows)
            {
                IDataClass dataClass = dataClassList.GetInstance();
                DatabaseTableAttribute.ReadData(row, dataClass);
                dataClassList.Add(dataClass);
            }
        }

        public static bool Exists(IDataClass dataClass)
        {
            Guid id = DatabaseKeyAttribute.GetKeyValue(dataClass);
            bool result = (id != Guid.Empty);
            if (result)
            {
                ParameterTable parameterList = new ParameterTable();
                string sql = DatabaseTableAttribute.BuildSelectQuery(dataClass, parameterList);
                // Select count....
                DataTable table = CODataAccess.Main.Endpoint.ExecuteReader(sql, parameterList); //
                result = (table.Rows.Count == 1);
            }
            return result;
        }

        public static void Save(IDataClass dataClass)
        {
            ParameterTable parameterList = new ParameterTable();
            string sql;
            if (Exists(dataClass))
            {
                sql = DatabaseTableAttribute.BuildUpdateQuery(dataClass, parameterList);
            }
            else
            {
                sql = DatabaseTableAttribute.BuildInsertQuery(dataClass, parameterList);
            }
            CODataAccess.Main.Endpoint.ExecuteNonQuery(sql, parameterList);
        }


        public static void Delete(IDataClass dataClass)
        {
            ParameterTable parameterList = new ParameterTable();
            string sql = DatabaseTableAttribute.BuildDeleteQuery(dataClass, parameterList);
            CODataAccess.Main.Endpoint.ExecuteNonQuery(sql, parameterList);
        }
    }
}
