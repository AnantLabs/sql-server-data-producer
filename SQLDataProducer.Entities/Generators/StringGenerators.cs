using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SQLDataProducer.Entities.Generators.Collections;
//

namespace SQLDataProducer.Entities.Generators
{
    public partial class Generator
    {
        internal static System.Collections.ObjectModel.ObservableCollection<Generator> GetStringGenerators()
        {
            ObservableCollection<Generator> valueGenerators = new ObservableCollection<Generator>();
            valueGenerators.Add(CreateCountriesGenerator());
            valueGenerators.Add(CreateFemaleNameGenerator());
            valueGenerators.Add(CreateMaleNameGenerator());
            valueGenerators.Add(CreateStaticStringGeneratorBase());
            valueGenerators.Add(CreateQueryGenerator());
            valueGenerators.Add(CreateCityGenerator());
            valueGenerators.Add(CreateUserNameGenerator());
            return valueGenerators;
        }


        static Generator()
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
            if (_cities == null)
            {
                _cities = new List<string>();
                _cities.AddRange(System.IO.File.ReadAllLines(@".\Generators\resources\SwedishCities.txt"));
            }
            if (_userNames == null)
            {
                _userNames = new List<string>();
                _userNames.AddRange(System.IO.File.ReadAllLines(@".\Generators\resources\UserNames.txt"));
            }
        }

        private static Generator CreateStaticStringGeneratorBase()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            paramss.Add(new GeneratorParameter("Value", ""));

            Generator gen = new Generator("Static String", (n, p) =>
            {
                return Wrap(GetParameterByName(p, "Value").ToString());
            }
                , paramss);
            return gen;
        }

        private static Generator CreateCountriesGenerator()
        {
            Generator gen = new Generator("Countries", (n, p) =>
            {
                return Wrap(CountryList[n % CountryList.Count]);
            }
                , null);
            return gen;
        }

        private static Generator CreateFemaleNameGenerator()
        {
            Generator gen = new Generator("Female names", (n, p) =>
            {
                return Wrap(Females[n % Females.Count]);
            }
                , null);
            return gen;
        }
        private static Generator CreateMaleNameGenerator()
        {
            Generator gen = new Generator("Male names", (n, p) =>
            {
                return Wrap(Males[n % Males.Count]);
            }
                , null);
            return gen;
        }

        private static Generator CreateCityGenerator()
        {
            Generator gen = new Generator("Cities", (n, p) =>
            {
                return Wrap(Cities[n % Cities.Count]);
            }
                , null);
            return gen;
        }

        private static Generator CreateUserNameGenerator()
        {
            Generator gen = new Generator("User Names", (n, p) =>
            {
                return Wrap(UserNames[n % UserNames.Count]);
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

        private static List<string> _cities;
        static List<string> Cities
        {
            get
            {

                return _cities;
            }
        }

        private static List<string> _userNames;
        static List<string> UserNames
        {
            get
            {

                return _userNames;
            }
        }

    }
}
