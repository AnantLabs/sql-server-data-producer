using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SQLRepeater.DatabaseEntities.Entities;
using SQLRepeater.Entities.Generators;

namespace SQLRepeater.Entities.DatabaseEntities.Collections
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
