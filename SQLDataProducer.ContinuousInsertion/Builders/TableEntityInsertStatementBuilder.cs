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
using SQLDataProducer.Entities.DatabaseEntities;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using SQLDataProducer.ContinuousInsertion.DataAccess;
using SQLDataProducer.Entities;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.ContinuousInsertion.Builders.EntityBuilders;
using SQLDataProducer.ContinuousInsertion.Builders.Helpers;

namespace SQLDataProducer.ContinuousInsertion.Builders
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


        ExecutionItem _execItem;
        public ExecutionItem ExecuteItem
        {
            get
            {
                return _execItem;
            }
        }

        public TableEntityInsertStatementBuilder(ExecutionItem ie)
        {
            _execItem = ie;
            _paramCollection = new Dictionary<string, DbParameter>();

            Init();
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
        private void Init()
        {
            FillParameterCollection();

            StringBuilder sb = new StringBuilder();

            if (ExecuteItem.TargetTable.Columns.Any(col => col.IsIdentity && col.Generator.GeneratorName != SQLDataProducer.Entities.Generators.Generator.GENERATOR_IdentityFromSqlServerGenerator))
                sb.AppendLine(string.Format("SET IDENTITY_INSERT {0} ON;", ExecuteItem.TargetTable.ToString()));

            TableQueryBuilder.AppendSqlScriptPartOfStatement(ExecuteItem.TargetTable, sb);
            ExecutionItemQueryBuilder.AppendInsertPartOfStatement(ExecuteItem, sb);
            ExecutionItemQueryBuilder.AppendValuePartOfInsertStatement(ExecuteItem, sb);


            if (ExecuteItem.TargetTable.Columns.Any(col => col.IsIdentity && col.Generator.GeneratorName != SQLDataProducer.Entities.Generators.Generator.GENERATOR_IdentityFromSqlServerGenerator))
                sb.AppendLine(string.Format("SET IDENTITY_INSERT {0} OFF;", ExecuteItem.TargetTable.ToString()));

            
            _insertStatement = sb.ToString();
         
        }

        private void FillParameterCollection()
        {
            for (int rep = 1; rep <= ExecuteItem.RepeatCount; rep++)
            {
                for (int i = 0; i < ExecuteItem.TargetTable.Columns.Count; i++)
                {
                    var col = ExecuteItem.TargetTable.Columns[i];
                    
                    //if (col.IsIdentity)
                    //    continue;

                    string paramName = QueryBuilderHelper.GetParamName(rep, col);
                    // Add the parameter with no value, values will be added in the GenerateValues method
                    var par = CommandFactory.CreateParameter(paramName, null, col.ColumnDataType.DBType);
                    Parameters.Add(paramName, par);
                }
            }

        }
        
        /// <summary>
        /// Set values for the parameters. The parameters must already be generated and should have been in the init() function
        /// </summary>
        /// <param name="getN">will be called once per row</param>
        public void GenerateValues(Func<long> getN)
        {
            for (int rep = 1; rep <= ExecuteItem.RepeatCount; rep++)
            {
                long N = getN();

                ExecuteItem.TargetTable.GenerateValuesForColumns(N);
                
                foreach (var col in ExecuteItem.TargetTable.Columns)//.Where(x => x.IsNotIdentity))
                {
                    string paramName = QueryBuilderHelper.GetParamName(rep, col);
                    Parameters[paramName].Value = col.PreviouslyGeneratedValue;
                }
            }
        }

        public static ObservableCollection<TableEntityInsertStatementBuilder> CreateBuilders(ExecutionItemCollection items)
        {
            var builders = new ObservableCollection<TableEntityInsertStatementBuilder>();
            foreach (var item in items)
                builders.Add(new TableEntityInsertStatementBuilder(item));

            return builders;
        }
    }
}
