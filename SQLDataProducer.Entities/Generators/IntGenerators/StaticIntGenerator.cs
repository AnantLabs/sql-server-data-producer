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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataProducer.Entities.Generators.IntGenerators
{
    public class StaticIntGenerator : IntegerGeneratorBase
    {
        public static readonly string GENERATOR_NAME = "Static Number";

        public StaticIntGenerator(ColumnDataTypeDefinition datatype)
            : base(GENERATOR_NAME, datatype)
        {
            GeneratorParameters.Add(new GeneratorParameter("StartValue", 0, GeneratorParameterParser.IntegerParser));
            GeneratorParameters.Add(new GeneratorParameter("Step", 1, GeneratorParameterParser.IntegerParser));
        }

        protected override object InternalGenerateValue(long n, Collections.GeneratorParameterCollection paramas)
        {
            //[GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.Integer)]
            //public static Generator CreateStaticNumberGenerator()
            //{
            //    GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            //    paramss.Add(new GeneratorParameter("Number", 0, GeneratorParameterParser.LonglParser));

            //    Generator gen = new Generator(GENERATOR_StaticNumber, (n, p) =>
            //    {
            //        long value = p.GetValueOf<long>("Number");

            //        return value;
            //    }
            //        , paramss);
            //    return gen;
            //}
            return n;
        }
    }
}
