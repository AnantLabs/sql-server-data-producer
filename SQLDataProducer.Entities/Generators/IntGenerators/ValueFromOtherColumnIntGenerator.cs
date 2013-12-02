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
    public class ValueFromOtherColumnIntGenerator : IntegerGeneratorBase
    {
        public static readonly string GENERATOR_NAME = "Value from other Column";

        public ValueFromOtherColumnIntGenerator(ColumnDataTypeDefinition datatype)
            : base(GENERATOR_NAME, datatype)
        {
            GeneratorParameters.Add(new GeneratorParameter("StartValue", 0, GeneratorParameterParser.IntegerParser));
            GeneratorParameters.Add(new GeneratorParameter("Step", 1, GeneratorParameterParser.IntegerParser));
        }

        protected override object InternalGenerateValue(long n, Collections.GeneratorParameterCollection paramas)
        {
            //public static Generator CreateValueFromOtherColumnGenerator_NewWay()
            //{
            //    GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            //    paramss.Add(new GeneratorParameter("Value From Column", null, GeneratorParameterParser.ObjectParser));

            //    Generator gen = new Generator();
            //    gen.GeneratorName = GENERATOR_NewWayToGetValueFromOtherColumn;
            //    gen._isTakingValueFromOtherColumn = true;
            //    gen.ValueGenerator = (n, p) =>
            //    {
            //        ColumnEntity col = p.GetValueOf<ColumnEntity>("Value From Column");
            //        if (col != null)
            //        {
            //            return col.ColumnIdentity;
            //        }
            //        throw new ArgumentNullException("Value From Column");
            //    };

            //    gen.GeneratorParameters = paramss;
            //    return gen;
            //}
            return n;
        }
    }
}
