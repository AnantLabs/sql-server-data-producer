// Copyright 2012-2013 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

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
