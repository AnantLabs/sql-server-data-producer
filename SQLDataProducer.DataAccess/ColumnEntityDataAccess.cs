// Copyright 2012 Peter Henell

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
using System.Collections.Generic;
using System.Linq;
using SQLDataProducer.Entities;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.Generators;
using SQLDataProducer.EntityQueryGenerator;
using SQLDataProducer.Entities.DatabaseEntities.Collections;
using SQLDataProducer.DataAccess.Factories;
using SQLDataProducer.Entities.DatabaseEntities.Factories;

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
            ColumnDataTypeDefinition dbType = new ColumnDataTypeDefinition(reader.GetString(reader.GetOrdinal("DataType")), reader.GetBoolean(reader.GetOrdinal("IsNullable")));
            
            return DatabaseEntityFactory.CreateColumnEntity(reader.GetString(reader.GetOrdinal("ColumnName")), dbType, (bool)reader["IsIdentity"], reader.GetInt32(reader.GetOrdinal("OrdinalPosition")), (bool)reader["IsForeignKey"], new ForeignKeyEntity
                {
                    ReferencingTable = new TableEntity(
                        reader.GetStringOrEmpty("ReferencedTableSchema"),
                        reader.GetStringOrEmpty("ReferencedTable")),
                    ReferencingColumn = reader.GetStringOrEmpty("ReferencedColumn")
                });
        };


        readonly string GET_COLUMNS_FOR_TABLE_QUERY = @"
select 
    name As ColumnName, 
    CASE datatyp.datatypen
        WHEN 'varchar' THEN datatyp.datatypen + '(' + case cols.max_length when -1 then 'max' else CAST(cols.max_length AS VARCHAR(100)) end + ')'
        WHEN 'nvarchar' THEN datatyp.datatypen + '(' + case cols.max_length when -1 then 'max' else CAST(cols.max_length / 2 AS VARCHAR(100)) end + ')'
        WHEN 'char' THEN datatyp.datatypen + '(' + CAST(cols.max_length AS VARCHAR(100)) + ')'
        WHEN 'nchar' THEN datatyp.datatypen + '(' + CAST(cols.max_length / 2 AS VARCHAR(100)) + ')'
        WHEN 'decimal' THEN datatyp.datatypen + '(' + CAST(cols.precision AS VARCHAR(100)) + ', ' + CAST(cols.scale AS VARCHAR(100)) +')'
        WHEN 'varbinary' THEN datatyp.datatypen + '(' + case cols.max_length when -1 then 'max' else CAST(cols.max_length AS VARCHAR(100)) end + ')'
        WHEN 'datetime2' THEN datatyp.datatypen + '(' + CAST(cols.scale AS VARCHAR(100)) + ')'
        ELSE datatyp.datatypen
    END as DataType
    , column_id as OrdinalPosition
    , is_identity as IsIdentity 
    , is_nullable as IsNullable
	, IsForeignKey = cast(case when foreignInfo.ReferencedTable is null then 0 else 1 end as bit)
	, ReferencedTableSchema = foreignInfo.ReferencedTableSchema
	, ReferencedTable = foreignInfo.ReferencedTable
	, ReferencedColumn = foreignInfo.ReferencedColumn

from 
    sys.columns cols

cross apply
(
	select 	 	top 1
		COALESCE(bt.name, t.name) as DataTypen
	from
		sys.types AS t
		--ON c.user_type_id = t.user_type_id
	LEFT OUTER JOIN 
		sys.types AS bt
		ON t.is_user_defined = 1
		AND bt.is_user_defined = 0
		AND t.system_type_id = bt.system_type_id
		AND t.user_type_id <> bt.user_type_id
		
		where t.system_type_id = cols.system_type_id and  cols.user_type_id = t.user_type_id
) datatyp(datatypen)

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
        public IEnumerable<ColumnEntity> GetAllColumnsForTable(TableEntity table)
        {
            var cols = GetMany(
                            string.Format(GET_COLUMNS_FOR_TABLE_QUERY
                                    , table.TableName
                                    , table.TableSchema)
                            , CreateColumnEntity);

            // TODO: Dont add these foreign key generators here. Should be handled more central
            foreach (var item in cols.Where(x => x.IsForeignKey))
            {
                GetForeignKeyGeneratorsForColumn(item);
            }

            return cols;
        }

        private void GetForeignKeyGeneratorsForColumn(ColumnEntity column)
        {
            column.ForeignKey.Keys = ForeignKeyManager.Instance.GetPrimaryKeysForTable(this._connectionString, column.ForeignKey.ReferencingTable, column.ForeignKey.ReferencingColumn);
            
            // If we found any keys in the foreign table, add the generators.
            // TODO: When the lazy implementation is done, this need to be adjusted
            if (column.ForeignKey.Keys.Count > 0)
            {
                IEnumerable<Generator> fkGenerators = GeneratorFactory.GetForeignKeyGenerators(column.ForeignKey.Keys);
                foreach (var fkgen in fkGenerators)
                {
                    column.PossibleGenerators.Add(fkgen);
                }

                column.Generator = fkGenerators.First();
            }
            
        }
    }
}
