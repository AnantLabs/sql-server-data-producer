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
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DatabaseEntities.Collections;
using System.Collections.Generic;
using System.Linq;
using SQLDataProducer.EntityQueryGenerator;
using SQLDataProducer.Entities.Generators;

namespace SQLDataProducer.DataAccess
{
    public partial class TableEntityDataAccess : DataAccessBase
    {
        readonly string ALL_TABLES_QUERY = "select Table_Name, Table_Schema from information_Schema.Tables where TABLE_TYPE = 'BASE TABLE' order by table_Schema, table_name";

        readonly string TABLES_IN_HIERARCHY_FROM_ROOT = @"DECLARE    @RowOrder INT = 0
			,@SchemaName sysname = '{0}'
			,@TableName sysname = '{1}'
			
	DECLARE @fkeytbl TABLE
    (
      ReferencingObjectid		INT		NULL
      ,ReferencingSchemaname	SYSNAME NULL
      ,ReferencingTablename		SYSNAME NULL
      ,ReferencingColumnname	SYSNAME NULL
      ,PrimarykeyObjectid		INT		NULL
      ,PrimarykeySchemaname		SYSNAME NULL
      ,PrimarykeyTablename		SYSNAME NULL
      ,PrimarykeyColumnname		SYSNAME NULL
      ,Hierarchy				VARCHAR(MAX) NULL 
      ,Generation				INT NULL
      ,Ranking						VARCHAR(MAX) NULL
      ,Processed				BIT DEFAULT 0  NULL
    ) ; 
 
 
	WITH fkey (ReferencingObjectid, ReferencingSchemaname, ReferencingTablename, ReferencingColumnname, PrimarykeyObjectid, PrimarykeySchemaname, PrimarykeyTablename, PrimarykeyColumnname, Hierarchy, Generation, Ranking )
          AS ( SELECT   soc.object_id ,
                        scc.name ,
                        soc.name ,
                        CONVERT(SYSNAME, NULL) ,
                        CONVERT(INT, NULL) ,
                        CONVERT(SYSNAME, NULL) ,
                        CONVERT(SYSNAME, NULL) ,
                        CONVERT(SYSNAME, NULL) ,
                        CONVERT(VARCHAR(MAX), scc.name + '.' + soc.name) AS Hierarchy ,
                        0 AS Generation,
                        Ranking = CONVERT(VARCHAR(MAX), soc.object_id)
               FROM     SYS.objects soc
                        JOIN sys.schemas scc ON soc.schema_id = scc.schema_id
               WHERE    scc.name = @Schemaname
                        AND soc.name = @Tablename
               UNION ALL
               SELECT   sop.object_id ,
                        scp.name ,
                        sop.name ,
                        socp.name ,
                        soc.object_id ,
                        scc.name ,
                        soc.name ,
                        socc.name ,
                        CONVERT(VARCHAR(MAX), f.Hierarchy + ' --> ' + scp.name
                        + '.' + sop.name) AS Hierarchy ,
                        f.Generation + 1 AS Generation,
                        Ranking = f.Ranking + '-'
                        + CONVERT(VARCHAR(MAX), sop.object_id)
               FROM     SYS.foreign_key_columns sfc
                        JOIN sys.Objects sop ON sfc.parent_object_id = sop.object_id
                        JOIN SYS.columns socp ON socp.object_id = sop.object_id
                                                 AND socp.column_id = sfc.parent_column_id
                        JOIN sys.schemas scp ON sop.schema_id = scp.schema_id
                        JOIN sys.objects soc ON sfc.referenced_object_id = soc.object_id
                        JOIN sys.columns socc ON socc.object_id = soc.object_id
                                                 AND socc.column_id = sfc.referenced_column_id
                        JOIN sys.schemas scc ON soc.schema_id = scc.schema_id
                        JOIN fkey f ON f.ReferencingObjectid = sfc.referenced_object_id
               WHERE    ISNULL(f.PrimarykeyObjectid, 0) <> f.ReferencingObjectid
             )
    INSERT  INTO @fkeytbl
            ( ReferencingObjectid ,
              ReferencingSchemaname ,
              ReferencingTablename ,
              ReferencingColumnname ,
              PrimarykeyObjectid ,
              PrimarykeySchemaname ,
              PrimarykeyTablename ,
              PrimarykeyColumnname ,
              Hierarchy ,
              Generation,
              Ranking
            )
            SELECT  ReferencingObjectid ,
                    ReferencingSchemaname ,
                    ReferencingTablename ,
                    ReferencingColumnname ,
                    PrimarykeyObjectid ,
                    PrimarykeySchemaname ,
                    PrimarykeyTablename ,
                    PrimarykeyColumnname ,
                    Hierarchy ,
                    Generation,
                    Ranking
            FROM    fkey 
         
