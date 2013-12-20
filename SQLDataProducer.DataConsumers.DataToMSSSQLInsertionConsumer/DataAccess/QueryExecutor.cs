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


using System.Data.Common;
using System.Collections.Generic;
using System;

namespace SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer.DataAccess
{
    public class QueryExecutor : IDisposable
    {
        private DbConnection _connection;
        DbCommand _cmd;
        
        public QueryExecutor(string connectionString)
        {
            _connection = CommandFactory.CreateDbConnection(connectionString);
            _connection.Open();
            _cmd = _connection.CreateCommand();
        }

        public void ExecuteNonQuery(string query)
        {
            PrepareCommand(query, null);
            _cmd.ExecuteNonQuery();
        }

        public void ExecuteNonQuery(string query, Dictionary<string, DbParameter> parameters)
        {
            PrepareCommand(query, parameters);
            _cmd.ExecuteNonQuery();
        }

        private void PrepareCommand(string query, Dictionary<string, DbParameter> parameters)
        {
            _cmd.Parameters.Clear();
            _cmd.CommandText = query;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    _cmd.Parameters.Add(p.Value);
                }
            }
        }

        public long ExecuteIdentity(string query, Dictionary<string, DbParameter> parameters)
        {
            PrepareCommand(query, parameters);
            var outputParam = CommandFactory.CreateParameter("@Identity_output", 0, System.Data.SqlDbType.BigInt);
            outputParam.Direction = System.Data.ParameterDirection.Output;
            _cmd.Parameters.Add(outputParam);
            
            _cmd.ExecuteNonQuery();

            return long.Parse(outputParam.Value.ToString());
        }

        public void Dispose()
        {
            if (_cmd != null)
                _cmd.Dispose();
            if(_connection != null)
                _connection.Dispose();
        }
    }
}
