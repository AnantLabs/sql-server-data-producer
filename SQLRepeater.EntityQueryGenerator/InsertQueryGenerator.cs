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
        public string GenerateQueryFor(TableEntity table)
        {
            //string sql = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("INSERT {0}.{1} (", table.TableSchema, table.TableName);
            foreach (var col in table.Columns.Where( x => x.IsIdentity == false))
            {
                if (col.OrdinalPosition == table.Columns.Count)
                    sb.AppendFormat("{0}", col.ColumnName);
                else
                    sb.AppendFormat("{0},", col.ColumnName);
            }

            sb.Append(")");
            sb.AppendLine();
            sb.Append("VALUES(");
            foreach (var col in table.Columns.Where(x => x.IsIdentity == false))
            {
                if (col.OrdinalPosition == table.Columns.Count)
                    sb.AppendFormat("@{0}", col.ColumnName);
                else
                    sb.AppendFormat("@{0},", col.ColumnName);
            }
            sb.Append(")");
            
            return sb.ToString(); ;
        }

      



        public string GenerateQueryForExecutionItems(IList<ExecutionItem> executionItems)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("set nocount on");
            sb.AppendLine();

            sb.AppendLine("-- Declarations");
            sb.AppendLine();
            //-- Declarations
            //declare @i1_OperatorIdentifier varchar(50)
            //declare @i1_HasPrivatePlayerGroups bit

            sb.Append("{0}"); // all of these should be generated on the fly for each iteration later using string.format(this, paramcreatorthing);
            sb.AppendLine();

            foreach (var item in executionItems)
            {
                bool hasIdentity = item.TargetTable.Columns.Any(col => col.IsIdentity);

                sb.AppendFormat("-- Insert item {0}", item.Order);
                sb.AppendLine();
                if (hasIdentity)
                {
                    sb.AppendFormat("declare @i{0}_identity bigint", item.Order);
                    sb.AppendLine();
                }
                
                sb.Append(GenerateQueryForExecutionItem(item));

                if (hasIdentity)
                {
                    sb.AppendLine();
                    sb.AppendFormat("select @i{0}_identity = scope_identity()", item.Order);
                }
                
                sb.AppendLine();
                sb.AppendFormat("-- done insert item {0}", item.Order);
                sb.AppendLine();
                
            }

            return sb.ToString();
        }

        private string GenerateQueryForExecutionItem(ExecutionItem item)
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

    }
}
