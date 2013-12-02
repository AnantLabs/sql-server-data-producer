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

namespace SQLDataProducer.Entities.Generators.StringGenerators
{
    public abstract class StringGeneratorBase : AbstractValueGenerator
    {
        public StringGeneratorBase(string generatorName, ColumnDataTypeDefinition dataType)
            : base(generatorName)
        {
            GeneratorParameters.Add(new GeneratorParameter("MaxLength", dataType.MaxLength, GeneratorParameterParser.IntegerParser, false));
        }

        protected override object ApplyGeneratorTypeSpecificLimits(object value)
        {
            int maxLength = GeneratorParameters.GetValueOf<int>("MaxLength");
            if (value is string)
                return (value as string).SubstringWithMaxLength(maxLength);
            else
                return value.ToString().SubstringWithMaxLength(maxLength);
        }

        protected static List<string> GetLinesFromFile(string fileName)
        {
            List<string> lines = new List<string>();
            if (System.IO.File.Exists(fileName))
                lines.AddRange(System.IO.File.ReadAllLines(fileName));
            else
                lines.Add(string.Format("{0} file not found", fileName));

            return lines;
        }
    }
}
