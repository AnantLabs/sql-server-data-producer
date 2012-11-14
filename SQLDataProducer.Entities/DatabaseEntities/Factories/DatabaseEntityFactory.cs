using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DatabaseEntities.Collections;

namespace SQLDataProducer.Entities.DatabaseEntities.Factories
{
    public static class DatabaseEntityFactory
    {
        public static ColumnEntity CreateColumnEntity(string columnName, ColumnDataTypeDefinition columnDatatype, bool isIdentity, int ordinalPosition, bool isForeignKey, ForeignKeyEntity foreignKeyEntity)
        {
            
            ColumnEntity c = new ColumnEntity(columnName, columnDatatype, isIdentity, ordinalPosition, isForeignKey, foreignKeyEntity);
            var gens = Generators.GeneratorFactory.GetGeneratorsForColumn(c);
            c.PossibleGenerators = gens;
            c.Generator = gens.First();
            
            return c;
        }

        public static ColumnEntity CreateColumnEntity(string columnName, ColumnDataTypeDefinition columnDatatype, bool isIdentity, int ordinalPosition, bool isForeignKey, ForeignKeyEntity foreignKeyEntity, string generatorName)
        {
            ColumnEntity c = new ColumnEntity(columnName, columnDatatype, isIdentity, ordinalPosition, isForeignKey, foreignKeyEntity);
            var gens = Generators.GeneratorFactory.GetGeneratorsForColumn(c);
            c.PossibleGenerators = gens;
            c.Generator = gens.Where(g => g.GeneratorName == generatorName).First();

            return c;
        }

        internal static ColumnEntity CloneColumn(ColumnEntity c)
        {
            var col = new ColumnEntity(c.ColumnName, c.ColumnDataType, c.IsIdentity, c.OrdinalPosition, c.IsForeignKey, c.ForeignKey.Clone());
            col.PossibleGenerators = c.PossibleGenerators.Clone();
            col.Generator = col.PossibleGenerators.Where(x => x.GeneratorName == c.Generator.GeneratorName).FirstOrDefault();
            return col;
        }
    }
}
