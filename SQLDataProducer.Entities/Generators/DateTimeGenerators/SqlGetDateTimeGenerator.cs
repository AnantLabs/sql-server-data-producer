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

namespace SQLDataProducer.Entities.Generators.DateTimeGenerators
{
    public class SqlGetDateTimeGenerator : DateTimeGeneratorBase
    {
        public static readonly string GENERATOR_NAME = "SQL GetDate()";

        public SqlGetDateTimeGenerator(ColumnDataTypeDefinition datatype)
            : base(GENERATOR_NAME, datatype)
        {
            GeneratorParameters.Add(new GeneratorParameter("Shift Days", 0, GeneratorParameterParser.IntegerParser));
            GeneratorParameters.Add(new GeneratorParameter("Shift Hours", 0, GeneratorParameterParser.IntegerParser));
            GeneratorParameters.Add(new GeneratorParameter("Shift Minutes", 0, GeneratorParameterParser.IntegerParser));
            GeneratorParameters.Add(new GeneratorParameter("Shift Seconds", 0, GeneratorParameterParser.IntegerParser));
            GeneratorParameters.Add(new GeneratorParameter("Shift Milliseconds", 0, GeneratorParameterParser.IntegerParser));
        }

        protected override object InternalGenerateValue(long n, Collections.GeneratorParameterCollection paramas)
        {
            int d = paramas.GetValueOf<int>( "Shift Days");
                int h = paramas.GetValueOf<int>( "Shift Hours");
                int min = paramas.GetValueOf<int>( "Shift Minutes");
                int s = paramas.GetValueOf<int>( "Shift Seconds");
                int ms = paramas.GetValueOf<int>( "Shift Milliseconds");
                var ts = new TimeSpan(d, h, min, s, ms);
                return DateTime.Now.Add(ts);
        }
    }
}
