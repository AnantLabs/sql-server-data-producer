// Copyright 2012-2014 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using SQLDataProducer.Entities.DatabaseEntities;
using System;

namespace SQLDataProducer.Entities.Generators.IntGenerators
{
    public class RandomForeignKeyIntGenerator : IntegerGeneratorBase
    {
        public static readonly string GENERATOR_NAME = "Random FOREIGN KEY Value (EAGER)";

        public RandomForeignKeyIntGenerator(ColumnDataTypeDefinition datatype)
            : base(GENERATOR_NAME, datatype)
        {
            GeneratorParameters.Add(new GeneratorParameter("StartValue", 0, GeneratorParameterParser.IntegerParser));
            GeneratorParameters.Add(new GeneratorParameter("Step", 1, GeneratorParameterParser.IntegerParser));
        }

        protected override object InternalGenerateValue(long n, Collections.GeneratorParameterCollection paramas)
        {
            //[GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.Integer)]
            //public static Generator CreateRandomForeignKeyGenerator(ObservableCollection<string> fkkeys)
            //{
            //    GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            //    GeneratorParameter foreignParam = new GeneratorParameter("Keys", fkkeys, GeneratorParameterParser.ObjectParser, false);
            //    paramss.Add(foreignParam);
            //    Generator gen = new Generator(GENERATOR_RandomFOREIGNKEYValueEAGER, (n, p) =>
            //    {
            //        ObservableCollection<string> keys = p.GetValueOf<ObservableCollection<string>>("Keys");
            //        if (keys == null || keys.Count == 0)
            //            throw new ArgumentException("There are no foreign keys in the table that this column references");

            //        return keys[RandomSupplier.Instance.GetNextInt() % keys.Count];
            //    }
            //        , paramss);
            //    return gen;
            //}
            return n;
        }
    }
}
