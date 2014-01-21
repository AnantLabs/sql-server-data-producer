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
    public abstract class IntegerGeneratorBase : AbstractValueGenerator
    {
        public IntegerGeneratorBase(string generatorName, ColumnDataTypeDefinition dataType, bool isTakingValueFromOtherColumn = false)
            : base(generatorName, isTakingValueFromOtherColumn)
        {
            GeneratorParameters.Add(new GeneratorParameter("MaxValue", dataType.MaxValue, GeneratorParameterParser.LonglParser, false));
            GeneratorParameters.Add(new GeneratorParameter("MinValue", dataType.MinValue, GeneratorParameterParser.LonglParser, false));
        }

        protected override object ApplyGeneratorTypeSpecificLimits(object value)
        {
            if (value is DBNull)
            {
                return value;
            }
            if (value is long)
            {
                var newValue = (long)value;
                var max = GeneratorParameters.GetValueOf<long>("MaxValue");
                var min = GeneratorParameters.GetValueOf<long>("MinValue");

                return Math.Min(Math.Max(min, newValue), max);
            }
            else
            {
                return value;
            }

        }
    }
}
