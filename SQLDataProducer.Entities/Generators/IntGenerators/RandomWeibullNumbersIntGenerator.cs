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
    public class RandomWeibullNumbersIntGenerator : IntegerGeneratorBase
    {
        public static readonly string GENERATOR_NAME = "Weibull Random Numbers";


        public RandomWeibullNumbersIntGenerator(ColumnDataTypeDefinition datatype)
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
            //public static Generator CreateWeibullRandomNumbersGenerator()
            //{
            //    GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            //    paramss.Add(new GeneratorParameter("Alpha", 1.1, GeneratorParameterParser.DecimalParser));
            //    paramss.Add(new GeneratorParameter("Beta", 1.1, GeneratorParameterParser.DecimalParser));

            //    Generator gen = new Generator(GENERATOR_WeibullRandomNumbers, (n, p) =>
            //    {
            //        double Alpha = (double)p.GetValueOf<decimal>("Alpha");
            //        double Beta = (double)p.GetValueOf<decimal>("Beta");
            //        double URN1 = (double)RandomSupplier.Instance.GetNextDecimal();

            //        //RETURN POWER((-1. / @Alpha) * LOG(1. - @URN), 1./@Beta)
            //        return Math.Pow((-1.0 / Alpha) * Math.Log(1.0 - URN1), 1.0 / Beta);
            //    }
            //        , paramss);
            //    return gen;
            //}
            return n;
        }
    }
}
