// Copyright 2012-2014 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.


using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataProducer.Entities.ExecutionEntities
{
    public sealed class DataProducer 
    {
        public ValueStore ValueStorage { get; private set; }
        private Dictionary<Guid, Guid> columnLastKeyMap;

        public DataProducer(ValueStore valuestore)
        {
            ValidateUtil.ValidateNotNull(valuestore, "valuestore");
            this.ValueStorage = valuestore;
            columnLastKeyMap = new Dictionary<Guid,Guid>();
        }

        public DataRowEntity ProduceRow(TableEntity table)
        {
            ValidateUtil.ValidateNotNull(table, "table");
            var row = new DataRowEntity(table);

            foreach (var col in table.Columns)
            {
                var value = col.GenerateValue();
                Guid key;

                if (col.Generator.IsTakingValueFromOtherColumn)
                {
                    if (value.GetType().Equals(typeof(Guid)))
                    {
                        if(!columnLastKeyMap.TryGetValue((Guid)value, out key))
                        {
                            throw new ArgumentNullException("Trying to take value from column that have not yet generated any value");
                        }
                    }
                    else
                    {
                        throw new InvalidCastException("The generator returned a non-GUID type as key when generating value from other column");
                    }
                }
                else if (col.IsIdentity)
                {
                    key = col.ColumnIdentity;
                    value = null;
                    ValueStorage.Put(key, value);
                    columnLastKeyMap[col.ColumnIdentity] = key;
                }
                else
                {
                    key = Guid.NewGuid();
                    columnLastKeyMap[col.ColumnIdentity] = key;
                    ValueStorage.Put(key, value);
                }
                
                row.AddField(col.ColumnName, key, col.ColumnDataType, col.IsIdentity);
            }

            return row;
        }
      
        public IEnumerable<DataRowEntity> ProduceRows(IEnumerable<TableEntity> tables)
        {
            ValidateUtil.ValidateNotNull(tables, "tables");

            foreach (var table in tables)
            {
                yield return ProduceRow(table);
            }
        }

        
    }
}
