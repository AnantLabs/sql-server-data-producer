//// Copyright 2012-2013 Peter Henell

////   Licensed under the Apache License, Version 2.0 (the "License");
////   you may not use this file except in compliance with the License.
////   You may obtain a copy of the License at

////       http://www.apache.org/licenses/LICENSE-2.0

////   Unless required by applicable law or agreed to in writing, software
////   distributed under the License is distributed on an "AS IS" BASIS,
////   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
////   See the License for the specific language governing permissions and
////   limitations under the License.

//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using SQLDataProducer.Entities.DatabaseEntities;
//using SQLDataProducer.Entities.Generators.Collections;
//using System;
////

//namespace SQLDataProducer.Entities.Generators
//{
//    public partial class Generator
//    {
//        public static readonly string GENERATOR_StaticString = "Static String";
//        public static readonly string GENERATOR_Countries = "Countries";
//        public static readonly string GENERATOR_FemaleNames = "Female names";
//        public static readonly string GENERATOR_MaleNames = "Male names";
//        public static readonly string GENERATOR_Cities = "Cities";
//        public static readonly string GENERATOR_UserNames = "User Names";
//        public static readonly string GENERATOR_StringValueFromOtherColumn = "Value from other Column";

//        public static System.Collections.ObjectModel.ObservableCollection<Generator> GetStringGenerators(int length)
//        {
//            ObservableCollection<Generator> valueGenerators = new ObservableCollection<Generator>();
//            valueGenerators.Add(CreateCountriesGenerator(length));
//            valueGenerators.Add(CreateFemaleNameGenerator(length));
//            valueGenerators.Add(CreateMaleNameGenerator(length));
//            valueGenerators.Add(CreateStaticStringGeneratorBase(length));
//            valueGenerators.Add(CreateQueryGenerator());
//            valueGenerators.Add(CreateCityGenerator(length));
//            valueGenerators.Add(CreateUserNameGenerator(length));
//            valueGenerators.Add(CreateRandomGUIDGenerator());
//            valueGenerators.Add(CreateStringValueFromOtherColumnGenerator());
//            return valueGenerators;
//        }


//        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.String)]
//        public static Generator CreateStringValueFromOtherColumnGenerator()
//        {
//            Generator gen = new Generator(GENERATOR_StringValueFromOtherColumn, (n, p) =>
//            {

//                //return System.DBNull.Value;
//                throw new NotImplementedException("Value from other column");
//            }
//                , null);
//            return gen;
//        }
      
       

       

//    }
//}
