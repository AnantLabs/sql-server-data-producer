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
using SQLDataProducer.ContinuousInsertion.Builders.Helpers;

namespace SQLDataProducer.ContinuousInsertion.Builders.EntityBuilders
{
    static class TableQueryBuilder
    {
        public static void AppendVariablesForTable(TableEntity table, StringBuilder sb, int rep, long N)
        {
            table.GenerateValuesForColumns(N);
            
            foreach (var col in table.Columns)//.Where(x => x.IsNotIdentity)
            {
                if (col.Generator.IsSqlQueryGenerator)
                    continue;

                string paramName = QueryBuilderHelper.GetParamName(rep, col);
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

        public static void CreateValuesPartForTable(TableEntity table, StringBuilder sb, int rep)
        {
            for (int i = 0; i < table.Columns.Count; i++)
            {
                var col = table.Columns[i];
                // If he column is identity and the generator is set to use SQL Server identity generation, then dont generate this value part
                if (col.IsIdentity && col.Generator.GeneratorName == SQLDataProducer.Entities.Generators.Generator.GENERATOR_IdentityFromSqlServerGenerator)
                    continue;

                //// Get the parameter name to use, if the generator is sql query generator then the generated variable should be used instead of the parameter.
                if (!col.Generator.IsSqlQueryGenerator)
                    sb.Append(QueryBuilderHelper.GetParamName(rep, col));
                else
                    sb.Append(QueryBuilderHelper.GetSqlQueryParameterName(col));

                sb.Append(i == table.Columns.Count - 1 ? string.Empty : ", ");
            }
        }

        /// <summary>
        /// Generate the custom sql script part of the final statement. <example>declare @c_DateTimeColumn_1 datetime = getdate();</example>
        /// </summary>
        /// <param name="sb">the stringbuilder to append the sql script part to</param>
        public static void AppendSqlScriptPartOfStatement(TableEntity table, StringBuilder sb)
        {
            sb.AppendLine();
            foreach (var c in table.Columns.Where(c => c.Generator.IsSqlQueryGenerator))
            {
                var variableName = QueryBuilderHelper.GetSqlQueryParameterName(c);
                c.GenerateValue(1);
                var query = c.PreviouslyGeneratedValue;
                sb.AppendLine(string.Format("DECLARE {0} {1} = ({2});", variableName, c.ColumnDataType.Raw, query));
            }
            sb.AppendLine();
        }

        public static void AppendInsertPartForTable(ExecutionItem ei, StringBuilder sb)
        {
            if (ei.TargetTable.Columns.Any(col => col.IsIdentity && col.Generator.GeneratorName != SQLDataProducer.Entities.Generators.Generator.GENERATOR_IdentityFromSqlServerGenerator))
                sb.AppendLine(string.Format("SET IDENTITY_INSERT {0} ON;", ei.TargetTable.ToString()));

            sb.AppendFormat("INSERT {0}.{1} (", ei.TargetTable.TableSchema, ei.TargetTable.TableName);
            sb.AppendLine();

            for (int i = 0; i < ei.TargetTable.Columns.Count; i++)
            {
                var col = ei.TargetTable.Columns[i];
                
                if (col.IsIdentity && col.Generator.GeneratorName == SQLDataProducer.Entities.Generators.Generator.GENERATOR_IdentityFromSqlServerGenerator)
                    continue;

                sb.AppendFormat("\t[{0}]", col.ColumnName);
                sb.Append(i == ei.TargetTable.Columns.Count - 1 ? string.Empty : ", ");
                sb.AppendLine();

            }

            sb.Append(")");
            if (ei.TargetTable.Columns.Any(col => col.IsIdentity && col.Generator.GeneratorName != SQLDataProducer.Entities.Generators.Generator.GENERATOR_IdentityFromSqlServerGenerator))
                sb.AppendLine(string.Format("SET IDENTITY_INSERT {0} OFF;", ei.TargetTable.ToString()));

        }
    }
}
