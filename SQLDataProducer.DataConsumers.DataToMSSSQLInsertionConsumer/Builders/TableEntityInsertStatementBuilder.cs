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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.Common;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.DataAccess.Factories;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DataEntities.Collections;
using SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer.Builders.EntityBuilders;
using SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer.Builders.Helpers;

namespace SQLDataProducer.DataConsumers.DataToMSSSQLScriptConsumer
{

    /// <summary>
    /// Provides functionality to generate queries based on an ExecutionItem
    /// </summary>
    public class TableEntityInsertStatementBuilder 
    {
        Dictionary<string, DbParameter> _paramCollection;
        public Dictionary<string, DbParameter> Parameters
        {
            get
            {
                return _paramCollection;
            }
        }


        //ExecutionItem _execItem;
        //public ExecutionItem ExecuteItem
        //{
        //    get
        //    {
        //        return _execItem;
        //    }
        //}

        public TableEntityInsertStatementBuilder(DataRowSet rows)
        {
            //_execItem = ie;
            _paramCollection = new Dictionary<string, DbParameter>();

            Init(rows);
        }

        string _insertStatement;
        public string InsertStatement
        {
            get
            {
                return _insertStatement;
            }
        }
        /// <summary>
        /// Initializes the insertstatement and DbParameters required.
        /// </summary>
        private void Init(DataRowSet ds)
        {
            FillParameterCollection(ds);

            StringBuilder sb = new StringBuilder();

            if(ds.IsIdentityInsert)
                sb.AppendLine(string.Format("SET IDENTITY_INSERT {0} ON;", ds.DatasetName));

            TableQueryBuilder.AppendSqlScriptPartOfStatement(ds, sb);
            ExecutionItemQueryBuilder.AppendInsertPartOfStatement(ds, sb);
            ExecutionItemQueryBuilder.AppendValuePartOfInsertStatement(ds, sb);


            if (ds.IsIdentityInsert)
                sb.AppendLine(string.Format("SET IDENTITY_INSERT {0} OFF;", ds.DatasetName));

                     _insertStatement = sb.ToString();
         
        }

        private void FillParameterCollection(DataRowSet ds)
        {
            for (int rep = 1; rep <= ds.Count; rep++)
            {
                for (int i = 0; i < ds.TargetTable.Columns.Count; i++)
                {
                    var col = ds.TargetTable.Columns[i];
                    
                    string paramName = QueryBuilderHelper.GetParamName(rep, col);
                    // Add the parameter with no value, values will be added in the GenerateValues method
                    var par = CommandFactory.CreateParameter(paramName, null, col.ColumnDataType.DBType);
                    Parameters.Add(paramName, par);
                }
            }
        }

        public static List<TableEntityInsertStatementBuilder> CreateBuilders(DataRowSet rows)
        {
            var builders = new List<TableEntityInsertStatementBuilder>();
            builders.Add(new TableEntityInsertStatementBuilder(rows));

            return builders;
        }

        internal void SetParameterValues(DataRowSet dataRows)
        {
            foreach (var row in dataRows)
            {
                foreach (var c in row.Cells)
                {
                    string paramName = QueryBuilderHelper.GetParamName(row.RowNumber, c.Column);
                    Parameters[paramName].Value = c.Value;
                }
            }
        }
    }
}