	SELECT ROW_NUMBER() OVER (ORDER BY f.Ranking ASC) AS RowOrder
		, f.SchemaName as Table_Schema
		, f.TableName as Table_Name
		, f.Generation
		, f.ParentSchemaName
		, f.ParentTableName
	FROM (SELECT DISTINCT
					ReferencingSchemaname as SchemaName
                    , ReferencingTablename as TableName 
                    ,Ranking 
                    ,Generation
                    ,PrimarykeySchemaname as ParentSchemaName
                    
                    ,PrimarykeyTablename AS ParentTableName
          FROM      @fkeytbl
        ) f
ORDER BY RowOrder ASC ";


        readonly string TABLES_IN_HIERARCHY_WITH_TABLE_AS_LEAF = @"DECLARE  @RowOrder INT = 0
		,@SchemaName sysname = '{0}'
		,@TableName sysname = '{1}'
			

; WITH Rels (
	 FKId
	,FKName
	,ChildId
	,ChildSchemaName
	,ChildName
	,ParentId
	,ParentSchemaName
	,ParentName
	,Hierarchy
	,Generation
	,Ranking 
)
AS ( 
	SELECT CONVERT(INT, null)		 FKId
		,CONVERT(sysname, '')	 FKName
		,o.object_id ChildId
		,SCHEMA_NAME(o.schema_id) ChildSchemaName
		,OBJECT_NAME(o.object_id) ChildName
		,o.object_id ParentId
		,SCHEMA_NAME(o.schema_id) ParentSchemaName
		,OBJECT_NAME(o.object_id) ParentName
		,CONVERT(VARCHAR(MAX), schema_name(o.schema_id)+ '.' + o.name) AS Hierarchy
		,0 AS Generation
		,Ranking = CONVERT(VARCHAR(MAX), o.object_id)
	FROM sys.objects o 
	WHERE o.object_id = object_id(@SchemaName + '.' + @TableName)

UNION ALL

	SELECT fk.object_id FKId
		,OBJECT_NAME(fk.object_id) FKName
		,co.object_id ChildId
		,SCHEMA_NAME(po.schema_id) ChildSchemaName
		,OBJECT_NAME(fk.parent_object_id) ChildName
		,fk.referenced_object_id ParentId
		,SCHEMA_NAME(co.schema_id) ParentSchemaName
		,OBJECT_NAME(fk.referenced_object_id) ParentName
		,CONVERT(VARCHAR(MAX), Rels.Hierarchy + ' --> ' 
			+ SCHEMA_NAME(co.schema_id)
            + '.' + po.name) AS Hierarchy 
        ,Rels.Generation + 1 AS Generation
        ,Ranking = Rels.Ranking + '-'
	FROM Rels
		JOIN sys.foreign_keys fk ON fk.parent_object_id = Rels.ParentId
		JOIN sys.objects co ON co.object_id = fk.parent_object_id
		JOIN sys.objects po ON po.object_id = fk.referenced_object_id
							AND po.object_id != co.object_id
)
SELECT ROW_NUMBER() OVER (ORDER BY Ranking ASC) AS RowNumber 
	  --,SchemaChildName as Table_Schema
	  --,ChildName  AS Table_Name
	  ,ChildSchemaName  as Table_Schema
	  ,ParentName AS Table_Name
	  ,Hierarchy
	  ,Generation AS [Level]
FROM Rels
ORDER BY RowNumber desc
";

        public TableEntityDataAccess(string connectionString) 
            : base(connectionString)
        {
        }

        /// <summary>
        /// function that will create a TableEntity from a sqldatareader
        /// </summary>
        private Func<SqlDataReader, TableEntity> CreateTableEntity = reader =>
        {
            return new TableEntity(
                reader.GetString(reader.GetOrdinal("Table_Schema")),
                reader.GetString(reader.GetOrdinal("Table_Name"))
                );
        };

        /// <summary>
        /// Create a TableEntity and get all its columns
        /// </summary>
        /// <param name="tableSchema">schema name of the table to get</param>
        /// <param name="tableName">tableName of the table to get</param>
        /// <returns></returns>
        public TableEntity GetTableAndColumns(string tableSchema, string tableName)
        {
            using (ColumnEntityDataAccess colDa = new ColumnEntityDataAccess(this._connectionString))
            {
                TableEntity table = GetOne(string.Format("select Table_Name, Table_Schema from information_Schema.Tables where table_schema = '{0}' and table_name = '{1}'", tableSchema, tableName), CreateTableEntity);
                table.Columns.AddRange(colDa.GetAllColumnsForTable(table));

                return table;
            }
        }



