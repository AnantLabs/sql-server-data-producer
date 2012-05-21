using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLRepeater.Entities;
using System.Data.SqlClient;
using SQLRepeater.DatabaseEntities.Entities;
using SQLRepeater.Entities.ExecutionOrderEntities;

namespace SQLRepeater.EntityQueryGenerator
{
    public class InsertQueryGenerator
    {
        //public string GenerateQueryFor(TableEntity table)
        //{
        //    //string sql = string.Empty;
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendFormat("INSERT {0}.{1} (", table.TableSchema, table.TableName);
        //    foreach (var col in table.Columns.Where( x => x.IsIdentity == false))
        //    {
        //        if (col.OrdinalPosition == table.Columns.Count)
        //            sb.AppendFormat("{0}", col.ColumnName);
        //        else
        //            sb.AppendFormat("{0},", col.ColumnName);
        //    }

        //    sb.Append(")");
        //    sb.AppendLine();
        //    sb.Append("VALUES(");
        //    foreach (var col in table.Columns.Where(x => x.IsIdentity == false))
        //    {
        //        if (col.OrdinalPosition == table.Columns.Count)
        //            sb.AppendFormat("@{0}", col.ColumnName);
        //        else
        //            sb.AppendFormat("@{0},", col.ColumnName);
        //    }
        //    sb.Append(")");
            
        //    return sb.ToString(); ;
        //}

      



        public string GenerateQueryForExecutionItems(IList<ExecutionItem> executionItems)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SET NOCOUNT ON");
            sb.AppendLine("SET XACT_ABORT ON");
            sb.AppendLine();
            sb.AppendLine("BEGIN TRANSACTION");
            sb.AppendLine();
            sb.AppendLine("-- Declarations");
            sb.AppendLine();

            sb.Append("{0}"); // all of these will be generated on the fly for each iteration later using GenerateVariableDeclarationAndValuesForExecutionItems
            sb.AppendLine();

            

            foreach (var item in executionItems)
            {
                // Skip tables with no columns
                if (item.TargetTable.Columns.Count == 0)
                {
                    continue;
                }

                bool hasIdentity = item.TargetTable.Columns.Any(col => col.IsIdentity);

                sb.AppendFormat("-- Insert item {0}", item.Order);
                sb.AppendLine();
                if (hasIdentity)
                {
                    sb.AppendFormat("declare @i{0}_identity bigint", item.Order);
                    sb.AppendLine();
                }
                
                // Generate for each column
                sb.Append(GenerateInsertStatement(item));

                if (hasIdentity)
                {
                    sb.AppendLine();
                    sb.AppendFormat("select @i{0}_identity = scope_identity()", item.Order);
                }
                
                sb.AppendLine();
                sb.AppendFormat("-- done insert item {0}", item.Order);
                sb.AppendLine();
                
                
            }
            sb.AppendLine("COMMIT");
            sb.AppendLine();

            return sb.ToString();
        }

        private string GenerateInsertStatement(ExecutionItem item)
        {
            //insert PlayerAccount.Operator
            //        ( OperatorIdentifier ,
            //          HasPrivatePlayerGroups
            //        )
            //VALUES  ( 
            //            @i1_OperatorIdentifier , -- OperatorIdentifier - varchar(50)
            //            @i1_HasPrivatePlayerGroups  -- HasPrivatePlayerGroups - bit
            //        )

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("INSERT {0}.{1} (", item.TargetTable.TableSchema, item.TargetTable.TableName);
            sb.AppendLine();
            foreach (var col in item.TargetTable.Columns.Where(x => x.IsIdentity == false))
            {
                sb.AppendFormat("\t{0}", col.ColumnName);
                sb.Append(col.OrdinalPosition == item.TargetTable.Columns.Count ? "" : ", ");
                sb.AppendLine();
            }

            sb.Append(")");
            sb.AppendLine();
            sb.Append("VALUES(");
            sb.AppendLine();
            foreach (var col in item.TargetTable.Columns.Where(x => x.IsIdentity == false))
            {
                sb.AppendFormat("\t@i{0}_{1}", item.Order, col.ColumnName);
                sb.Append(col.OrdinalPosition == item.TargetTable.Columns.Count ? "" : ", ");
                sb.AppendLine();
            }
            sb.Append(")");

            return sb.ToString();
        }


        /// <summary>
        /// Generate the declaration section of the sqlquery, including the values for the variables
        /// </summary>
        /// <param name="n">The serial number to use when creating the values for the variables</param>
        /// <param name="execItems">the executionItems to be included in the variable declarations</param>
        /// <returns></returns>
        private string GenerateVariableDeclarationAndValuesForExecutionItems(int n, IEnumerable<ExecutionItem> execItems)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("DECLARE @n BIGINT = {0}", n);
            sb.AppendLine();

            foreach (var tabl in execItems)
            {
                // Skip tables with no columns
                if (tabl.TargetTable.Columns.Count == 0)
                {
                    continue;
                }

                sb.AppendFormat("-- Item {0}, {1}.{2}", tabl.Order, tabl.TargetTable.TableSchema, tabl.TargetTable.TableName);
                sb.AppendLine();
                foreach (ColumnEntity col in tabl.TargetTable.Columns)
                {
                    // create declaration of the columns datatype, set the value to the generated value generated from the columns generator and its generatorParameter
                    sb.AppendFormat("DECLARE @i{0}_{1} {2} = {3};", tabl.Order, col.ColumnName, col.ColumnDataType, col.Generator.GenerateValue(n));
                    sb.AppendLine();
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public string GenerateFinalQuery(IEnumerable<ExecutionItem> execItems, string baseQuery, int n)
        {
            string declarations = GenerateVariableDeclarationAndValuesForExecutionItems(n, execItems);

            // This will contain the insertstatements and the declare variables with values, ready to be executed
            string finalResult = string.Format(baseQuery, declarations);
            return finalResult;
        }

    }
}
