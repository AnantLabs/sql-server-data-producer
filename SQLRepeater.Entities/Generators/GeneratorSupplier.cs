using System.Collections.ObjectModel;
using System.Linq;
//

namespace SQLRepeater.Entities.Generators
{
    public class Generatorsupplier
    {
        public static ObservableCollection<GeneratorBase> GetGeneratorsForDataType(string dataType)
        {
            switch (dataType)
            {
                case "int":
                case "bigint":
                    return Generators.IntGenerator.GetGenerators();

                case "smallint":
                    //return Generators.SmallIntGenerator.Generators;
                    return Generators.IntGenerator.GetGenerators();

                case "tinyint":
                    //return Generators.TinyIntGenerator.Generators;
                    return Generators.IntGenerator.GetGenerators();

                case "decimal":
                case "float":
                    //return Generators.DecimalGenerator.Generators;
                    return Generators.IntGenerator.GetGenerators();

                case "datetime":
                case "datetime2":
                    //return Generators.DateTimeGenerator.Generators;
                    return Generators.IntGenerator.GetGenerators();

                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                    //return Generators.StringGenerator.Generators;
                    return Generators.IntGenerator.GetGenerators();

                case "bit":
                    //return Generators.BooleanGenerator.Generators;
                    return Generators.IntGenerator.GetGenerators();

                case "uniqueidentifier":
                    //return Generators.UniqueIdentifierGenerator.Generators;
                    return Generators.IntGenerator.GetGenerators();

                default:
                    //return Generators.StringGenerator.Generators;
                    return Generators.IntGenerator.GetGenerators();
            }
        }

        //public static GeneratorBase GetDefaultGeneratorForDataType(string dataType)
        //{
        //    switch (dataType)
        //    {
        //        case "int":
        //        case "bigint":
        //            return Generators.IntGenerator.DefaultGenerator();

        //        case "smallint":
        //            //return Generators.SmallIntGenerator.Generators.First();
        //            return Generators.IntGenerator.DefaultGenerator();

        //        case "tinyint":
        //            //return Generators.TinyIntGenerator.Generators.First();
        //            return Generators.IntGenerator.DefaultGenerator();

        //        case "decimal":
        //        case "float":
        //            //return Generators.DecimalGenerator.Generators.First();
        //            return Generators.IntGenerator.DefaultGenerator();

        //        case "datetime":
        //        case "datetime2":
        //            //return Generators.DateTimeGenerator.Generators.First();
        //            return Generators.IntGenerator.DefaultGenerator();

        //        case "varchar":
        //        case "nvarchar":
        //        case "char":
        //        case "nchar":
        //            //return Generators.StringGenerator.Generators.First();
        //            return Generators.IntGenerator.DefaultGenerator();

        //        case "bit":
        //            //return Generators.BooleanGenerator.Generators.First();
        //            return Generators.IntGenerator.DefaultGenerator();


        //        case "uniqueidentifier":
        //            //return Generators.UniqueIdentifierGenerator.Generators.First();
        //            return Generators.IntGenerator.DefaultGenerator();

        //        default:
        //            //return Generators.StringGenerator.Generators.First();
        //            return Generators.IntGenerator.DefaultGenerator();
        //    }
        //}

    }
}