        ///// <summary>
        ///// Get all tables, does not block the caller. The provided callback method will be called when the exection is done.
        ///// </summary>
        ///// <param name="callback">the callback method to be called when the execution is done.</param>
        //public void BeginGetAllTables(Action<TableEntityCollection> callback)
        //{
        //    BeginGetMany(ALL_TABLES_QUERY, CreateTableEntity, callback);
        //}
        /// <summary>
        /// Begin get all tables and their columns, does not block the caller. The provided callback method will be called when the exection is done.
        /// </summary>
        /// <param name="callback">the callback method to be called when the execution is done.</param>
        public void BeginGetAllTablesAndColumns(Action<TableEntityCollection> callback)
        {
            BeginGetMany(ALL_TABLES_QUERY, CreateTableEntity, tables =>
                {
                    using (ColumnEntityDataAccess colDa = new ColumnEntityDataAccess(this._connectionString))
                    {
                        foreach (var tabl in tables)
                        {
                            tabl.Columns.AddRange(colDa.GetAllColumnsForTable(tabl));
                            // TODO: Dont add these foreign key generators here. Should be handled more central
                            foreach (var item in tabl.Columns.Where(x => x.IsForeignKey))
                            {
                                GetForeignKeyGeneratorsForColumn(item);
                            }
                        }
                    }
                    callback(new TableEntityCollection(tables));
                });
        }

        /// <summary>
        /// Get TableEntitites for every table in the database.
        /// </summary>
        /// <returns></returns>
        private TableEntityCollection GetAllTables()
        {
            return new TableEntityCollection(GetMany(ALL_TABLES_QUERY, CreateTableEntity));
        }

        public TableEntityCollection GetAllTablesAndColumns()
        {
            var tables = GetAllTables();
            using (ColumnEntityDataAccess colDa = new ColumnEntityDataAccess(this._connectionString))
            {
                foreach (var tabl in tables)
                {
                    tabl.Columns.AddRange(colDa.GetAllColumnsForTable(tabl));
                    // TODO: Dont add these foreign key generators here. Should be handled more central
                    foreach (var item in tabl.Columns.Where(x => x.IsForeignKey))
                    {
                        GetForeignKeyGeneratorsForColumn(item);
                    }
                }
                return tables;
            }
        }

        private void GetForeignKeyGeneratorsForColumn(ColumnEntity column)
        {
            column.ForeignKey.Keys = ForeignKeyManager.Instance.GetPrimaryKeysForTable(this, column.ForeignKey.ReferencingTable, column.ForeignKey.ReferencingColumn);

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

        /// <summary>
        /// Delete all rows in the provided table...
        /// </summary>
        /// <param name="table">the tableEntity for which all rows should be deleted.</param>
        public void TruncateTable(TableEntity table)
        { 
            // TODO: Handle foreign keys
            // Remove all? Recursively?
            ExecuteNoResult(string.Format("Delete {0}.{1}", table.TableSchema, table.TableName));
        }

        public ForeignKeyCollection GetPrimaryKeysForColumnInTable(TableEntity table, string primaryKeyColumn)
        {
            string s = string.Format("SELECT TOP {0} {1} FROM {2}.{3}", 1000, primaryKeyColumn, table.TableSchema, table.TableName);
            Func<SqlDataReader, string> createKey = reader =>
            {
                return reader.GetValue(0).ToString();
            };
            return new ForeignKeyCollection(GetMany(s, createKey));
        }


        public IEnumerable<TableEntity> GetTreeStructureFromRoot(TableEntity tableAsRootForTree, TableEntityCollection tablesAvailAble)
        {
            string s = string.Format(TABLES_IN_HIERARCHY_FROM_ROOT, tableAsRootForTree.TableSchema, tableAsRootForTree.TableName);

            Func<SqlDataReader, TableEntity> getTreeStructure = reader =>
                {

                    var tableInTree =
                                       new
                                       {
                                           TableSchema = reader.GetString(reader.GetOrdinal("Table_Schema")),
                                           TableName = reader.GetString(reader.GetOrdinal("Table_Name"))
                                       };

                    return tablesAvailAble.Where(x => 
                                        x.TableName == tableInTree.TableName 
                                        && x.TableSchema == tableInTree.TableSchema
                                    ).FirstOrDefault();
                };
            return GetMany(s, getTreeStructure);
        }


        public TableEntity CloneTable(TableEntity table)
        {
            return GetTableAndColumns(table.TableSchema, table.TableName);
        }

        public IEnumerable<TableEntity> CloneTables(IEnumerable<TableEntity> tables)
        {
            List<TableEntity> list = new List<TableEntity>();
            foreach (var table in tables)
            {
                list.Add(CloneTable(table));
            }

            return list;
        }

        public IEnumerable<TableEntity> GetTreeStructureWithTableAsLeaf(TableEntity tableAsLeafOfTree, TableEntityCollection tablesAvailAble)
        {
            string s = string.Format(TABLES_IN_HIERARCHY_WITH_TABLE_AS_LEAF, tableAsLeafOfTree.TableSchema, tableAsLeafOfTree.TableName);

            Func<SqlDataReader, TableEntity> getTreeStructure = reader =>
            {

                var tableInTree =
                                   new
                                   {
                                       TableSchema = reader.GetString(reader.GetOrdinal("Table_Schema")),
                                       TableName = reader.GetString(reader.GetOrdinal("Table_Name"))
                                   };

                return tablesAvailAble.Where(x =>
                                    x.TableName == tableInTree.TableName
                                    && x.TableSchema == tableInTree.TableSchema
                                ).FirstOrDefault();
            };
            return GetMany(s, getTreeStructure);
        }
    }
}
