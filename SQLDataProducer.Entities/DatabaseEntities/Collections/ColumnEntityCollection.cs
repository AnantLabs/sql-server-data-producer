// Copyright 2012 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using SQLDataProducer.Entities.DatabaseEntities;
using System.Xml.Linq;

namespace SQLDataProducer.Entities.DatabaseEntities.Collections
{
    public class ColumnEntityCollection : ObservableCollection<ColumnEntity>
    {
        public ColumnEntityCollection()
            : base()
        {
        }

        public ColumnEntityCollection(IEnumerable<ColumnEntity> cols)
            : base (cols)
        {
        }

        public void AddRange(IEnumerable<ColumnEntity> columns)
        {
            foreach (var c in columns)
            {
                base.Add(c);
            }
        }

        /// <summary>
        /// Clones the collection of ColumnEntities.
        /// </summary>
        /// <returns></returns>
        internal ColumnEntityCollection Clone()
        {
            ColumnEntityCollection cols = new ColumnEntityCollection();
            foreach (var c in Items)
            {
                var col = Factories.DatabaseEntityFactory.CloneColumn(c);

                cols.Add(col);
            }

            return new ColumnEntityCollection(cols);
        }

        public void ReadXml(XElement xe)
        {
            foreach (var columnElement in xe.Descendants("Column"))
            {
                ColumnEntity col = new ColumnEntity();
                col.ReadXml(columnElement);
                Items.Add(col);
            }
        }
        
    }
}
