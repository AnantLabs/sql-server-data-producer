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
    public class RandomNormalDistributionIntGenerator : IntegerGeneratorBase
    {
        public static readonly string GENERATOR_NAME = "Normally Distributed Random Numbers";

        public RandomNormalDistributionIntGenerator(ColumnDataTypeDefinition datatype)
            : base(GENERATOR_NAME, datatype)
        {
            GeneratorParameters.Add(new GeneratorParameter("StartValue", 0, GeneratorParameterParser.IntegerParser));
            GeneratorParameters.Add(new GeneratorParameter("Step", 1, GeneratorParameterParser.IntegerParser));
        }

        protected override object InternalGenerateValue(long n, Collections.GeneratorParameterCollection paramas)
        {
            ///// <summary>
            ///// http://www.sqlservercentral.com/articles/SQL+Uniform+Random+Numbers/91103/
            ///// </summary>
            ///// <returns></returns>
            //[GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.Integer)]
            //public static Generator CreateNormallyDistributedRandomGenerator()
            //{
            //    GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            //    paramss.Add(new GeneratorParameter("Mean", new decimal(1.1), GeneratorParameterParser.DecimalParser));
            //    paramss.Add(new GeneratorParameter("StDev", new decimal(1.1), GeneratorParameterParser.DecimalParser));

            //    Generator gen = new Generator(GENERATOR_NormallyDistributedRandomNumbers, (n, p) =>
            //    {
            //        double Mean = (double)p.GetValueOf<decimal>("Mean");
            //        double StDev = (double)p.GetValueOf<decimal>("StDev");
            //        double URN1 = RandomSupplier.Instance.GetNextDouble();
            //        double URN2 = RandomSupplier.Instance.GetNextDouble();

            //        return (StDev * Math.Sqrt(-2 * Math.Log(URN1)) * Math.Cos(2 * Math.Acos(-1.0) * URN2)) + Mean;
            //    }
            //        , paramss);
            //    return gen;
            //}
            return n;
        }
    }
}
