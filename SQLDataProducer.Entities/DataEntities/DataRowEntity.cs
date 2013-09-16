using SQLDataProducer.Entities.DatabaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataProducer.Entities.DataEntities
{
    public class DataRowEntity : EntityBase
    {
        private List<DataFieldEntity> _fields;

        public List<DataFieldEntity> Fields
        {
            get { return _fields; }
            private set { _fields = value; }
        }

        public DataRowEntity()
        {
            Fields = new List<DataFieldEntity>();
        }

        public void AddField(string fieldName, Guid valueKey, ColumnDataTypeDefinition datatype, bool producesValue)
        {
            Fields.Add(new DataFieldEntity(fieldName, valueKey, datatype, producesValue));
        }
    }
}
