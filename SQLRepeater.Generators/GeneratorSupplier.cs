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
                    return Generators.IntGenerators.Generators;

                case "smallint":
                    return Generators.SmallIntGenerators.Generators;

                case "tinyint":
                    return Generators.TinyIntGenerators.Generators;

                case "decimal":
                case "float":
                    return Generators.DecimalGenerators.Generators;

                case "datetime":
                case "datetime2":
                    return Generators.DateTimeGenerators.Generators;

                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                    return Generators.StringGenerators.Generators;

                case "bit":
                    return Generators.BooleanGenerators.Generators;

                default:
                    return Generators.StringGenerators.Generators;
            }
        }

        public static ValueCreatorDelegate GetDefaultGeneratorForDataType(string dataType)
        {
            switch (dataType)
            {
                case "int":
                case "bigint":
                    return Generators.IntGenerators.Generators.First();

                case "smallint":
                    return Generators.SmallIntGenerators.Generators.First();

                case "tinyint":
                    return Generators.TinyIntGenerators.Generators.First();

                case "decimal":
                case "float":
                    return Generators.DecimalGenerators.Generators.First();

                case "datetime":
                case "datetime2":
                    return Generators.DateTimeGenerators.Generators.First();

                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                    return Generators.StringGenerators.Generators.First();

                case "bit":
                    return Generators.BooleanGenerators.Generators.First();

                default:
                    return Generators.StringGenerators.Generators.First();
            }
        }

    }
}
