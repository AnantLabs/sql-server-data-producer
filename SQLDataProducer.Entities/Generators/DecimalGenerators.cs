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

using System.Collections.ObjectModel;
using SQLDataProducer.Entities.Generators.Collections;
using System;

namespace SQLDataProducer.Entities.Generators
{
    public partial class Generator
    {
        public static readonly string GENERATOR_RandomDecimal = "Random Decimal";
        public static readonly string GENERATOR_CountingUpDecimal = "Counting up Decimal";
        public static ObservableCollection<Generator> GetDecimalGenerators(double maxValue)
        {
            ObservableCollection<Generator> valueGenerators = new ObservableCollection<Generator>();
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
        private static Generator CreateRandomDecimalGenerator(double dataTypeMax)
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("MinValue", 0.0));
            paramss.Add(new GeneratorParameter("MaxValue", 10000000.0));

            Generator gen = new Generator(GENERATOR_RandomDecimal, (n, p) =>
            {
                double maxValue = double.Parse(GetParameterByName(p, "MaxValue").ToString());
                double minValue = double.Parse(GetParameterByName(p, "MinValue").ToString());
                var newMax = Math.Min(dataTypeMax, maxValue);
                return (((RandomSupplier.Instance.GetNextDouble() * double.MaxValue) % newMax) + minValue);
            }
                , paramss);
            return gen;
        }

        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.Decimal)]
        private static Generator CreateDecimalUpCounter(double dataTypeMax)
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            paramss.Add(new GeneratorParameter("MinValue", 0.0));
            paramss.Add(new GeneratorParameter("MaxValue", 10000000.0));
            paramss.Add(new GeneratorParameter("Step", 1.0));

            Generator gen = new Generator(GENERATOR_CountingUpDecimal, (n, p) =>
            {
                double maxValue = double.Parse(GetParameterByName(p, "MaxValue").ToString());
                double minValue = double.Parse(GetParameterByName(p, "MinValue").ToString());
                double step = double.Parse(GetParameterByName(p, "Step").ToString());

                var newMax = Math.Min(dataTypeMax, maxValue);
                return ((minValue + (step * (n - 1))) % newMax);
            }
                , paramss);
            return gen;
        }

                
    }
}
