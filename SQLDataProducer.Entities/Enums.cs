// Copyright 2012-2014 Peter Henell

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

    //public enum DBDataType
    //{
    //    INT,
    //    TINYINT,
    //    SMALLINT,
    //    BIGINT,
    //    BIT,
    //    VARCHAR,
    //    //NVARCHAR,
    //    //CHAR,
    //    //NCHAR,
    //    DECIMAL,
    //    //FLOAT,
    //    DATETIME,
    //    UNIQUEIDENTIFIER,
    //    UNKNOWN
    //}

    //public static class EnumExtensions
    //{
    //    public static string ToCompareString(this ExecutionConditions cond, int conditionValue)
    //    {
    //        switch (cond)
    //        {
    //            case ExecutionConditions.None:
    //                return string.Empty;
    //            case ExecutionConditions.LessThan:
    //                return string.Format("< {0}", conditionValue);
    //            case ExecutionConditions.LessOrEqualTo:
    //                return string.Format("<= {0}", conditionValue);
    //            case ExecutionConditions.EqualTo:
    //                return string.Format("= {0}", conditionValue);
    //            case ExecutionConditions.EqualOrGreaterThan:
    //                return string.Format(">= {0}", conditionValue);
    //            case ExecutionConditions.GreaterThan:
    //                return string.Format("> {0}", conditionValue);
    //            case ExecutionConditions.NotEqualTo:
    //                return string.Format("<> {0}", conditionValue);
    //            case ExecutionConditions.EveryOtherX:
    //                return string.Format("% {0} = 0", conditionValue);
    //            default:
    //                throw new NotImplementedException();
    //        }
    //    }

    //}

    //public class EnumConverter
    //{
    //    public static string ToDescriptionString(NumberGeneratorMethods method)
    //    {
    //        switch (method)
    //        {
    //            case NumberGeneratorMethods.NewNForEachExecution:
    //                return "New N For Each Execution";
    //            case NumberGeneratorMethods.NewNForEachRow:
    //                return "New N For Each Inserted Row";
    //            case NumberGeneratorMethods.ConstantN:
    //                return "Constant N";
    //            default:
    //                throw new NotImplementedException(method.ToString());
    //        }
    //    }
        
    //    public static string ToDescriptionString(ExecutionTypes execType)
    //    {
    //        switch (execType)
    //        {
    //            case ExecutionTypes.DurationBased:
    //                return "DurationBased Execution";
    //            case ExecutionTypes.ExecutionCountBased:
    //                return "Execution Count Based Execution";
    //            default:
    //                throw new NotImplementedException(execType.ToString());
    //        }
    //    }
    //}
}
