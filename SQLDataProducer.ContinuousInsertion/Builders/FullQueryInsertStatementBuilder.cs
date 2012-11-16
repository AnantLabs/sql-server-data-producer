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
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.Entities.DatabaseEntities;

namespace SQLDataProducer.ContinuousInsertion.Builders
{
    public class FullQueryInsertStatementBuilder
    {
        ExecutionItemCollection _items;

        public FullQueryInsertStatementBuilder(ExecutionItemCollection items)
        {
            _items = items;
        }

        /// <summary>
        /// Generate the full SQL script that can be executed outside the application and result in the same rows as it would have inside the application.
        /// </summary>
        /// <returns>The full sql query that will insert all the rows and values</returns>
        public string GenerateFullStatement(Func<long> getN)
        {
            StringBuilder sb = new StringBuilder();

            // set the values
            foreach (var ei in _items)
            {
                GenerateVariables(ei, sb,getN);
                GenerateSqlScriptPartOfStatement(ei, sb);
                GenerateInsertPartOfStatement(ei, sb);
                GenerateValuePartOfInsertStatement(ei, sb);
            }
            
            return sb.ToString();
        }

        

        /// <summary>
        /// Generate the custom sql script part of the final statement. <example>declare @c_DateTimeColumn_1 datetime = getdate();</example>
        /// </summary>
        /// <param name="sb">the stringbuilder to append the sql script part to</param>
        private void GenerateSqlScriptPartOfStatement(ExecutionItem ei, StringBuilder sb)
        {
            sb.AppendLine();
            foreach (var c in ei.TargetTable.Columns.Where(c => c.Generator.IsSqlQueryGenerator))
            {
                var variableName = GetSqlQueryParameterName(c);
                c.GenerateValue(1);
                var query = c.PreviouslyGeneratedValue;
                sb.AppendLine(string.Format("DECLARE {0} {1} = ({2});", variableName, c.ColumnDataType.Raw, query));
            }
            sb.AppendLine();
        }

        

        /// <summary>
        /// Generate the INSERT-part of the statement, example <example>INSERT schemaName.TableName(c1, c2)</example>
        /// </summary>
        /// <param name="sb">the stringbuilder to append the insert statement to</param>
        private void GenerateInsertPartOfStatement(ExecutionItem ei, StringBuilder sb)
        {
            sb.AppendLine();

            sb.AppendFormat("INSERT {0}.{1} (", ei.TargetTable.TableSchema, ei.TargetTable.TableName);
            sb.AppendLine();

            for (int i = 0; i < ei.TargetTable.Columns.Count; i++)
            {
                var col = ei.TargetTable.Columns[i];

                if (col.IsIdentity)
                    continue;

                sb.AppendFormat("\t[{0}]", col.ColumnName);
                sb.Append(i == ei.TargetTable.Columns.Count - 1 ? string.Empty : ", ");
                sb.AppendLine();

            }

            sb.Append(")");
            sb.AppendLine();
        }

        private void GenerateValuePartOfInsertStatement(ExecutionItem ei, StringBuilder sb)
        {
            sb.AppendLine();
            sb.AppendLine("VALUES");
            for (int rep = 1; rep <= ei.RepeatCount; rep++)
            {
                sb.Append("\t");
                sb.Append("(");

                for (int i = 0; i < ei.TargetTable.Columns.Count; i++)
                {
                    var col = ei.TargetTable.Columns[i];

                    if (col.IsIdentity)
                        continue;

                    string paramName = GetParamName(rep, col);
                    
                    //// Get the parameter name to use, if the generator is sql query generator then the generated variable should be used instead of the parameter.
                    if (!col.Generator.IsSqlQueryGenerator)
                        sb.Append(paramName);
                    else
                        sb.Append(GetSqlQueryParameterName(col));

                    sb.Append(i == ei.TargetTable.Columns.Count - 1 ? string.Empty : ", ");
                }

                sb.Append(")");
                sb.Append(ei.RepeatCount == rep ? ";" : ", ");
                sb.AppendLine();
            }
            // If the table have idenitity column then we shuold store that in a variable

            if (ei.TargetTable.HasIdentityColumn)
            {
                var identityVariableName = string.Format("@Identity_i{0}", ei.Order);
                sb.AppendFormat("DECLARE {0} {1} = SCOPE_IDENTITY();", identityVariableName, "int");
                //sb.AppendFormat("SELECT {0} = SCOPE_IDENTITY();", identityVariableName);
                sb.AppendLine();
            }
            sb.AppendLine();
        }

        /// <summary>
        /// Set values for the parameters. The parameters must already be generated and should have been in the init() function
        /// </summary>
        /// <param name="getN">will be called once per row</param>
        private void GenerateVariables(ExecutionItem ei, StringBuilder sb, Func<long> getN)
        {
            sb.AppendLine();
            for (int rep = 1; rep <= ei.RepeatCount; rep++)
            {
                long N = getN();

                ei.TargetTable.GenerateValuesForColumns(N);
                foreach (var col in ei.TargetTable.Columns.Where(x => x.IsNotIdentity))
                {
                    if (col.Generator.IsSqlQueryGenerator)
                        continue;

                    string paramName = GetParamName(rep, col);
                    if (col.Generator.GeneratorName != SQLDataProducer.Entities.Generators.Generator.GENERATOR_IdentityFromPreviousItem)
                    {
                        var value = col.PreviouslyGeneratedValue == DBNull.Value ? "NULL" : string.Format(col.ColumnDataType.StringFormatter, col.PreviouslyGeneratedValue);
                        sb.AppendLine(string.Format("DECLARE {0} {1} = {2}; -- {3}",
                            paramName,
                            col.ColumnDataType.Raw,
                            value,
                            col.Generator.GeneratorName));
                    }
                    else
                    {
                        sb.AppendLine(string.Format("DECLARE {0} {1} = {2}; -- {3}",
                            paramName,
                            col.ColumnDataType.Raw,
                            string.Format("@Identity_i{0}", col.Generator.GeneratorParameters[0].Value),
                            col.Generator.GeneratorName));
                    }
                }
            }
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

        private static string GetParamName(int rep, ColumnEntity col)
        {
            var ei = col.ParentTable.ParentExecutionItem;
            return string.Format("@{0}_{1}_{2}", col.ColumnName.Replace(" ", "").Replace(".", ""), ei.Order ,rep);
        }
    }
}
