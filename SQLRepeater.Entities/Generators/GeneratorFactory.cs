using System.Collections.ObjectModel;
using System.Linq;
//

namespace SQLRepeater.Entities.Generators
{
    public class GeneratorFactory
    {
        public static ObservableCollection<GeneratorBase> GetGeneratorsForDataType(string dataType)
        {
            if (dataType.StartsWith("int"))
            {
                return Generators.IntGenerator.GetGeneratorsForInt();
            }
            else if (dataType.StartsWith("tinyint"))
            {
                return Generators.IntGenerator.GetGeneratorsForTinyInt();
            }
            else if (dataType.StartsWith("smallint"))
            {
                return Generators.IntGenerator.GetGeneratorsForSmallInt();
            }
            else if (dataType.StartsWith("bigint"))
            {
                return Generators.IntGenerator.GetGeneratorsForBigInt();
            }
            else if (dataType.StartsWith("bit"))
            {
                return Generators.IntGenerator.GetGeneratorsForBit();
            }
            else if (dataType.StartsWith("varchar")
                || dataType.StartsWith("nvarchar")
                || dataType.StartsWith("char")
                || dataType.StartsWith("nchar"))
            {
                return Generators.StringGenerator.GetGenerators();
            }
            else if (dataType.StartsWith("decimal")
                || dataType.StartsWith("float"))
            {
                return Generators.DecimalGenerator.GetGenerators();
            }
            else if (dataType.StartsWith("datetime"))
            {
                return Generators.DateTimeGenerator.GetGenerators();
            }
            else if (dataType.StartsWith("uniqueidentifier"))
            {
                return Generators.UniqueIdentifierGenerator.GetGenerators();
            }

            else
            {
                return Generators.StringGenerator.GetGenerators();
            }
        }
               
    }
}
