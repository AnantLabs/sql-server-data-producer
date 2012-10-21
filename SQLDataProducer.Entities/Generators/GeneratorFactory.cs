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
using System.Linq;
using System.Text.RegularExpressions;
using SQLDataProducer.Entities.DatabaseEntities;
using System;
using SQLDataProducer.Entities.Generators;
using Generators = SQLDataProducer.Entities.Generators;
using SQLDataProducer.Entities;

//

namespace SQLDataProducer.Entities.Generators
{
    public class GeneratorFactory
    {
        public static ObservableCollection<Generator> GetGeneratorsForDataType(ColumnDataTypeDefinition dbataTypeDef)
        {
            ObservableCollection<Generator> gens = GetDefaultGeneratorsForDataType(dbataTypeDef);

            // If the column is nullable then the default generator should be the NULL generator. We insert it at the top so that it will be picked up as the default.
            if (dbataTypeDef.IsNullable)
                gens.Insert(0, Generator.CreateNULLValueGenerator());
            
            return gens;
        }

        private static ObservableCollection<Generator> GetDefaultGeneratorsForDataType(ColumnDataTypeDefinition dbataTypeDef)
        {
            switch (dbataTypeDef.DBType)
            {
                case DBDataType.INT:
                    return Generators.Generator.GetGeneratorsForInt();
                case DBDataType.TINYINT:
                    return Generators.Generator.GetGeneratorsForTinyInt();
                case DBDataType.SMALLINT:
                    return Generators.Generator.GetGeneratorsForSmallInt();
                case DBDataType.BIGINT:
                    return Generators.Generator.GetGeneratorsForBigInt();
                case DBDataType.BIT:
                    return Generators.Generator.GetGeneratorsForBit();
                case DBDataType.VARCHAR:
                    return Generators.Generator.GetStringGenerators(dbataTypeDef.MaxLength);
                case DBDataType.DECIMAL:
                    return Generators.Generator.GetDecimalGenerators();
                case DBDataType.DATETIME:
                    return Generators.Generator.GetDateTimeGenerators();
                case DBDataType.UNIQUEIDENTIFIER:
                    return Generators.Generator.GetGUIDGenerators();
                case DBDataType.UNKNOWN:
                    return Generators.Generator.GetStringGenerators(1);
                default:
                    throw new NotImplementedException(dbataTypeDef.ToString());
            }
        }

        //private static ObservableCollection<Generator> GetDefaultGeneratorsForDataType2(ColumnDataTypeDefinition dbataTypeDef)
        //{
        //    System.Reflection.MemberInfo inf = typeof(Generator);

        //    object[] attributes =
        //       inf.GetCustomAttributes(
        //            typeof(GeneratorMetaDataAttribute), false);

        //    foreach (GeneratorMetaDataAttribute attribute in attributes)
        //    {
                
        //    }

        //    return null;
        //}

        public static System.Collections.Generic.IEnumerable<Generator> GetForeignKeyGenerators(ObservableCollection<string> fkKeys)
        {
            var l = new System.Collections.Generic.List<Generator>();
            l.Add(Generators.Generator.CreateRandomForeignKeyGenerator(fkKeys));
            l.Add(Generators.Generator.CreateSequentialForeignKeyGenerator(fkKeys));
            //l.Add(Generators.Generator.CreateSequentialForeignKeyGeneratorLazy(fkKeys));
            return l;
        }
    }
}
