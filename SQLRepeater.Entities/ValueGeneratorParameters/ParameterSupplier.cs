using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.Entities.ValueGeneratorParameters
{
    public class ParameterSupplier
    {
        public static object GetGeneratorParameterForDataType(string dataType)
        {
            switch (dataType)
            {
                case "int":
                case "smallint":
                case "tinyint":
                    return new IntParameter();

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
