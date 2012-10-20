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

        /// <summary>
        /// Clones the collection of ColumnEntities.
        /// </summary>
        /// <returns></returns>
        internal ColumnEntityCollection Clone()
        {
            var cols = from c in Items
                       select new ColumnEntity(c.ColumnName, c.ColumnDataType, c.IsIdentity, c.OrdinalPosition, c.IsForeignKey, c.ForeignKey.Clone()
                           , c.PossibleGenerators.Clone(), c.Generator.GeneratorName);
                           

            return new ColumnEntityCollection(cols);
        }

        
    }
}
