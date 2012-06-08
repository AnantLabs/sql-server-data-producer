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
        public string GenerateQueryForExecutionItems(IEnumerable<ExecutionItem> executionItems)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SET NOCOUNT ON");
            sb.AppendLine("SET XACT_ABORT ON");
            sb.AppendLine();
            sb.AppendLine("BEGIN TRANSACTION");
            sb.AppendLine();
            //sb.AppendLine("-- Declarations");
            //sb.AppendLine();

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
                
                // Generate for each column
                sb.Append(GenerateInsertStatement(item));

                if (hasIdentity)
                {
                    sb.AppendLine();
                    sb.AppendFormat("declare @i{0}_identity bigint = scope_Identity()", item.Order);
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

            // This placeholder will be replaced during data generation
            // The variables will be generated and inserted instead of this placeholder.
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendFormat("<DECLARE ITEM{0}>", item.Order);
            
            sb.AppendFormat("INSERT {0}.{1} (", item.TargetTable.TableSchema, item.TargetTable.TableName);
            sb.AppendLine();
            foreach (var col in item.TargetTable.Columns.Where(x => x.IsIdentity == false))
            {
                sb.AppendFormat("\t{0}", col.ColumnName);
                sb.Append(col.OrdinalPosition == item.TargetTable.Columns.Count ? string.Empty : ", ");
                sb.AppendLine();
            }

            sb.Append(")");
            sb.AppendLine();
            sb.Append("SELECT");
            sb.AppendLine();
            foreach (var col in item.TargetTable.Columns.Where(x => x.IsIdentity == false))
            {
                string variable = CreateVariableName(item, col);
                sb.Append("\t");
                sb.Append(variable);
                sb.Append(col.OrdinalPosition == item.TargetTable.Columns.Count ? string.Empty : ", ");
                sb.AppendLine();
            }
            
            if (item.RepeatCount > 1)
            {
                // To have set based insertions we will append this line. The parameters will all have the same value, how to fix that?
                // the execution item would need to have an option thats says if if should be repeated.
                // Option to somehow generate new values for each repetition? How to handle that?
                sb.AppendFormat("FROM (SELECT TOP {0} ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) FROM sys.objects o1, sys.objects o2, sys.objects o3) r(rn)", item.RepeatCount);
            }

            return sb.ToString();
        }

        private string CreateVariableName(ExecutionItem item, ColumnEntity col)
        {
            return string.Format("@i{0}_{1}", item.Order, col.ColumnName);
        }

        /// <summary>
        /// Generate the declaration section of the sqlquery, including the values for the variables
        /// </summary>
        /// <param name="n">The serial number to use when creating the values for the variables</param>
        /// <param name="execItems">the executionItems to be included in the variable declarations</param>
        /// <returns></returns>
        public string GenerateFinalQuery(string baseQuery, int n, IEnumerable<ExecutionItem> execItems)
        {
            string modified = baseQuery.Clone() as string;

            foreach (var item in execItems)
            {
                StringBuilder sb = new StringBuilder();
                // Skip tables with no columns
                if (item.TargetTable.Columns.Count == 0)
                {
                    continue;
                }

                sb.AppendLine();
                sb.AppendFormat("-- Item {0}, {1}.{2}", item.Order, item.TargetTable.TableSchema, item.TargetTable.TableName);
                sb.AppendLine();
                foreach (ColumnEntity col in item.TargetTable.Columns.Where(x => x.IsIdentity == false))
                {
                    // create declaration of the columns datatype, set the value to the generated value generated from the columns generator and its generatorParameter
                    sb.AppendFormat("DECLARE @i{0}_{1} {2} = {3};", item.Order, col.ColumnName, col.ColumnDataType, col.Generator.GenerateValue(n));
                    sb.AppendLine();
                }

                // Find the place in the big string where this set of declarations belong and replace the placeholder with the actual values.
                string itemNumber = string.Format("<DECLARE ITEM{0}>", item.Order);
                modified = modified.Replace(itemNumber, sb.ToString());
            }

            return modified;
        }


        //public IEnumerable<SqlParameter> GenerateParameters(int n, IEnumerable<ExecutionItem> execItems)
        //{
        //    List<SqlParameter> parms = new List<SqlParameter>();

        //    foreach (var execItem in execItems)
        //    {
        //        // Skip tables with no columns
        //        if (execItem.TargetTable.Columns.Count == 0)
        //        {
        //            continue;
        //        }

        //        foreach (ColumnEntity col in execItem.TargetTable.Columns.Where(x => x.IsIdentity == false))
        //        {
        //            string paramName = CreateParameterName(execItem, col);
        //            object paramValue = col.Generator.GenerateValue(n);

        //            parms.Add(new SqlParameter(paramName, paramValue));
        //        }
        //    }

        //    return parms;
        //}

    }
}
