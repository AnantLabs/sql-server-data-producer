using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLRepeater.Entities;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using SQLRepeater.DatabaseEntities.Entities;
using SQLRepeater.Entities.Generators;

namespace SQLRepeater.DataAccess
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
            ObservableCollection<GeneratorBase> generators = GeneratorFactory.GetGeneratorsForDataType(reader.GetString(reader.GetOrdinal("DataType")));
            return new ColumnEntity
            (
                reader.GetString(reader.GetOrdinal("ColumnName")),
                reader.GetString(reader.GetOrdinal("DataType")),
                (bool)reader["IsIdentity"],
                reader.GetInt32(reader.GetOrdinal("OrdinalPosition")),
                (bool)reader["IsForeignKey"],
                reader.GetStringOrEmpty("ReferencedTable"),
                reader.GetStringOrEmpty("ReferencedColumn"),
                generators.FirstOrDefault(),
                generators
            );
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
	, ReferencedTable = foreignInfo.ReferencedTable
	, ReferencedColumn = foreignInfo.ReferencedColumn

from 
    sys.columns cols

outer apply(
	select 
		  referencedTable.name as ReferencedTable
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

) foreignInfo( ReferencedTable, ReferencedColumn)

where object_id=object_id('{1}.{0}')";


        /// <summary>
        /// Begin get all columns for a table, does not block the caller. Provided callback method will be called when the execution is done.
        /// </summary>
        /// <param name="table">the table to get all columns for</param>
        /// <param name="callback">the callback method that will be called once the execution is done</param>
        public void BeginGetAllColumnsForTable(TableEntity table, Action<ObservableCollection<ColumnEntity>> callback)
        {
            BeginGetMany(
                string.Format(GET_COLUMNS_FOR_TABLE_QUERY
                        , table.TableName
                        , table.TableSchema)
                , CreateColumnEntity
                , callback);
        }

        /// <summary>
        /// Get all columns for the provided table.
        /// </summary>
        /// <param name="table">the table to get all columns for</param>
        /// <returns></returns>
        public ObservableCollection<ColumnEntity> GetAllColumnsForTable(TableEntity table)
        {
            return GetMany(
                string.Format(GET_COLUMNS_FOR_TABLE_QUERY
                        , table.TableName
                        , table.TableSchema)
                , CreateColumnEntity );
        }
        //public ObservableCollection<ColumnEntity> GetAllColumnsForTable(string schemaName, string tableName)
        //{
        //    return GetMany(
        //        string.Format(GET_COLUMNS_FOR_TABLE_QUERY
        //                , tableName
        //                , schemaName)
        //        , CreateColumnEntity);
        //}


    }
}
