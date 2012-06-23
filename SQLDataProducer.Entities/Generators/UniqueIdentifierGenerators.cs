using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SQLDataProducer.Entities.Generators.Collections;

namespace SQLDataProducer.Entities.Generators
{
   public partial class Generator
    {
      
        //private UniqueIdentifierGenerator(string name, ValueCreatorDelegate generator, GeneratorParameterCollection genParams)
        //    : base(name, generator, genParams)
        //{
        //}
        

        internal static ObservableCollection<Generator> GetGUIDGenerators()
        {
            ObservableCollection<Generator> valueGenerators = new ObservableCollection<Generator>();
            valueGenerators.Add(CreateRandomGUIDGenerator());
            valueGenerators.Add(CreateQueryGenerator());
            valueGenerators.Add(StaticGUID());

            return valueGenerators;
        }

        private static Generator CreateRandomGUIDGenerator()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            Generator gen = new Generator("Random GUID", (n, p) =>
            {
                return Wrap(Guid.NewGuid());
            }
                , paramss);
            return gen;
        }

        private static Generator StaticGUID()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            paramss.Add(new GeneratorParameter("GUID", new Guid()));

            Generator gen = new Generator("Static GUID", (n, p) =>
            {
                string value = GetParameterByName(p, "GUID").ToString();

                return Wrap(value);
            }
                , paramss);
            return gen;
        }
    }
}
