using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SQLDataProducer.Entities.DatabaseEntities.Collections
{
    public static class GeneratorCollectionExtensions
    {
        public static ObservableCollection<Generators.GeneratorBase> Clone(this ObservableCollection<Generators.GeneratorBase> gens)
        {
            var newGens = new ObservableCollection<Generators.GeneratorBase>();
            foreach (var item in gens)
            {
                newGens.Add(item.Clone());
            }
            return newGens;
        }
    }
}
