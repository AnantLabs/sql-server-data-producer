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

namespace SQLDataProducer.ContinuousInsertion.Builders
{

    public class TableEntityInsertStatementBuilder 
    {
        Dictionary<string, DbParameter> _paramCollection;
        public Dictionary<string, DbParameter> Parameters
        {
            get
            {
                return _paramCollection;
            }
            set
            {
                if (_paramCollection != value)
                {
                    _paramCollection = value;
                  
                }
            }
        }


        ExecutionItem _execItem;
        public ExecutionItem ExecuteItem
        {
            get
            {
                return _execItem;
            }
            set
            {
                if (_execItem != value)
                {
                    _execItem = value;
                }
            }
        }

        public TableEntityInsertStatementBuilder(ExecutionItem ie)
        {
            ExecuteItem = ie;
            Parameters = new Dictionary<string, DbParameter>();
            Init();
        }

        string _insertStatement;
        public string InsertStatement
        {
            get
            {
                return _insertStatement;
            }
            set
            {
                if (_insertStatement != value)
                {
                    _insertStatement = value;
                   
                }
            }
        }

        private void Init()
        {
            StringBuilder sb = new StringBuilder();
            GenerateSqlScriptPartOfStatement(sb);
            GenerateInsertPartOfStatement(sb);
            GenerateValuePartOfInsertStatement(sb);
            InsertStatement = sb.ToString();
        }

        private void GenerateSqlScriptPartOfStatement(StringBuilder sb)
        {
//            string example = @"declare @c_DateTimeColumn_1 datetime = getdate();
//                         declare @c_Name_1 varchar(500) = (select ''peter e så god'');
//                    ";
            sb.AppendLine();
            foreach (var c in ExecuteItem.TargetTable.Columns.Where( c => c.Generator.IsSqlQueryGenerator ))
            {
                var variableName = GetSqlQueryParameterName(c);
                c.GenerateValue(1);
                var query = c.PreviouslyGeneratedValue;
                sb.AppendLine(string.Format("DECLARE {0} {1} = ({2});", variableName, c.ColumnDataType.Raw, query));
            }
            sb.AppendLine();
        }

        /// <summary>
        /// Create the parameter name used when the generator is Sql Query generator
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static string GetSqlQueryParameterName(ColumnEntity c)
        {
            return GetParamName(1, c).Replace("@", "@c_");
        }

        private void GenerateInsertPartOfStatement(StringBuilder sb)
        {
            sb.AppendLine();

            sb.AppendFormat("INSERT {0}.{1} (", ExecuteItem.TargetTable.TableSchema, ExecuteItem.TargetTable.TableName);
            sb.AppendLine();

            for (int i = 0; i < ExecuteItem.TargetTable.Columns.Count; i++)
            {
                var col = ExecuteItem.TargetTable.Columns[i];

                if (col.IsIdentity)
                    continue;

                sb.AppendFormat("\t[{0}]", col.ColumnName);
                sb.Append(i == ExecuteItem.TargetTable.Columns.Count - 1 ? string.Empty : ", ");
                sb.AppendLine();

            }

            sb.Append(")");
            
            
            sb.AppendLine();
        }

        private void GenerateValuePartOfInsertStatement(StringBuilder sb)
        {
            sb.AppendLine();
            sb.AppendLine("VALUES");
            for (int rep = 1; rep <= ExecuteItem.RepeatCount; rep++)
            {
                sb.Append("\t");
                sb.Append("(");

                for (int i = 0; i < ExecuteItem.TargetTable.Columns.Count; i++)
                {
                    var col = ExecuteItem.TargetTable.Columns[i];

                    if (col.IsIdentity)
                        continue;



                    string paramName = GetParamName(rep, col);
                    // Add the parameter with no value, values will be added in the GenerateValues method
                    var par = CommandFactory.CreateParameter(paramName, null, col.ColumnDataType.DBType);
                    Parameters.Add(paramName, par);

                    // Get the parameter name to use, if the generator is sql query generator then the generated variable should be used instead of the parameter.
                    if (!col.Generator.IsSqlQueryGenerator)
                        sb.Append(paramName);
                    else
                        sb.Append(GetSqlQueryParameterName(col));


                    sb.Append(i == ExecuteItem.TargetTable.Columns.Count - 1 ? string.Empty : ", ");
                }

                sb.Append(")");
                sb.Append(ExecuteItem.RepeatCount == rep ? ";" : ", ");
                sb.AppendLine();
            }
            // If the table have idenitity column then we shuold select that to get it back to the application
            if (ExecuteItem.TargetTable.HasIdentityColumn)
                sb.AppendLine("SELECT SCOPE_IDENTITY();");
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
                foreach (var col in ExecuteItem.TargetTable.Columns.Where(x => x.IsNotIdentity))
                {
                    string paramName = GetParamName(rep, col);
                    Parameters[paramName].Value = col.PreviouslyGeneratedValue;
                }

            }
        }

        public string GenerateFullStatement()
        {
            StringBuilder sb = new StringBuilder();
            for (int rep = 1; rep <= ExecuteItem.RepeatCount; rep++)
            {
                for (int i = 0; i < ExecuteItem.TargetTable.Columns.Count; i++)
                {
                    var col = ExecuteItem.TargetTable.Columns[i];

                    if (col.IsIdentity)
                        continue;

                    string paramName = GetParamName(rep, col);
                    sb.AppendFormat("\t DECLARE {0} {1} = '{2}';", paramName, col.ColumnDataType.Raw, col.PreviouslyGeneratedValue);
                    sb.AppendLine();
                }
            }
            
            sb.AppendLine();
            sb.Append(InsertStatement);
            return sb.ToString();
        }

        private static string GetParamName(int rep, ColumnEntity col)
        {
            return string.Format("@{0}_{1}", col.ColumnName.Replace(" ", "").Replace(".", ""), rep);
        }

        public static ObservableCollection<TableEntityInsertStatementBuilder> CreateBuilders(ExecutionItemCollection items)
        {
            var builders = new ObservableCollection<TableEntityInsertStatementBuilder>();
            foreach (var item in items)
            {
                builders.Add(new TableEntityInsertStatementBuilder(item));
            }

            return builders;
        }
    }
}
