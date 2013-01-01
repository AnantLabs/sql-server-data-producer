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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SQLDataProducer.Entities.Generators.Collections;

namespace SQLDataProducer.Entities.Generators
{
    public partial class Generator
    {
        public static readonly string GENERATOR_BinaryStaticGenerator = "Static Binary Value";

        public static ObservableCollection<Generator> GetBinaryGenerators(int length)
        {
            ObservableCollection<Generator> gens = new ObservableCollection<Generator>();
            gens.Add(Create_Binary_StaticGenerator(length));

            return gens;
        }

        private static Generator Create_Binary_StaticGenerator(int length)
        {
            //GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            // paramss.Add(new GeneratorParameter("Number", 0));

            Generator gen = new Generator(GENERATOR_BinaryStaticGenerator, (n, p) =>
            {
                return 0x10;
            }
                , null);
            return gen;
        }

    }
}
