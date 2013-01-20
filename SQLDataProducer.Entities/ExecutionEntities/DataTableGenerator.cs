using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Entities.ExecutionEntities
{
    public class DataTableGenerator
    {
        public static IEnumerable<DataRow> GenerateDataTable(ExecutionItem item, Func<long> getN)
        {
            DataTable dt = AsDataTable(item);

            for (int i = 0; i < item.RepeatCount; i++)
            {
                yield return CreateDataRow(dt, item, getN());
            }

        }

        private static DataTable AsDataTable(ExecutionItem item)
        {
            throw new NotImplementedException();
        }

        private static DataRow CreateDataRow(DataTable dt, ExecutionItem item, long p)
        {
            throw new NotImplementedException();
        }
    }
}
