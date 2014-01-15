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
        public static ColumnEntity CreateColumnEntity(string columnName, ColumnDataTypeDefinition columnDatatype, bool isIdentity, int ordinalPosition, bool isForeignKey, string constraintDefinition, ForeignKeyEntity foreignKeyEntity)
        {
            ColumnEntity c = new ColumnEntity(columnName, columnDatatype, isIdentity, ordinalPosition, isForeignKey, constraintDefinition, foreignKeyEntity);
            var gens = Generators.GeneratorFactory.GetGeneratorsForColumn(c);
            c.PossibleGenerators = gens;
            c.Generator = gens.First();
            
            return c;
        }


        public static ColumnEntity CreateColumnEntity(string columnName, ColumnDataTypeDefinition columnDatatype, bool isIdentity, int ordinalPosition, bool isForeignKey, string constraintDefinition, ForeignKeyEntity foreignKeyEntity, string generatorName)
        {
            ColumnEntity c = new ColumnEntity(columnName, columnDatatype, isIdentity, ordinalPosition, isForeignKey, constraintDefinition, foreignKeyEntity);
            var gens = Generators.GeneratorFactory.GetGeneratorsForColumn(c);
            c.PossibleGenerators = gens;
            c.Generator = gens.Where(g => g.GeneratorName == generatorName).First();

            return c;
        }

        public static ColumnEntity CreateColumnFromColumn(ColumnEntity originalColumn)
        {
            ColumnDataTypeDefinition dataType = new ColumnDataTypeDefinition(originalColumn.ColumnDataType.Raw, originalColumn.ColumnDataType.IsNullable);
            var constraints = originalColumn.Constraints;
            var foreignKeys = originalColumn.ForeignKey == null ? null : originalColumn.ForeignKey.Clone();


            var newColumn = CreateColumnEntity(originalColumn.ColumnName, dataType, originalColumn.IsIdentity, originalColumn.OrdinalPosition, originalColumn.IsForeignKey, constraints, foreignKeys);
            
            return newColumn;
        }

        /// <summary>
        /// Crateas a new table with the same columns as the original table. The default values will be used for all column parameters.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static TableEntity CreateTableFromTable(TableEntity table)
        {
            var newTable = new TableEntity(table.TableSchema, table.TableName);
            foreach (var column in table.Columns)
            {
                newTable.AddColumn(CreateColumnFromColumn(column));
            }
            return newTable;
        }
    }
}
