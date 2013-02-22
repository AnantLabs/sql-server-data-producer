using SQLDataProducer.Entities.DatabaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Entities.DataEntities.Collections
{
    public class DataRowSet : List<RowEntity>
    {
        public bool IsIdentityInsert { get; set; }
        

        public TableEntity TargetTable { get; set; }

        public int Order { get; set; }
    }
}
