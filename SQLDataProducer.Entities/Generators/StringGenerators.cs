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
using System.Collections.ObjectModel;
using SQLDataProducer.Entities.Generators.Collections;
//

namespace SQLDataProducer.Entities.Generators
{
    public partial class Generator
    {
        public static readonly string GENERATOR_StaticString = "Static String";
        public static readonly string GENERATOR_Countries = "Countries";
        public static readonly string GENERATOR_FemaleNames = "Female names";
        public static readonly string GENERATOR_MaleNames = "Male names";
        public static readonly string GENERATOR_Cities = "Cities";
        public static readonly string GENERATOR_UserNames = "User Names";

        public static System.Collections.ObjectModel.ObservableCollection<Generator> GetStringGenerators(int length)
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

        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.String)]
        private static Generator CreateStaticStringGeneratorBase(int length)
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            paramss.Add(new GeneratorParameter("Value", ""));
            paramss.Add(new GeneratorParameter("Length", length, false));

            Generator gen = new Generator(GENERATOR_StaticString, (n, p) =>
            {
                int l = int.Parse(GetParameterByName(p, "Length").ToString());
                return Wrap(GetParameterByName(p, "Value").ToString().SubstringWithMaxLength(l));
            }
                , paramss);
            return gen;
        }

        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.String)]
        private static Generator CreateCountriesGenerator(int length)
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("Length", length, false));
            Generator gen = new Generator(GENERATOR_Countries, (n, p) =>
            {
                int l = int.Parse(GetParameterByName(p, "Length").ToString());
                return Wrap(CountryList[n.LongToInt() % CountryList.Count].SubstringWithMaxLength(l));
            }
                , paramss);
            return gen;
        }

        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.String)]
        private static Generator CreateFemaleNameGenerator(int length)
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("Length", length, false));
            Generator gen = new Generator(GENERATOR_FemaleNames, (n, p) =>
            {
                int l = int.Parse(GetParameterByName(p, "Length").ToString());
                return Wrap(Females[n.LongToInt() % Females.Count].SubstringWithMaxLength(l));
            }
                , paramss);
            return gen;
        }

        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.String)]
        private static Generator CreateMaleNameGenerator(int length)
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("Length", length, false));
            Generator gen = new Generator(GENERATOR_MaleNames, (n, p) =>
            {
                int l = int.Parse(GetParameterByName(p, "Length").ToString());
                return Wrap(Males[n.LongToInt() % Males.Count].SubstringWithMaxLength(l));
            }
               , paramss);
            return gen;
        }

        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.String)]
        private static Generator CreateCityGenerator(int length)
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("Length", length, false));
            Generator gen = new Generator(GENERATOR_Cities, (n, p) =>
            {
                int l = int.Parse(GetParameterByName(p, "Length").ToString());
                return Wrap(Cities[n.LongToInt() % Cities.Count].SubstringWithMaxLength(l));
            }
                , paramss);
            return gen;
        }


        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.String)]
        private static Generator CreateUserNameGenerator(int length)
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("Length", length, false));
            Generator gen = new Generator(GENERATOR_UserNames, (n, p) =>
            {
                int l = int.Parse(GetParameterByName(p, "Length").ToString());
                return Wrap(UserNames[n.LongToInt() % UserNames.Count].SubstringWithMaxLength(l));
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
