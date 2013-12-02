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
    public class RandomLaplaceIntGenerator : IntegerGeneratorBase
    {
        public static readonly string GENERATOR_NAME = "Laplace Random Numbers";

        public RandomLaplaceIntGenerator(ColumnDataTypeDefinition datatype)
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
            //public static Generator CreateLaplaceRandomNumbersGenerator()
            //{
            //    GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            //    paramss.Add(new GeneratorParameter("u", new decimal(1.1), GeneratorParameterParser.DecimalParser));
            //    paramss.Add(new GeneratorParameter("b", new decimal(1.1), GeneratorParameterParser.DecimalParser));

            //    Generator gen = new Generator(GENERATOR_LaplaceRandomNumbers, (n, p) =>
            //    {
            //        double u = (double)p.GetValueOf<decimal>("u");
            //        double b = (double)p.GetValueOf<decimal>("b");
            //        double URN1 = (double)RandomSupplier.Instance.GetNextDecimal();

            //        int s = 0;
            //        if (0 < URN1 - 0.5)
            //            s = 1;
            //        if (0 > URN1 - 0.5)
            //            s = -1;

            //        return u - b * Math.Log(1 - 2 * Math.Abs(URN1 - 0.5)) * s;
            //        //        RETURN @u - @b * LOG(1 - 2 * ABS(@URN - 0.5)) *
            //        //CASE WHEN 0 < @URN - 0.5 THEN 1 
            //        //     WHEN 0 > @URN - 0.5 THEN -1 
            //        //     ELSE 0 
            //        //END

            //    }
            //        , paramss);
            //    return gen;
            //}
            return n;
        }
    }
}
