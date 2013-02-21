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
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Entities.DatabaseEntities
{
    public class RowEntity : EntityBase//, IListSource, ICustomTypeDescriptor
    {
        public List<DataCell> Cells { get; set; }
        public int RowNumber { get; set; }

        private RowEntity()
        {
            Cells = new List<DataCell>();
        }

        public static RowEntity Create(TableEntity table, long n, int rowNumber)
        {
            var r = new RowEntity();
            r.RowNumber = rowNumber;

            for (int i = 0; i < table.Columns.Count; i++)
            {
                var col = table.Columns[i];
                if (col.IsIdentity && col.Generator.GeneratorName == Generators.Generator.GENERATOR_IdentityFromSqlServerGenerator)
                    continue;

                r.Cells.Add(new DataCell
                {
                    Column = col,
                    Value = col.GenerateValue(n)
                });
            }

            return r;
        }
    }
}
