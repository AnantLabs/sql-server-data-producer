using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace SQLDataProducer.DataAccess
{
    public static class Extensions
    {
        /// <summary>
        /// Get string of the field or an empty string. Use when the column have a NULLABLE string datatype
        /// </summary>
        /// <param name="reader">the sql datareader that this extension is attached to</param>
        /// <param name="colName">the string name of the column</param>
        /// <returns>string.Empty if the column is null, otherwise the value of the column</returns>
        public static string GetStringOrEmpty(this SqlDataReader reader, string colName)
        {
            if (reader.IsDBNull(reader.GetOrdinal(colName)))
                return string.Empty;
            else
                return reader.GetString(reader.GetOrdinal(colName));
        }

    }
}
