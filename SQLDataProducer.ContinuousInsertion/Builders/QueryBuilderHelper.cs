using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDataProducer.Entities.DatabaseEntities;

namespace SQLDataProducer.ContinuousInsertion.Builders
{
    static class QueryBuilderHelper
    {
        /// <summary>
        /// Create the parameter name used when the generator is Sql Query generator
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string GetSqlQueryParameterName(ColumnEntity c)
        {
            return GetParamName(1, c).Replace("@", "@c_");
        }

        public static string GetParamName(int rep, ColumnEntity col)
        {
            var ei = col.ParentTable.ParentExecutionItem;
            return string.Format("@{0}_{1}_{2}", col.ColumnName.Replace(" ", "").Replace(".", ""), ei.Order, rep);
        }
    }
}
