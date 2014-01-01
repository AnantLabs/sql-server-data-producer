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

namespace SQLDataProducer.Entities.Generators.StringGenerators
{
    public class StaticStringGenerator : StringGeneratorBase
    {
        public static readonly string GENERATOR_NAME = "Static String";

        public StaticStringGenerator(ColumnDataTypeDefinition datatype)
            : base(GENERATOR_NAME, datatype)
        {
            GeneratorParameters.Add(new GeneratorParameter("Value", "", GeneratorParameterParser.StringParser));
            GeneratorParameters.Add(new GeneratorParameter("Length", datatype.MaxLength, GeneratorParameterParser.IntegerParser));
        }

        protected override object InternalGenerateValue(long n, Collections.GeneratorParameterCollection paramas)
        {
            int l = paramas.GetValueOf<int>("Length");
            return paramas.GetValueOf<string>("Value").SubstringWithMaxLength(l);
        }
    }
}
