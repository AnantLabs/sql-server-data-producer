// Copyright 2012-2013 Peter Henell

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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataProducer.Entities.Generators.IntGenerators
{
    public class SequentialForeignKeyIntGenerator : IntegerGeneratorBase
    {
        public static readonly string GENERATOR_NAME = "Sequential FOREIGN KEY Value (EAGER)";

        public SequentialForeignKeyIntGenerator(ColumnDataTypeDefinition datatype)
            : base(GENERATOR_NAME, datatype)
        {
            GeneratorParameters.Add(new GeneratorParameter("StartValue", 0, GeneratorParameterParser.IntegerParser));
            GeneratorParameters.Add(new GeneratorParameter("Step", 1, GeneratorParameterParser.IntegerParser));
        }

        protected override object InternalGenerateValue(long n, Collections.GeneratorParameterCollection paramas)
        {
            //[GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.Integer)]
            //public static Generator CreateSequentialForeignKeyGenerator(ObservableCollection<string> fkkeys)
            //{
            //    GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            //    GeneratorParameter foreignParam = new GeneratorParameter("Keys", fkkeys, GeneratorParameterParser.ObjectParser, false);
            //    GeneratorParameter startIndex = new GeneratorParameter("Start Index", 1, GeneratorParameterParser.LonglParser);
            //    GeneratorParameter maxIndex = new GeneratorParameter("Max Index", 1000, GeneratorParameterParser.LonglParser);

            //    paramss.Add(foreignParam);
            //    paramss.Add(startIndex);
            //    paramss.Add(maxIndex);

            //    Generator gen = new Generator(GENERATOR_SequentialFOREIGNKEYValueEAGER, (n, p) =>
            //    {
            //        ObservableCollection<string> keys = p.GetValueOf<ObservableCollection<string>>("Keys");
            //        if (keys == null || keys.Count == 0)
            //            throw new ArgumentException("There are no foreign keys in the table that this column references");

            //        int si = p.GetValueOf<int>("Start Index");
            //        int mi = p.GetValueOf<int>("Max Index");
            //        if (mi > fkkeys.Count)
            //        {
            //            mi = fkkeys.Count;
            //        }
            //        return keys[n.LongToInt() % keys.Count];
            //    }
            //        , paramss);
            //    return gen;
            //}
            return n;
        }
    }
}
