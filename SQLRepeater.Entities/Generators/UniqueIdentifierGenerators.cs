using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SQLRepeater.Entities.Generators
{
    public class UniqueIdentifierGenerator : GeneratorBase
    {
      
        private UniqueIdentifierGenerator(string name, ValueCreatorDelegate generator, ObservableCollection<GeneratorParameter> genParams)
            : base(name, generator, genParams)
        {
        }
        

        internal static ObservableCollection<GeneratorBase> GetGenerators()
        {
            ObservableCollection<GeneratorBase> valueGenerators = new ObservableCollection<GeneratorBase>();
            valueGenerators.Add(CreateRandomGenerator());
            valueGenerators.Add(CreateQueryGenerator());
            valueGenerators.Add(StaticGUID());

            return valueGenerators;
        }

        private static UniqueIdentifierGenerator CreateRandomGenerator()
        {
            ObservableCollection<GeneratorParameter> paramss = new ObservableCollection<GeneratorParameter>();

            UniqueIdentifierGenerator gen = new UniqueIdentifierGenerator("Random GUID", (n, p) =>
            {
                return Wrap(Guid.NewGuid());
            }
                , paramss);
            return gen;
        }

        //private static UniqueIdentifierGenerator Query()
        //{
        //    ObservableCollection<GeneratorParameter> paramss = new ObservableCollection<GeneratorParameter>();

        //    paramss.Add(new GeneratorParameter("Query", "select newid()"));

        //    UniqueIdentifierGenerator gen = new UniqueIdentifierGenerator("Custom SQL Query", (n, p) =>
        //    {
        //        string value = GetParameterByName<string>(p, "Query");

        //        return string.Format("({0})", value);
        //    }
        //        , paramss);
        //    return gen;
        //}


        private static UniqueIdentifierGenerator StaticGUID()
        {
            ObservableCollection<GeneratorParameter> paramss = new ObservableCollection<GeneratorParameter>();

            paramss.Add(new GeneratorParameter("GUID", new Guid()));

            UniqueIdentifierGenerator gen = new UniqueIdentifierGenerator("Static GUID", (n, p) =>
            {
                string value = GetParameterByName(p, "GUID").ToString();

                return Wrap(value);
            }
                , paramss);
            return gen;
        }
    }
}
