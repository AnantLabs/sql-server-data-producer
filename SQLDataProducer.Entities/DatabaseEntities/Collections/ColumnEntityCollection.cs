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

using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using SQLDataProducer.Entities.DatabaseEntities;
using System.Xml.Linq;
using System.Text;

namespace SQLDataProducer.Entities.DatabaseEntities.Collections
{
    public class ColumnEntityCollection : IEnumerable<ColumnEntity>
    {
        private readonly Dictionary<string, ColumnEntity> columns;

        public ColumnEntityCollection()
            : base()
        {
            columns = new Dictionary<string, ColumnEntity>();
        }

        public ColumnEntityCollection Add(ColumnEntity column)
        {
            columns.Add(column.ColumnName, column);
            return this;
        }

        public void AddRange(IEnumerable<ColumnEntity> columns)
        {
            foreach (var column in columns)
            {
                Add(column);
            }
        }

        public ColumnEntity this[int index]
        {
            get
            {
                return columns.Values.Skip(index).FirstOrDefault();
            }
        }

        /// <summary>
        /// Clones the collection of ColumnEntities.
        /// </summary>
        /// <returns></returns>
        internal ColumnEntityCollection Clone()
        {
            // TODO: Clone using the same entity as the LOAD/SAVE functionality
            ColumnEntityCollection cols = new ColumnEntityCollection();
            foreach (var key in columns.Keys)
            {
                var clonedColumn = Factories.DatabaseEntityFactory.CloneColumn(columns[key]);

                cols.Add(clonedColumn);
            }

            return cols;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var key in columns.Keys)
            {
                sb.AppendLine(columns[key].ToString());
            }
            return sb.ToString();
        }




        public IEnumerator<ColumnEntity> GetEnumerator()
        {
            return columns.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return columns.Values.GetEnumerator();
        }

        public ColumnEntity Get(string columnName)
        {
            return columns[columnName];
        }
    }
}
