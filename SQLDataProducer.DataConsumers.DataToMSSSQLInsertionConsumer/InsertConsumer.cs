﻿// Copyright 2012-2013 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.


using SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer.DataAccess;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DataConsumers;
using SQLDataProducer.Entities.DataEntities;
using SQLDataProducer.Entities.ExecutionEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Collections;
using System.Data;

namespace SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer
{
    [ConsumerMetaData("Insert rows to DB", null)]
    public class InsertConsumer : IDataConsumer
    {
        private Dictionary<string, string> _options;
        private QueryExecutor _queryExecutor;
        private int _rowCounter = 0;

        public string ConnectionString
        {
            get { return _options["ConnectionString"]; }
            set { _options["ConnectionString"] = value; }
        }

        public bool Init(string connectionString, Dictionary<string, string> options)
        {
            _options = options;
            ConnectionString = connectionString;
            _queryExecutor = new QueryExecutor(ConnectionString);
            _rowCounter = 0;
            return true;
        }

        public void Dispose()
        {
            if (_queryExecutor != null)
            {
                _queryExecutor.Dispose();
            }
        }

        public ExecutionResult Consume(IEnumerable<DataRowEntity> rows, ValueStore valueStore)
        {
            foreach (var insertQuery in TableQueryGenerator.GenerateInsertStatements(rows, valueStore))
            {
                Dictionary<Guid, object> generatedValues = InsertWithResult(insertQuery);
                
                if (null != generatedValues)
                    PutGeneratedValuesInValueStore(generatedValues, valueStore);
                
                _rowCounter++;
            }

            return null;
        }
        /// <summary>
        /// TODO: Refactor. Remove hardcoding of OUTPUT. to know if the inserted row should create any value (identity etc)
        /// TODO: Refactor method of reading values from table
        /// </summary>
        /// <param name="insertQuery"></param>
        /// <returns></returns>
        private Dictionary<Guid, object> InsertWithResult(string insertQuery)
        {
            Dictionary<Guid, object> generatedValues = null;
            if (insertQuery.Contains("OUTPUT INSERTED."))
            {
                DataTable table = _queryExecutor.ExecuteTableResult(insertQuery);
                generatedValues = new Dictionary<Guid, object>();
                if (table != null)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            Guid key;
                            if (Guid.TryParse(column.ColumnName, out key))
                            {
                                generatedValues[key] = row[column];
                            }
                            else
                            {
                                throw new InvalidCastException("The column name in the datatable is expected to be GUID as a key in the value store, name was not a GUID.");
                            }
                        }
                    }
                }
            }
            else
            {
                _queryExecutor.ExecuteNonQuery(insertQuery);
            }
            return generatedValues;
        }

        private static void PutGeneratedValuesInValueStore(Dictionary<Guid, object> generatedValues, ValueStore valueStore)
        {
            foreach (var key in generatedValues.Keys)
            {
                valueStore.Put(key, generatedValues[key]);
            }
        }

        public int TotalRows
        {
            get { return _rowCounter; }
        }
    }
}