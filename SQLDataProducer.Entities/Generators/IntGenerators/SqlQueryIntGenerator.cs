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
    public class SqlQueryIntGenerator : IntegerGeneratorBase
    {
        public static readonly string GENERATOR_NAME = "Custom SQL Query";

        public SqlQueryIntGenerator(ColumnDataTypeDefinition datatype)
            : base(GENERATOR_NAME, datatype)
        {
            GeneratorParameters.Add(new GeneratorParameter("StartValue", 0, GeneratorParameterParser.IntegerParser));
            GeneratorParameters.Add(new GeneratorParameter("Step", 1, GeneratorParameterParser.IntegerParser));
        }

        protected override object InternalGenerateValue(long n, Collections.GeneratorParameterCollection paramas)
        {
        //     [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.General)]
        //protected static Generator CreateQueryGenerator()
        //{
        //    GeneratorParameterCollection paramss = new GeneratorParameterCollection();

        //    paramss.Add(new GeneratorParameter("Query", "select ...", GeneratorParameterParser.StringParser));

        //    Generator gen = new Generator(GENERATOR_CustomSQLQuery, (n, p) =>
        //    {
        //        string value = p.GetValueOf<String>("Query");

        //        return string.Format("({0})", value);
        //    }
        //        , paramss, true);
        //    return gen;
        //}
            return n;
        }
    }
}
