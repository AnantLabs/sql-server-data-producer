﻿// Copyright 2012 Peter Henell

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
        DbCommand _cmd;
        
        public QueryExecutor(string connectionString)
        {
            _connection = CommandFactory.CreateDbConnection(connectionString);
            _connection.Open();
            _cmd = _connection.CreateCommand();
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
            foreach (var p in parameters)
            {
                _cmd.Parameters.Add(p.Value);
            }
        }
        public long ExecuteIdentity(string query, Dictionary<string, DbParameter> parameters)
        {
            PrepareCommand(query, parameters);

            using (var reader = _cmd.ExecuteReader())
            {
                if (reader.Read())
                    return long.Parse(reader.GetValue(0).ToString());

                // TODO: How to handle failed idenitity reads? Would never happen?
                return 0;
            }
        }

        //public void ExecuteNonQuery(string query)
        //{
            
        //}

        //public DbCommand PrepareCommand(string query, Dictionary<string, DbParameter> parameters)
        //{
        //    var cmd = CommandFactory.CreateCommand(query, _connection, System.Data.CommandType.Text);

        //    foreach (var p in parameters)
        //    {
        //        cmd.Parameters.Add(p.Value);
        //    }
        //    cmd.ExecuteNonQuery();
        //    return cmd;
        //}

        //public void ExecuteCommand(DbCommand cmd)
        //{
        //    cmd.Connection = _connection;
        //    cmd.ExecuteNonQuery();
        //    cmd.Connection = null;
        //}
    }
}
