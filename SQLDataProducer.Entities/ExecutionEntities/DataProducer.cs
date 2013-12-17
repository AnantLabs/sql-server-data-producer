// Copyright 2012-2013 Peter Henell

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
        public DataProducer(ValueStore valuestore)
        {
            ValidateUtil.ValidateNotNull(valuestore, "valuestore");
            this.ValueStorage = valuestore;
        }

        public DataRowEntity ProduceRow(TableEntity table, long n)
        {
            ValidateUtil.ValidateNotNull(table, "table");
            var row = new DataRowEntity(table);

            foreach (var col in table.Columns)
            {
                var value = col.GenerateValue(n);
                Guid key;

                if (col.Generator.IsTakingValueFromOtherColumn)
                {
                    if (value.GetType().Equals(typeof(Guid)))
                    {
                        key = (Guid)value;
                    }
                    else
                    {
                        throw new InvalidCastException("The generator returned a non-GUID type as key when generating value from other column");
                    }
                }
                else // if col is generating identity or other value at insert, do not generate value
                {
                    if (col.IsIdentity)
                    {
                        key = col.ColumnIdentity;
                        value = null;
                    }
                    else
                    {
                        key = Guid.NewGuid();
                    }
                    
                    ValueStorage.Put(key, value);
                }
                
                row.AddField(col.ColumnName, key, col.ColumnDataType, col.IsIdentity);
            }

            return row;
        }
      
        public IEnumerable<DataRowEntity> ProduceRows(IEnumerable<TableEntity> tables, Func<long> getN)
        {
            ValidateUtil.ValidateNotNull(getN, "getN");
            ValidateUtil.ValidateNotNull(tables, "tables");

            foreach (var table in tables)
            {
                yield return ProduceRow(table, getN());
            }
        }

        public ValueStore ValueStorage { get; private set; }
    }
}
