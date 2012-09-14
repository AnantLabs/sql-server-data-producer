using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Entities.Generators
{
    [AttributeUsage(AttributeTargets.Method)]
    public class GeneratorMetaDataAttribute : Attribute
    {
        public enum GeneratorType
        {
            General,
            Integer,
            String,
            UniqueIdentifier,
            DateTime,
            Decimal
        }

        public GeneratorMetaDataAttribute(GeneratorType type)
        {

        }
    }
}
