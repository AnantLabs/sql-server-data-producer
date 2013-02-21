using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Entities.DatabaseEntities
{
    public class DataCell
    {
        public ColumnEntity Column { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0} = '{1}'", Column, Value);
        }
    }
}
