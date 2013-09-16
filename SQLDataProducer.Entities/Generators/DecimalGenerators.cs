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

using System.Collections.ObjectModel;
using SQLDataProducer.Entities.Generators.Collections;
using System;

namespace SQLDataProducer.Entities.Generators
{
    public partial class Generator
    {
        public static readonly string GENERATOR_RandomDecimal = "Random Decimal";
        public static readonly string GENERATOR_CountingUpDecimal = "Counting up Decimal";
        public static readonly string GENERATOR_StaticNumberDecimal = "Static Decimal Number";

        public static ObservableCollection<Generator> GetDecimalGenerators(decimal maxValue)
        {
            ObservableCollection<Generator> valueGenerators = new ObservableCollection<Generator>();
            valueGenerators.Add(CreateStaticDecimalNumberGenerator());
            valueGenerators.Add(CreateDecimalUpCounter(maxValue));
            valueGenerators.Add(CreateQueryGenerator());
            valueGenerators.Add(CreateRandomDecimalGenerator(maxValue));
            valueGenerators.Add(CreateExponentialRandomNumbersGenerator());
            valueGenerators.Add(CreateNormallyDistributedRandomGenerator());
            valueGenerators.Add(CreateWeibullRandomNumbersGenerator());
            valueGenerators.Add(CreateLaplaceRandomNumbersGenerator());

            return valueGenerators;
        }

        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.Decimal)]
        private static Generator CreateRandomDecimalGenerator(decimal dataTypeMax)
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("MinValue", 0.0, GeneratorParameterParser.DecimalParser));
            paramss.Add(new GeneratorParameter("MaxValue", 10000000.0, GeneratorParameterParser.DecimalParser));

            Generator gen = new Generator(GENERATOR_RandomDecimal, (n, p) =>
            {
                decimal maxValue = (decimal)(double)p.GetParameterByName( "MaxValue");
                decimal minValue = (decimal)(double)p.GetParameterByName( "MinValue");
                var newMax = Math.Min(dataTypeMax, maxValue);
                return (((RandomSupplier.Instance.GetNextDouble() * decimal.MaxValue) % newMax) + minValue);
            }
                , paramss);
            return gen;
        }

        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.Decimal)]
        private static Generator CreateDecimalUpCounter(decimal dataTypeMax)
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            paramss.Add(new GeneratorParameter("MinValue", 0.0, GeneratorParameterParser.DecimalParser));
            paramss.Add(new GeneratorParameter("MaxValue", 10000000.0, GeneratorParameterParser.DecimalParser));
            paramss.Add(new GeneratorParameter("Step", 1.0, GeneratorParameterParser.DecimalParser));

            Generator gen = new Generator(GENERATOR_CountingUpDecimal, (n, p) =>
            {
                decimal maxValue = (decimal)(double)p.GetParameterByName( "MaxValue");
                decimal minValue = (decimal)(double)p.GetParameterByName( "MinValue");
                decimal step = (decimal)(double)p.GetParameterByName( "Step");

                var newMax = Math.Min(dataTypeMax, maxValue);
                return ((minValue + (step * (n - 1))) % newMax);
            }
                , paramss);
            return gen;
        }
        
        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.Decimal)]
        private static Generator CreateStaticDecimalNumberGenerator()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("Static Decimal Value", 0.0, GeneratorParameterParser.DecimalParser));
            
            Generator gen = new Generator(GENERATOR_StaticNumberDecimal, (n, p) =>
            {
                double value = (double)p.GetParameterByName( "Static Decimal Value");
                
                return value;
            }
                , paramss);
            return gen;
        }
        
                
    }
}
