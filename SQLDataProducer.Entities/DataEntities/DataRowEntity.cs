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
        private readonly TableEntity table;

        public List<DataFieldEntity> Fields
        {
            get { return _fields; }
            private set { _fields = value; }
        }

        public DataRowEntity(TableEntity table)
        {
            Fields = new List<DataFieldEntity>();
            this.table = table;
        }

        public DataRowEntity AddField(string fieldName, Guid valueKey, ColumnDataTypeDefinition datatype, bool producesValue)
        {
            Fields.Add(new DataFieldEntity(fieldName, valueKey, datatype, producesValue));
            return this;
        }
    }
}
