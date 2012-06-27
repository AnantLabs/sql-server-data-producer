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
        internal static System.Collections.ObjectModel.ObservableCollection<Generator> GetStringGenerators(int length)
        {
            ObservableCollection<Generator> valueGenerators = new ObservableCollection<Generator>();
            valueGenerators.Add(CreateCountriesGenerator(length));
            valueGenerators.Add(CreateFemaleNameGenerator(length));
            valueGenerators.Add(CreateMaleNameGenerator(length));
            valueGenerators.Add(CreateStaticStringGeneratorBase(length));
            valueGenerators.Add(CreateQueryGenerator());
            valueGenerators.Add(CreateCityGenerator(length));
            valueGenerators.Add(CreateUserNameGenerator(length));
            return valueGenerators;
        }


        static Generator()
        {
            if (_countries == null)
            {
                _countries = GetLinesFromFile( @".\Generators\resources\Countries.txt");
            }
            if (_females == null)
            {
                _females = GetLinesFromFile(@".\Generators\resources\FemaleNames.txt");
            }
            if (_males == null)
            {
                _males = GetLinesFromFile(@".\Generators\resources\MaleNames.txt");
            }
            if (_cities == null)
            {
                _cities = GetLinesFromFile(@".\Generators\resources\SwedishCities.txt");
            }
            if (_userNames == null)
            {
                _userNames = GetLinesFromFile(@".\Generators\resources\UserNames.txt");
            }
        }

        private static List<string> GetLinesFromFile(string fileName)
        {
            List<string> lines = new List<string>();
            if (System.IO.File.Exists(fileName))
                lines.AddRange(System.IO.File.ReadAllLines(fileName));
            else
                lines.Add(string.Format("{0} file not found", fileName));

            return lines;
        }

        private static Generator CreateStaticStringGeneratorBase(int length)
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            paramss.Add(new GeneratorParameter("Value", ""));
            paramss.Add(new GeneratorParameter("Length", length, false));

            Generator gen = new Generator("Static String", (n, p) =>
            {
                int l = int.Parse(GetParameterByName(p, "Length").ToString());
                return Wrap(GetParameterByName(p, "Value").ToString().SubstringWithMaxLength(l));
            }
                , paramss);
            return gen;
        }

        private static Generator CreateCountriesGenerator(int length)
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("Length", length, false));
            Generator gen = new Generator("Countries", (n, p) =>
            {
                int l = int.Parse(GetParameterByName(p, "Length").ToString());
                return Wrap(CountryList[n % CountryList.Count].SubstringWithMaxLength(l));
            }
                , paramss);
            return gen;
        }

        private static Generator CreateFemaleNameGenerator(int length)
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("Length", length, false));
            Generator gen = new Generator("Female names", (n, p) =>
            {
                int l = int.Parse(GetParameterByName(p, "Length").ToString());
                return Wrap(Females[n % Females.Count].SubstringWithMaxLength(l));
            }
                , paramss);
            return gen;
        }
        private static Generator CreateMaleNameGenerator(int length)
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("Length", length, false));
            Generator gen = new Generator("Male names", (n, p) =>
            {
                int l = int.Parse(GetParameterByName(p, "Length").ToString());
                return Wrap(Males[n % Males.Count].SubstringWithMaxLength(l));
            }
               , paramss);
            return gen;
        }

        private static Generator CreateCityGenerator(int length)
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("Length", length, false));
            Generator gen = new Generator("Cities", (n, p) =>
            {
                int l = int.Parse(GetParameterByName(p, "Length").ToString());
                return Wrap(Cities[n % Cities.Count].SubstringWithMaxLength(l));
            }
                , paramss);
            return gen;
        }

        private static Generator CreateUserNameGenerator(int length)
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("Length", length, false));
            Generator gen = new Generator("User Names", (n, p) =>
            {
                int l = int.Parse(GetParameterByName(p, "Length").ToString());
                return Wrap(UserNames[n % UserNames.Count].SubstringWithMaxLength(l));
            }
                , paramss);
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
