using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SQLRepeater.Entities.Generators.Collections;
//

namespace SQLRepeater.Entities.Generators
{
    public class StringGenerator : GeneratorBase
    {

        private StringGenerator(string name, ValueCreatorDelegate generator, GeneratorParameterCollection genParams)
            : base(name, generator, genParams)
        {
        }

        internal static System.Collections.ObjectModel.ObservableCollection<GeneratorBase> GetGenerators()
        {
            ObservableCollection<GeneratorBase> valueGenerators = new ObservableCollection<GeneratorBase>();
            valueGenerators.Add(CreateCountriesGenerator());
            valueGenerators.Add(CreateFemaleNameGenerator());
            valueGenerators.Add(CreateMaleNameGenerator());
            valueGenerators.Add(CreateStaticStringGenerator());
            valueGenerators.Add(CreateQueryGenerator());

            
            
            return valueGenerators;
        }


        static StringGenerator()
        {
            if (_countries == null)
            {
                _countries = new List<string>();
                _countries.AddRange(System.IO.File.ReadAllLines(@".\Generators\resources\Countries.txt"));
            }
            if (_females == null)
            {
                _females = new List<string>();
                _females.AddRange(System.IO.File.ReadAllLines(@".\Generators\resources\FemaleNames.txt"));
            }
            if (_males == null)
            {
                _males = new List<string>();
                _males.AddRange(System.IO.File.ReadAllLines(@".\Generators\resources\MaleNames.txt"));
            }
        }

        private static StringGenerator CreateStaticStringGenerator()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            paramss.Add(new GeneratorParameter("Value", ""));

            StringGenerator gen = new StringGenerator("Static String", (n, p) =>
            {
                return Wrap(GetParameterByName(p, "Value").ToString());
            }
                , paramss);
            return gen;
        }

        private static StringGenerator CreateCountriesGenerator()
        {
            StringGenerator gen = new StringGenerator("Countries", (n, p) =>
            {
                return Wrap(CountryList[n % CountryList.Count]);
            }
                , null);
            return gen;
        }

        private static StringGenerator CreateFemaleNameGenerator()
        {
            StringGenerator gen = new StringGenerator("Female names", (n, p) =>
            {
                return Wrap(Females[n % Females.Count]);
            }
                , null);
            return gen;
        }
        private static StringGenerator CreateMaleNameGenerator()
        {
            StringGenerator gen = new StringGenerator("Male names", (n, p) =>
            {
                return Wrap(Males[n % Males.Count]);
            }
                , null);
            return gen;
        }

        private static List<string> _countries;
        static List<string> CountryList
        {
            get
            {
                
                return _countries;
            }
        }

        private static List<string> _females;
        static List<string> Females
        {
            get
            {
                
                return _females;
            }
        }
        private static List<string> _males;
        static List<string> Males
        {
            get
            {
                
                return _males;
            }
        }

       
    }
}
