using System.Collections.ObjectModel;
using SQLRepeater.Entities.ValueGeneratorParameters;

namespace SQLRepeater.Generators
{
    public class Generatorsupplier
    {
        public static ObservableCollection<ValueCreatorDelegate> GetGeneratorsForDataType(string dataType)
        {
            switch (dataType)
            {
                case "int":
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

        public static object GetGeneratorParameterForDataType(string dataType)
        {
            switch (dataType)
            {
                case "int":
                case "smallint":
                case "tinyint":                
                    return  new IntParameter() ;

                case "bit":
                    return new BooleanParameter();

                case "decimal":
                case "float":
                    return new DecimalParameter();

                case "datetime":
                case "datetime2":
                    return new DateTimeParameter();

                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                
                    return new StringParameter();

                default:
                    return new StringParameter();
            }
        }
    }
}
