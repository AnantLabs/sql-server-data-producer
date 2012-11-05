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
                case System.Data.DbType.Boolean:
                    return Generators.Generator.GetGeneratorsForBit();

                case System.Data.DbType.Byte:
                case System.Data.DbType.SByte:
                    return Generators.Generator.GetGeneratorsForTinyInt();
                
                case System.Data.DbType.Date:
                case System.Data.DbType.DateTime:
                case System.Data.DbType.Time:
                case System.Data.DbType.DateTime2:
                case System.Data.DbType.DateTimeOffset:
                    return Generators.Generator.GetDateTimeGenerators();    
                
                case System.Data.DbType.Decimal:
                case System.Data.DbType.Double:
                case System.Data.DbType.Single:
                case System.Data.DbType.Currency:
                case System.Data.DbType.VarNumeric:
                    return Generators.Generator.GetDecimalGenerators();
                    
                case System.Data.DbType.Guid:
                    return Generators.Generator.GetGUIDGenerators();

                case System.Data.DbType.Int16:
                    return Generators.Generator.GetGeneratorsForSmallInt();

                case System.Data.DbType.Int32:
                case System.Data.DbType.UInt16:
                case System.Data.DbType.UInt32:
                case System.Data.DbType.UInt64:
                    return Generators.Generator.GetGeneratorsForInt();
                    
                case System.Data.DbType.Int64:
                    return Generators.Generator.GetGeneratorsForBigInt();
                
                case System.Data.DbType.String:
                case System.Data.DbType.StringFixedLength:
                case System.Data.DbType.AnsiStringFixedLength:
                case System.Data.DbType.AnsiString:
                    return Generators.Generator.GetStringGenerators(dbataTypeDef.MaxLength);
                
                    
                case System.Data.DbType.Xml:
                    return Generators.Generator.GetXMLGenerators();

                case System.Data.DbType.Binary:
                case System.Data.DbType.Object:
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
