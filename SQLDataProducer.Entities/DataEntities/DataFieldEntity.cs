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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Entities.DataEntities
{
    public class DataFieldEntity
    {

        private Guid _keyValue;
        public Guid KeyValue
        {
            get { return _keyValue; }
            private set { _keyValue = value; }
        }

        private string _fieldName;
        public string FieldName
        {
            get { return _fieldName; }
            private set { _fieldName = value; }
        }
        
        private bool _producesValue;
        public bool ProducesValue
        {
            get { return _producesValue; }
            private set { _producesValue = value; }
        }

        private ColumnDataTypeDefinition _datatype;
        public ColumnDataTypeDefinition DataType
        {
            get { return _datatype; }
            private set { _datatype = value; }
        }
        
        public DataFieldEntity(string fieldName, Guid valueKey, ColumnDataTypeDefinition datatype, bool producesValue)
        {
            this.FieldName = fieldName;
            this.KeyValue = valueKey;
            this.ProducesValue = producesValue;
            this.DataType = datatype;
        }
    }
}
