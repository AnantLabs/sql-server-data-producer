// Copyright 2012 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.


using System.Data.Common;
using System.Collections.Generic;
namespace SQLDataProducer.ContinuousInsertion.DataAccess
{
    public class QueryExecutor 
    {
        private DbConnection _connection;
        
        public QueryExecutor(string connectionString)
        {
            _connection = CommandFactory.CreateDbConnection(connectionString);
            _connection.Open();
        }

        public void ExecuteNonQuery(string query, Dictionary<string, DbParameter> parameters)
        {
            var cmd = CommandFactory.CreateCommand(query, _connection, System.Data.CommandType.Text);
            foreach (var p in parameters)
            {
                cmd.Parameters.Add(p.Value);
            }
            cmd.ExecuteNonQuery();
        }
    }
}
