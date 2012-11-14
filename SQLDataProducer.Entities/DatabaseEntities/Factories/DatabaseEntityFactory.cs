using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Entities.DatabaseEntities.Factories
{
    public static class DatabaseEntityFactory
    {
        public static ColumnEntity CreateColumnEntity(string columnName, ColumnDataTypeDefinition columnDatatype, bool isIdentity, int ordinalPosition, bool isForeignKey, ForeignKeyEntity foreignKeyEntity)
        {
            var gens = Generators.GeneratorFactory.GetGeneratorsForDataType(columnDatatype);
            ColumnEntity c = new ColumnEntity(columnName, columnDatatype, isIdentity, ordinalPosition, isForeignKey, foreignKeyEntity, gens, gens.First());
            
            return c;
        }

        public static ColumnEntity CreateColumnEntity(string columnName, ColumnDataTypeDefinition columnDatatype, bool isIdentity, int ordinalPosition, bool isForeignKey, ForeignKeyEntity foreignKeyEntity, string generatorName)
        {
            var gens = Generators.GeneratorFactory.GetGeneratorsForDataType(columnDatatype);
            var selectedGenerator = gens.Where(g => g.GeneratorName == generatorName).First();
            ColumnEntity c = new ColumnEntity(columnName, columnDatatype, isIdentity, ordinalPosition, isForeignKey, foreignKeyEntity, gens, selectedGenerator);

            return c;
        }
    }
}
