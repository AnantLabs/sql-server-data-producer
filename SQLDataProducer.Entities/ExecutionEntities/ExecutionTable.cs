using SQLDataProducer.Entities.DatabaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataProducer.Entities.ExecutionEntities
{
    public class ExecutionTable
    {
        TableEntity table;
        long n;

        public ExecutionTable(TableEntity table, long n)
        {
            this.table = table;
            this.n = n;
        }

        public TableEntity Table
        {
            get { return table; }
        }

        public long N
        {
            get { return n; }
        }


    }
}
