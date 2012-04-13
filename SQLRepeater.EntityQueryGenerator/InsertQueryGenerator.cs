using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLRepeater.Entities;
using System.Data.SqlClient;
using SQLRepeater.DatabaseEntities.Entities;

namespace SQLRepeater.EntityQueryGenerator
{
    public class InsertQueryGenerator
    {
        public string GenerateQueryFor(TableEntity table)
        {
            string sql = string.Empty;
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

        //public SqlParameter[] CreateParameters(int n)
        //{ 
            
        //}
    }
}
