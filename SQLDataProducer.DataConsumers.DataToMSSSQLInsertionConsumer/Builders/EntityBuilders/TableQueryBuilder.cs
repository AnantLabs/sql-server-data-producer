//// Copyright 2012-2013 Peter Henell

////   Licensed under the Apache License, Version 2.0 (the "License");
////   you may not use this file except in compliance with the License.
////   You may obtain a copy of the License at

////       http://www.apache.org/licenses/LICENSE-2.0

////   Unless required by applicable law or agreed to in writing, software
////   distributed under the License is distributed on an "AS IS" BASIS,
////   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
////   See the License for the specific language governing permissions and
////   limitations under the License.

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SQLDataProducer.Entities.ExecutionEntities;
//using SQLDataProducer.Entities.DatabaseEntities;
//using SQLDataProducer.Entities.DataEntities.Collections;
//using SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer.Builders.Helpers;

//namespace SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer.Builders.EntityBuilders
//{
//    static class TableQueryBuilder
//    {
//        //public static void AppendVariablesForTable(DataRowSet ds, StringBuilder sb, int rep)
//        //{
//        //    // TODO: This line is eating a lot of performance. Replace-fix
//        //    //ds.GenerateValuesForColumns(N);
            
//        //    foreach (var col in ds.TargetTable.Columns)//.Where(x => x.IsNotIdentity)
//        //    {
//        //        if (col.Generator.IsSqlQueryGenerator)
//        //            continue;

//        //        string paramName = QueryBuilderHelper.GetParamName(rep, col);
//        //        if (col.Generator.GeneratorName != SQLDataProducer.Entities.Generators.Generator.GENERATOR_IdentityFromPreviousItem)
//        //        {
//        //            var value = col.PreviouslyGeneratedValue == DBNull.Value ? "NULL" : string.Format(col.ColumnDataType.StringFormatter, col.PreviouslyGeneratedValue);
//        //            // TODO: This line is eating a lot of performance. Replace-fix
//        //            sb.AppendLine(string.Format("DECLARE {0} {1} = {2}; -- {3}",
//        //                paramName,
//        //                col.ColumnDataType.Raw,
//        //                value,
//        //                col.Generator.GeneratorName));
//        //        }
//        //        else
//        //        {
//        //            sb.AppendLine(string.Format("DECLARE {0} {1} = {2}; -- {3}",
//        //                paramName,
//        //                col.ColumnDataType.Raw,
//        //                string.Format("@Identity_i{0}", col.Generator.GeneratorParameters[0].Value),
//        //                col.Generator.GeneratorName));
//        //        }
//        //    }
//        //}

//        public static void CreateValuesPart(DataRowSet ds, StringBuilder sb)
//        {
//            var values = 
//                    string.Join("," + Environment.NewLine, ds.Select( r =>
//                        "(" + string.Join(",", r.Cells.Select(c2 => 
//                            c2.Column.Generator.IsSqlQueryGenerator ? QueryBuilderHelper.GetSqlQueryParameterName(c2.Column) : QueryBuilderHelper.GetParamName(r.RowNumber, c2.Column))) + ")"));

//            sb.AppendLine(values);
//        }

//        /// <summary>
//        /// Generate the custom sql script part of the final statement. <example>declare @c_DateTimeColumn_1 datetime = getdate();</example>
//        /// </summary>
//        /// <param name="sb">the stringbuilder to append the sql script part to</param>
//        public static void AppendSqlScriptPartOfStatement(DataRowSet ds, StringBuilder sb)
//        {
//            //sb.AppendLine();
//            //foreach (var c in ds.TargetTable.Columns.Where(c => c.Generator.IsSqlQueryGenerator))
//            //{
//            //    var variableName = QueryBuilderHelper.GetSqlQueryParameterName(c);
//            //    c.GenerateValue(1);
//            //    var query = c.PreviouslyGeneratedValue;
//            //    sb.AppendLine(string.Format("DECLARE {0} {1} = ({2});", variableName, c.ColumnDataType.Raw, query));
//            //}
//            //sb.AppendLine();
//        }

//        public static void AppendInsertPartForTable(DataRowSet ds, StringBuilder sb)
//        {
//            sb.AppendFormat("INSERT {0}.{1} (", ds.TargetTable.TableSchema, ds.TargetTable.TableName);
//            sb.AppendLine();

//            var columnList = string.Join("," + Environment.NewLine + "\t\t", ds[0].Cells.Select(c => c.Column.ColumnName));
//            sb.Append(columnList);

//            sb.Append(")");
//        }
//    }
//}
