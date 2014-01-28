using MJS.Framework.Base.Types;
using MJS.Framework.Communication.CO;
using MJS.Framework.Data.CO;
using MJS.Framework.Data.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MJS.Framework.Win.CO
{
    public static class CODataList
    {
        public static void FillGrid(DataGridView grid, Type dataClassType, string where = null, ParameterTable parameterTable = null, string order = null)
        {
            DatabaseFieldAttribute[] fieldAttributes = DatabaseFieldAttribute.GetDatabaseFields(dataClassType);
            string sql = DatabaseTableAttribute.BuildSelectListQuery(dataClassType, where, order, false);
            DataTable table = null;
            if (parameterTable == null)
            {
                table = CODataAccess.Main.Endpoint.ExecuteReader(sql);
            }
            else
            {
                table = CODataAccess.Main.Endpoint.ExecuteReader(sql, parameterTable);
            }
            grid.DataSource = table;
            foreach (DatabaseFieldAttribute field in fieldAttributes)
            {
                if (field is DatabaseKeyAttribute)
                {
                    grid.Columns[field.FieldName].Visible = false;
                }
            }
        }
    }
}
