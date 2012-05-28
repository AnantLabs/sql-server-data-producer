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

            // This line will be replaced during data generation
            // The variables will be generated and inserted instead of this line.
            sb.AppendFormat("<DECLARE ITEM{0}>", item.Order);
            sb.AppendLine();
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
                string value = CreateParameterName(item, col);
                sb.Append("\t");
                sb.Append(value);
                sb.Append(col.OrdinalPosition == item.TargetTable.Columns.Count ? "" : ", ");
                sb.AppendLine();
            }
            sb.Append(")");

            return sb.ToString();
        }

        private string CreateParameterName(ExecutionItem item, ColumnEntity col)
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

                sb.AppendFormat("-- Item {0}, {1}.{2}", item.Order, item.TargetTable.TableSchema, item.TargetTable.TableName);
                sb.AppendLine();
                foreach (ColumnEntity col in item.TargetTable.Columns.Where(x => x.IsIdentity == false))
                {
                    // create declaration of the columns datatype, set the value to the generated value generated from the columns generator and its generatorParameter
                    sb.AppendFormat("DECLARE @i{0}_{1} {2} = {3};", item.Order, col.ColumnName, col.ColumnDataType, col.Generator.GenerateValue(n));
                    sb.AppendLine();
                }
                sb.AppendLine();

                string itemNumber = string.Format("<DECLARE ITEM{0}>", item.Order);
                modified = modified.Replace(itemNumber, sb.ToString());
            }

            return modified;
        }


        public IEnumerable<SqlParameter> GenerateParameters(int n, IEnumerable<ExecutionItem> execItems)
        {
            List<SqlParameter> parms = new List<SqlParameter>();

            foreach (var execItem in execItems)
            {
                // Skip tables with no columns
                if (execItem.TargetTable.Columns.Count == 0)
                {
                    continue;
                }

                foreach (ColumnEntity col in execItem.TargetTable.Columns.Where(x => x.IsIdentity == false))
                {
                    string paramName = CreateParameterName(execItem, col);
                    object paramValue = col.Generator.GenerateValue(n);

                    parms.Add(new SqlParameter(paramName, paramValue));
                }
            }

            return parms;
        }

    }
}
