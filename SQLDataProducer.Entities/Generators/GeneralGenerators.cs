using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDataProducer.Entities.Generators.Collections;

namespace SQLDataProducer.Entities.Generators
{
    public partial class Generator 
    {
        public static readonly string GENERATOR_CustomSQLQuery = "Custom SQL Query";
        public static readonly string GENERATOR_NULLValue = "NULL value";

        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.General)]
        protected static Generator CreateQueryGenerator()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            paramss.Add(new GeneratorParameter("Query", "select ..."));

            Generator gen = new Generator(GENERATOR_CustomSQLQuery, (n, p) =>
            {
                string value = GetParameterByName(p, "Query").ToString();

                return string.Format("({0})", value);
            }
                , paramss);
            return gen;
        }

        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.General)]
        public static Generator CreateNULLValueGenerator()
        {
            Generator gen = new Generator(GENERATOR_NULLValue, (n, p) =>
            {
                return DBNull.Value; // "NULL";
            }
                , null);
            return gen;
        }
    }
}
