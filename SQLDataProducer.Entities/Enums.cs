using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
