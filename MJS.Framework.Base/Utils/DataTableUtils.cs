using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJS.Framework.Base.Utils
{
    public static class DataTableUtils
    {
        public static void DataTableToStream(Stream stream, DataTable table)
        {
            StreamUtils.WriteValue(stream, table.Columns.Count);
            foreach (DataColumn column in table.Columns)
            {
                StreamUtils.WriteValue(stream, column.ColumnName);
                StreamUtils.WriteValue(stream, column.DataType.FullName);
            }
            StreamUtils.WriteValue(stream, table.Rows.Count);
            foreach (DataRow row in table.Rows)
            {
                foreach(DataColumn column in table.Columns)
                {
                    StreamUtils.WriteValue(stream, row[column]);
                }
            }
        }

        public static void StreamToDataTable(Stream stream, DataTable table)
        {
            int columnCount = StreamUtils.ReadIntValue(stream);
            for (int index = 0; index < columnCount; index++)
            {
                DataColumn column = table.Columns.Add((string)StreamUtils.ReadValue(stream));
                column.DataType = Type.GetType((string)StreamUtils.ReadValue(stream));
            }
            int rowCount = StreamUtils.ReadIntValue(stream);
            for (int index = 0; index < rowCount; index++)
            {
                object[] valueList = new object[columnCount];
                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    valueList[columnIndex] = StreamUtils.ReadValue(stream);
                }
                table.Rows.Add(valueList);
            }
        }
    }
}
