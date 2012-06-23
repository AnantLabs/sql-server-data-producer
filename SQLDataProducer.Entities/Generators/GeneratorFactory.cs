using System.Collections.ObjectModel;
using System.Linq;
//

namespace SQLDataProducer.Entities.Generators
{
    public class GeneratorFactory
    {
        public static ObservableCollection<Generator> GetGeneratorsForDataType(string dataType)
        {
            if (dataType.StartsWith("int"))
            {
                return Generators.Generator.GetGeneratorsForInt();
            }
            else if (dataType.StartsWith("tinyint"))
            {
                return Generators.Generator.GetGeneratorsForTinyInt();
            }
            else if (dataType.StartsWith("smallint"))
            {
                return Generators.Generator.GetGeneratorsForSmallInt();
            }
            else if (dataType.StartsWith("bigint"))
            {
                return Generators.Generator.GetGeneratorsForBigInt();
            }
            else if (dataType.StartsWith("bit"))
            {
                return Generators.Generator.GetGeneratorsForBit();
            }
            else if (dataType.StartsWith("varchar")
                || dataType.StartsWith("nvarchar")
                || dataType.StartsWith("char")
                || dataType.StartsWith("nchar"))
            {
                return Generators.Generator.GetStringGenerators();
            }
            else if (dataType.StartsWith("decimal")
                || dataType.StartsWith("float"))
            {
                return Generators.Generator.GetDecimalGenerators();
            }
            else if (dataType.StartsWith("datetime"))
            {
                return Generators.Generator.GetDateTimeGenerators();
            }
            else if (dataType.StartsWith("uniqueidentifier"))
            {
                return Generators.Generator.GetGUIDGenerators();
            }

            else
            {
                return Generators.Generator.GetStringGenerators();
            }

        }

        public static System.Collections.Generic.IEnumerable<Generator> GetForeignKeyGenerators(ObservableCollection<int> fkKeys)
        {
            var l = new System.Collections.Generic.List<Generator>();
            l.Add(Generators.Generator.CreateRandomForeignKeyGenerator(fkKeys));
            l.Add(Generators.Generator.CreateSequentialForeignKeyGenerator(fkKeys));
            //l.Add(Generators.Generator.CreateSequentialForeignKeyGeneratorLazy(fkKeys));
            return l;
        }
    }
}
