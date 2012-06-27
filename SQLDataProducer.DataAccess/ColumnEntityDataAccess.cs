using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDataProducer.Entities;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using SQLDataProducer.DatabaseEntities.Entities;
using SQLDataProducer.Entities.Generators;
using SQLDataProducer.EntityQueryGenerator;
using SQLDataProducer.Entities.DatabaseEntities.Collections;

namespace SQLDataProducer.DataAccess
{
    public class ColumnEntityDataAccess : DataAccessBase
    {
        public ColumnEntityDataAccess(string connectionString)
            : base(connectionString)
        { }

        /// <summary>
        /// Function to generate one ColumnEntity from a sqldatareader
        /// </summary>
        private Func<SqlDataReader, ColumnEntity> CreateColumnEntity = reader =>
        {
            ObservableCollection<Generator> possibleGenerators = GeneratorFactory.GetGeneratorsForDataType(reader.GetString(reader.GetOrdinal("DataType")));
            Generator defaultGenerator = possibleGenerators.FirstOrDefault();

            return new ColumnEntity(reader.GetString(reader.GetOrdinal("ColumnName")), reader.GetString(reader.GetOrdinal("DataType")), (bool)reader["IsIdentity"], reader.GetInt32(reader.GetOrdinal("OrdinalPosition")), (bool)reader["IsForeignKey"], new ForeignKeyEntity
                {
                    ReferencingTable = new TableEntity(
                        reader.GetStringOrEmpty("ReferencedTableSchema"),
                        reader.GetStringOrEmpty("ReferencedTable")),
                    ReferencingColumn = reader.GetStringOrEmpty("ReferencedColumn")
                }, possibleGenerators, defaultGenerator);
        };


        readonly string GET_COLUMNS_FOR_TABLE_QUERY = @"
select 
    name As ColumnName, 
    CASE TYPE_NAME(cols.system_type_id)
        WHEN 'varchar' THEN TYPE_NAME(cols.system_type_id) + '(' + case cols.max_length when -1 then 'max' else CAST(cols.max_length AS VARCHAR(100)) end + ')'
        WHEN 'nvarchar' THEN TYPE_NAME(cols.system_type_id) + '(' + case cols.max_length when -1 then 'max' else CAST(cols.max_length AS VARCHAR(100)) end + ')'
        WHEN 'char' THEN TYPE_NAME(cols.system_type_id) + '(' + CAST(cols.max_length AS VARCHAR(100)) + ')'
        WHEN 'nchar' THEN TYPE_NAME(cols.system_type_id) + '(' + CAST(cols.max_length AS VARCHAR(100)) + ')'
        WHEN 'decimal' THEN TYPE_NAME(cols.system_type_id) + '(' + CAST(cols.precision AS VARCHAR(100)) + ', ' + CAST(cols.scale AS VARCHAR(100)) +')'
        WHEN 'varbinary' THEN TYPE_NAME(cols.system_type_id) + '(' + case cols.max_length when -1 then 'max' else CAST(cols.max_length AS VARCHAR(100)) end + ')'
        WHEN 'datetime2' THEN TYPE_NAME(cols.system_type_id) + '(' + CAST(cols.scale AS VARCHAR(100)) + ')'
        ELSE TYPE_NAME(cols.system_type_id)
    END as DataType
    , column_id as OrdinalPosition
    , is_identity as IsIdentity 
	, IsForeignKey = cast(case when foreignInfo.ReferencedTable is null then 0 else 1 end as bit)
	, ReferencedTableSchema = foreignInfo.ReferencedTableSchema
	, ReferencedTable = foreignInfo.ReferencedTable
	, ReferencedColumn = foreignInfo.ReferencedColumn

from 
    sys.columns cols

outer apply(
	select 
		schema_name(referencedTable.schema_id)
		,  referencedTable.name as ReferencedTable
		, crk.name ReferencedColumn
	FROM 
		sys.tables AS tbl
	LEFT JOIN 
		sys.foreign_keys AS fkeys
		ON fkeys.parent_object_id = tbl.object_id
	LEFT JOIN 
		sys.foreign_key_columns AS fkcols
		ON fkcols.constraint_object_id = fkeys.object_id

	LEFT JOIN 
		sys.columns AS referencedCols
		ON fkcols.parent_column_id = referencedCols.column_id
		AND fkcols.parent_object_id = referencedCols.object_id

	left join 
		sys.tables as referencedTable
		on referencedTable.object_id = fkeys.referenced_object_id
	LEFT JOIN 
		sys.columns AS crk
		ON fkcols.referenced_column_id = crk.column_id
		AND fkcols.referenced_object_id = crk.object_id

		where 
			referencedCols.column_id = cols.column_id
			and cols.object_id = tbl.object_id

) foreignInfo( ReferencedTableSchema,ReferencedTable, ReferencedColumn)

where object_id=object_id('{1}.{0}')  and cols.is_computed = 0";


        /// <summary>
        /// Begin get all columns for a table, does not block the caller. Provided callback method will be called when the execution is done.
        /// </summary>
        /// <param name="table">the table to get all columns for</param>
        /// <param name="callback">the callback method that will be called once the execution is done</param>
        public void BeginGetAllColumnsForTable(TableEntity table, Action<ColumnEntityCollection> callback)
        {
            BeginGetMany(
                string.Format(GET_COLUMNS_FOR_TABLE_QUERY
                        , table.TableName
                        , table.TableSchema)
                , CreateColumnEntity
                , cols =>
                {
                    foreach (var item in cols.Where(x => x.IsForeignKey))
                    {
                        GetForeignKeyGeneratorsForColumn(item);
                    }
                    callback((ColumnEntityCollection)cols);
                });
        }

        /// <summary>
        /// Get all columns for the provided table.
        /// </summary>
        /// <param name="table">the table to get all columns for</param>
        /// <returns></returns>
        public ColumnEntityCollection GetAllColumnsForTable(TableEntity table)
        {
            ColumnEntityCollection cols = new ColumnEntityCollection(GetMany(
                            string.Format(GET_COLUMNS_FOR_TABLE_QUERY
                                    , table.TableName
                                    , table.TableSchema)
                            , CreateColumnEntity));

            foreach (var item in cols.Where(x => x.IsForeignKey))
            {
                GetForeignKeyGeneratorsForColumn(item);
            }

            return cols;


        }

        private void GetForeignKeyGeneratorsForColumn(ColumnEntity item)
        {
            item.ForeignKey.Keys = ForeignKeyManager.Instance.GetPrimaryKeysForTable(this._connectionString, item.ForeignKey.ReferencingTable, item.ForeignKey.ReferencingColumn);
            
            // If we found any keys in the foreign table, add the generators.
            // TODO: When the lazy implementation is done, this need to be adjusted
            if (item.ForeignKey.Keys.Count > 0)
            {
                IEnumerable<Generator> fkGenerators = GeneratorFactory.GetForeignKeyGenerators(item.ForeignKey.Keys);
                foreach (var fkgen in fkGenerators)
                {
                    item.PossibleGenerators.Add(fkgen);
                }

                item.Generator = fkGenerators.First();
            }
            
        }
    }
}
