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

using System;

namespace SQLDataProducer.Entities
{
    public enum ExecutionConditions
    {
        None,
        LessThan,
        LessOrEqualTo,
        EqualTo,
        EqualOrGreaterThan,
        GreaterThan,
        NotEqualTo,
        EveryOtherX
    }

    public enum ExecutionTypes
    {
        DurationBased,
        ExecutionCountBased
    }

    public enum NumberGeneratorMethods
    {
        NewNForEachExecution,
        NewNForEachRow,
        ConstantN
    }

    public enum DBDataType
    {
        INT,
        TINYINT,
        SMALLINT,
        BIGINT,
        BIT,
        VARCHAR,
        //NVARCHAR,
        //CHAR,
        //NCHAR,
        DECIMAL,
        //FLOAT,
        DATETIME,
        UNIQUEIDENTIFIER,
        UNKNOWN
    }

    public static class EnumExtensions
    {
        public static string ToCompareString(this ExecutionConditions cond, int conditionValue)
        {
            switch (cond)
            {
                case ExecutionConditions.None:
                    return string.Empty;
                case ExecutionConditions.LessThan:
                    return string.Format("< {0}",  conditionValue);
                case ExecutionConditions.LessOrEqualTo:
                    return string.Format("<= {0}",  conditionValue);
                case ExecutionConditions.EqualTo:
                    return string.Format("= {0}",  conditionValue);
                case ExecutionConditions.EqualOrGreaterThan:
                    return string.Format(">= {0}",  conditionValue);
                case ExecutionConditions.GreaterThan:
                    return string.Format("> {0}",  conditionValue);
                case ExecutionConditions.NotEqualTo:
                    return string.Format("<> {0}",  conditionValue);
                case ExecutionConditions.EveryOtherX:
                    return string.Format("% {0} = 0",  conditionValue);
                default:
                    throw new NotImplementedException();
            }
        }

        public static DBDataType ToDBDataType(this string dataType)
        {
            if (dataType.StartsWith("int"))
            {
                return DBDataType.INT;
            }
            else if (dataType.StartsWith("tinyint"))
            {
                return DBDataType.TINYINT;
            }
            else if (dataType.StartsWith("smallint"))
            {
                return DBDataType.SMALLINT;
            }
            else if (dataType.StartsWith("bigint"))
            {
                return DBDataType.BIGINT;
            }
            else if (dataType.StartsWith("bit"))
            {
                return DBDataType.BIT;
            }
            else if (dataType.StartsWith("varchar")
                || dataType.StartsWith("nvarchar")
                || dataType.StartsWith("char")
                || dataType.StartsWith("nchar"))
            {

                return DBDataType.VARCHAR;
            }
            else if (dataType.StartsWith("decimal")
                || dataType.StartsWith("float"))
            {
                return DBDataType.DECIMAL;
            }
            else if (dataType.StartsWith("datetime"))
            {
                return DBDataType.DATETIME;
            }
            else if (dataType.StartsWith("uniqueidentifier"))
            {
                return DBDataType.UNIQUEIDENTIFIER;
            }
            else
            {
                return DBDataType.UNKNOWN;
            }
        }
    }
}
