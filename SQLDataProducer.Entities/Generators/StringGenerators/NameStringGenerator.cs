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
    public class NameStringGenerator : StringGeneratorBase
    {
        public static readonly string GENERATOR_NAME = "Names";

        public NameStringGenerator(ColumnDataTypeDefinition datatype)
            : base(GENERATOR_NAME, datatype)
        {
            GeneratorParameters.Add(new GeneratorParameter("Length", datatype.MaxLength, GeneratorParameterParser.IntegerParser));
        }

        private static List<string> _names;
        static List<string> Names
        {
            get
            {
                if (_names == null)
                {
                    _names = GetLinesFromFile(@".\Generators\resources\FemaleNames.txt");
                    _names.AddRange(GetLinesFromFile(@".\Generators\resources\MaleNames.txt"));
                }
                return _names;
            }
        }

        protected override object InternalGenerateValue(long n, Collections.GeneratorParameterCollection paramas)
        {
            int l = paramas.GetValueOf<int>("Length");
            return Names[n.LongToInt() % Names.Count].SubstringWithMaxLength(l);
        }
    }
}
