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
        NotEqualTo
    }

    public enum ExecutionTypes
    {
        DurationBased,
        ExecutionCountBased
    }

    public static class EnumExtensions
    {
        public static string ToCompareString(this ExecutionConditions cond)
        {
            switch (cond)
            {
                case ExecutionConditions.None:
                    return string.Empty;
                case ExecutionConditions.LessThan:
                    return "<";
                case ExecutionConditions.LessOrEqualTo:
                    return "<=";
                case ExecutionConditions.EqualTo:
                    return "=";
                case ExecutionConditions.EqualOrGreaterThan:
                    return ">=";
                case ExecutionConditions.GreaterThan:
                    return ">";
                case ExecutionConditions.NotEqualTo:
                    return "<>";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
