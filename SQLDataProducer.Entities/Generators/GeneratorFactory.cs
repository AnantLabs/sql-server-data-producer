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

using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using SQLDataProducer.Entities.DatabaseEntities;
using System;
using SQLDataProducer.Entities.Generators;
using Generators = SQLDataProducer.Entities.Generators;
using SQLDataProducer.Entities;
using System.Collections.Generic;

namespace SQLDataProducer.Entities.Generators
{
    public class GeneratorFactory
    {

        public static ObservableCollection<Generator> GetGeneratorsForColumn(ColumnEntity column)
        {
            ObservableCollection<Generator> gens = GetGeneratorsForDataType(column.ColumnDataType);

            if (column.IsIdentity)
            {
                foreach (var g in Generators.Generator.GetGeneratorsForIdentity())
                {
                    gens.Insert(0, g);
                }
            }

            return gens;
        }

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
                case System.Data.SqlDbType.BigInt:
                    return Generators.Generator.GetGeneratorsForBigInt();
                
                case System.Data.SqlDbType.Bit:
                    return Generators.Generator.GetGeneratorsForBit();
                case System.Data.SqlDbType.Char:
                    return Generators.Generator.GetStringGenerators(dbataTypeDef.MaxLength);
                
                case System.Data.SqlDbType.Date:
                case System.Data.SqlDbType.DateTime:
                case System.Data.SqlDbType.DateTime2:
                case System.Data.SqlDbType.SmallDateTime:
                case System.Data.SqlDbType.Time:
                case System.Data.SqlDbType.DateTimeOffset:
                    return Generators.Generator.GetDateTimeGenerators();

                case System.Data.SqlDbType.Decimal:
                    return Generators.Generator.GetDecimalGenerators(100000000000000000);
                case System.Data.SqlDbType.Float:
                    return Generators.Generator.GetDecimalGenerators(100000000000000000);
                case System.Data.SqlDbType.Real:
                    return Generators.Generator.GetDecimalGenerators(100000000000000000);
                case System.Data.SqlDbType.Money:
                    return Generators.Generator.GetDecimalGenerators(922337203685470);
                case System.Data.SqlDbType.SmallMoney:
                    return Generators.Generator.GetDecimalGenerators(214740);
                    

                case System.Data.SqlDbType.Binary:
                case System.Data.SqlDbType.Image:
                case System.Data.SqlDbType.Timestamp:
                case System.Data.SqlDbType.VarBinary:
                    return Generators.Generator.GetBinaryGenerators(1);

                case System.Data.SqlDbType.Int:
                    return Generators.Generator.GetGeneratorsForInt();
                    
                case System.Data.SqlDbType.NChar:
                case System.Data.SqlDbType.NText:
                case System.Data.SqlDbType.NVarChar:
                case System.Data.SqlDbType.Text:
                case System.Data.SqlDbType.VarChar:
                    return Generators.Generator.GetStringGenerators(dbataTypeDef.MaxLength);
                
                case System.Data.SqlDbType.SmallInt:
                    return Generators.Generator.GetGeneratorsForSmallInt();
                
                case System.Data.SqlDbType.Structured:
                    break;
                case System.Data.SqlDbType.TinyInt:
                    return Generators.Generator.GetGeneratorsForTinyInt();
                case System.Data.SqlDbType.Udt:
                    break;
                case System.Data.SqlDbType.UniqueIdentifier:
                    return Generators.Generator.GetGUIDGenerators();

                case System.Data.SqlDbType.Variant:
                    return Generators.Generator.GetGeneratorsForInt();

                case System.Data.SqlDbType.Xml:
                    return Generators.Generator.GetXMLGenerators();

                default:
                    break;
            }

            return new ObservableCollection<Generator>();

        }

        public static List<Generator> GetForeignKeyGenerators(ObservableCollection<string> fkKeys)
        {
            var l = new System.Collections.Generic.List<Generator>();
            l.Add(Generators.Generator.CreateRandomForeignKeyGenerator(fkKeys));
            l.Add(Generators.Generator.CreateSequentialForeignKeyGenerator(fkKeys));
            //l.Add(Generators.Generator.CreateSequentialForeignKeyGeneratorLazy(fkKeys));
            return l;
        }

        public static ObservableCollection<Generator> GetAllGeneratorsForType(System.Data.SqlDbType sqlDbType)
        {
            return null;
        }
    }
}
