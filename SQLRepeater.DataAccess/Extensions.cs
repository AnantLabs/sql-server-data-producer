using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace SQLRepeater.DataAccess
{
    public static class Extensions
    {
        public static string GetStringOrEmpty(this SqlDataReader reader, string colName)
        {
            if (reader.IsDBNull(reader.GetOrdinal(colName)))
                return string.Empty;
            else
                return reader.GetString(reader.GetOrdinal(colName));
        }

    }
}
