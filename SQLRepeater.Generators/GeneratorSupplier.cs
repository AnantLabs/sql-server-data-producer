using System.Collections.ObjectModel;
using System.Linq;
//using SQLRepeater.Entities.ValueGeneratorParameters;

namespace SQLRepeater.Generators
{
    public class Generatorsupplier
    {
        public static ObservableCollection<ValueCreatorDelegate> GetGeneratorsForDataType(string dataType)
        {
            switch (dataType)
            {
                case "int":
                case "bigint":
                    return Generators.IntGenerator.Generators;

                case "smallint":
                    return Generators.SmallIntGenerator.Generators;

                case "tinyint":
                    return Generators.TinyIntGenerator.Generators;

                case "decimal":
                case "float":
                    return Generators.DecimalGenerator.Generators;

                case "datetime":
                case "datetime2":
                    return Generators.DateTimeGenerator.Generators;

                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                    return Generators.StringGenerator.Generators;

                case "bit":
                    return Generators.BooleanGenerator.Generators;

                case "uniqueidentifier":
                    return Generators.UniqueIdentifierGenerator.Generators;

                default:
                    return Generators.StringGenerator.Generators;
            }
        }

        public static ValueCreatorDelegate GetDefaultGeneratorForDataType(string dataType)
        {
            switch (dataType)
            {
                case "int":
                case "bigint":
                    return Generators.IntGenerator.Generators.First();

                case "smallint":
                    return Generators.SmallIntGenerator.Generators.First();

                case "tinyint":
                    return Generators.TinyIntGenerator.Generators.First();

                case "decimal":
                case "float":
                    return Generators.DecimalGenerator.Generators.First();

                case "datetime":
                case "datetime2":
                    return Generators.DateTimeGenerator.Generators.First();

                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                    return Generators.StringGenerator.Generators.First();

                case "bit":
                    return Generators.BooleanGenerator.Generators.First();

                case "uniqueidentifier":
                    return Generators.UniqueIdentifierGenerator.Generators.First();

                default:
                    return Generators.StringGenerator.Generators.First();
            }
        }

    }
}
