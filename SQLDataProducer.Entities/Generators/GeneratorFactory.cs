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
using System.Reflection;
using SQLDataProducer.Entities.Generators.DecimalGenerators;
using SQLDataProducer.Entities.Generators.XMLGenerators;
using SQLDataProducer.Entities.Generators.UniqueIdentifierGenerators;
using SQLDataProducer.Entities.Generators.StringGenerators;
using SQLDataProducer.Entities.Generators.BinaryGenerators;
using SQLDataProducer.Entities.Generators.DateTimeGenerators;
using SQLDataProducer.Entities.Generators.IntGenerators;
using SQLDataProducer.Entities.Generators.ForeignKeyGenerators;

namespace SQLDataProducer.Entities.Generators
{
    public class GeneratorFactory
    {

        public static ObservableCollection<AbstractValueGenerator> GetGeneratorsForColumn(ColumnEntity column)
        {
            ObservableCollection<AbstractValueGenerator> gens = GetAllGeneratorsForType(column.ColumnDataType);

            if (column.IsIdentity)
            {
                // TODO: Fix hardcoded INT generator, this is sort of safe since we are only checking the name but it is a smell.
                // TODO: sorting by using != is not intuitive
                gens = new ObservableCollection<AbstractValueGenerator>(gens.OrderBy(x => x.GeneratorName != IdentityIntGenerator.GENERATOR_NAME));
            }

            return gens;
        }

        public static List<AbstractValueGenerator> GetForeignKeyGenerators(IEnumerable<string> fkKeys)
        {
            return new List<AbstractValueGenerator>(GetGeneratorsOfBaseType<ForeignKeyGeneratorBase>(new List<string>(fkKeys)));
        }

        public static ObservableCollection<AbstractValueGenerator> GetAllGeneratorsForType(ColumnDataTypeDefinition dataType)
        {
            IEnumerable<AbstractValueGenerator> valueGenerators;

            switch (dataType.DBType)
            {
                case System.Data.SqlDbType.BigInt:
                case System.Data.SqlDbType.TinyInt:
                case System.Data.SqlDbType.SmallInt:
                case System.Data.SqlDbType.Bit:
                case System.Data.SqlDbType.Int:
                    valueGenerators = GetGeneratorsOfBaseType<IntegerGeneratorBase>(dataType);
                    break;
                case System.Data.SqlDbType.Date:
                case System.Data.SqlDbType.DateTime:
                case System.Data.SqlDbType.DateTime2:
                case System.Data.SqlDbType.SmallDateTime:
                case System.Data.SqlDbType.Time:
                case System.Data.SqlDbType.DateTimeOffset:
                    valueGenerators = GetGeneratorsOfBaseType<DateTimeGeneratorBase>(dataType);
                    break;
                
                case System.Data.SqlDbType.Decimal:
                case System.Data.SqlDbType.Float:
                case System.Data.SqlDbType.Real:
                case System.Data.SqlDbType.Money:
                case System.Data.SqlDbType.SmallMoney:
                    valueGenerators = GetGeneratorsOfBaseType<DecimalGeneratorBase>(dataType);
                    break;
                
                case System.Data.SqlDbType.Binary:
                case System.Data.SqlDbType.Image:
                case System.Data.SqlDbType.Timestamp:
                case System.Data.SqlDbType.VarBinary:
                    valueGenerators = GetGeneratorsOfBaseType<BinaryGeneratorBase>(dataType);
                    break;
                
                case System.Data.SqlDbType.NChar:
                case System.Data.SqlDbType.NText:
                case System.Data.SqlDbType.NVarChar:
                case System.Data.SqlDbType.Text:
                case System.Data.SqlDbType.VarChar:
                case System.Data.SqlDbType.Variant:
                case System.Data.SqlDbType.Char:
                    valueGenerators = GetGeneratorsOfBaseType<StringGeneratorBase>(dataType);
                    break;
                
                case System.Data.SqlDbType.UniqueIdentifier:
                    valueGenerators = GetGeneratorsOfBaseType<GuidGeneratorBase>(dataType);
                    break;

                case System.Data.SqlDbType.Xml:
                    valueGenerators = GetGeneratorsOfBaseType<XmlGeneratorBase>(dataType);
                    break;

                    // not supported
                case System.Data.SqlDbType.Structured:
                case System.Data.SqlDbType.Udt:
                    throw new NotSupportedException(dataType.ToString());
                
                default:
                    throw new NotSupportedException(dataType.ToString());
            }

            if (dataType.IsNullable)
            {
                // sort so that the null generator is the first in the list.
                return new ObservableCollection<AbstractValueGenerator>(valueGenerators.OrderBy( x => x.GeneratorName != NullValueIntGenerator.GENERATOR_NAME)) ;
            }
            else
                return new ObservableCollection<AbstractValueGenerator>(valueGenerators.Where(z => z.GeneratorName != NullValueIntGenerator.GENERATOR_NAME));
        }
        
        // based on
        //http://stackoverflow.com/questions/3353699/using-reflection-to-get-all-classes-of-certain-base-type-in-dll
        public static ObservableCollection<AbstractValueGenerator> GetGeneratorsOfBaseType<T>(ColumnDataTypeDefinition dataType) where T : AbstractValueGenerator
        {
            // find all types where basetype is T and constructor takes one ColumnDTDef as parameter.
            // type must exist in the assembly of the abstractValueGenerator
            var a = (from t in typeof(AbstractValueGenerator).Assembly.GetTypes()
                     where t.BaseType == (typeof(T)) && t.GetConstructor( new Type[] { typeof(ColumnDataTypeDefinition) }) != null
                     select CreateInstance(t, dataType)).ToList();

            return new ObservableCollection<AbstractValueGenerator>(a);
        }
        public static ObservableCollection<AbstractValueGenerator> GetGeneratorsOfBaseType<T>(List<string> foreignKeys) where T : AbstractValueGenerator
        {
            // find all types where basetype is T and constructor takes one ColumnDTDef as parameter.
            // type must exist in the assembly of the abstractValueGenerator
            var a = (from t in typeof(AbstractValueGenerator).Assembly.GetTypes()
                     where t.BaseType == (typeof(T)) && t.GetConstructor(new Type[] { typeof(List<string>) }) != null
                     select CreateInstance(t, foreignKeys)).ToList();

            return new ObservableCollection<AbstractValueGenerator>(a);
        }
        public static AbstractValueGenerator CreateInstance(Type t, ColumnDataTypeDefinition dataType)
        {
            return Activator.CreateInstance(t, dataType) as AbstractValueGenerator;
        }
        public static AbstractValueGenerator CreateInstance(Type t, List<string> foreignKeys)
        {
            return Activator.CreateInstance(t, foreignKeys) as AbstractValueGenerator;
        }
    }
}
