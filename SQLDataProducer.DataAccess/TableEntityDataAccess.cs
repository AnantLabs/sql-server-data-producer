// Copyright 2012-2013 Peter Henell

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
        //readonly string ALL_TABLES_QUERY = "select Table_Name, Table_Schema from information_Schema.Tables where TABLE_TYPE = 'BASE TABLE' order by table_Schema, table_name";

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
               FROM 
                    SYS.objects soc  WITH(NOLOCK)
               JOIN 
                    sys.schemas scc WITH(NOLOCK)
                    ON soc.schema_id = scc.schema_id
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
               FROM     SYS.foreign_key_columns sfc WITH(NOLOCK)
                        JOIN sys.Objects sop  WITH(NOLOCK)
                            ON sfc.parent_object_id = sop.object_id
                        JOIN SYS.columns socp  WITH(NOLOCK)
                            ON socp.object_id = sop.object_id
                            AND socp.column_id = sfc.parent_column_id
                        JOIN sys.schemas scp  WITH(NOLOCK)
                            ON sop.schema_id = scp.schema_id
                        JOIN sys.objects soc  WITH(NOLOCK)
                            ON sfc.referenced_object_id = soc.object_id
                        JOIN sys.columns socc  WITH(NOLOCK)
                            ON socc.object_id = soc.object_id
                            AND socc.column_id = sfc.referenced_column_id
                        JOIN sys.schemas scc  WITH(NOLOCK)
                            ON soc.schema_id = scc.schema_id
                        JOIN fkey f  WITH(NOLOCK)
                            ON f.ReferencingObjectid = sfc.referenced_object_id
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
	FROM sys.objects o  WITH(NOLOCK)
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
		JOIN sys.foreign_keys fk WITH(NOLOCK) ON fk.parent_object_id = Rels.ParentId
		JOIN sys.objects co WITH(NOLOCK) ON co.object_id = fk.parent_object_id
		JOIN sys.objects po WITH(NOLOCK) ON po.object_id = fk.referenced_object_id
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

        private readonly string ALL_Tables_And_All_Columns_SQL_Query = @"select 
	Table_Schema = obj.table_schema,
	Table_Name = object_name(cols.object_id),
    ColumnName = name, 
    DataType = 
		CASE datatyp.datatypen
			WHEN 'varchar' THEN datatyp.datatypen + '(' + case cols.max_length when -1 then 'max' else CAST(cols.max_length AS VARCHAR(100)) end + ')'
			WHEN 'nvarchar' THEN datatyp.datatypen + '(' + case cols.max_length when -1 then 'max' else CAST(cols.max_length / 2 AS VARCHAR(100)) end + ')'
			WHEN 'char' THEN datatyp.datatypen + '(' + CAST(cols.max_length AS VARCHAR(100)) + ')'
			WHEN 'nchar' THEN datatyp.datatypen + '(' + CAST(cols.max_length / 2 AS VARCHAR(100)) + ')'
			WHEN 'decimal' THEN datatyp.datatypen + '(' + CAST(cols.precision AS VARCHAR(100)) + ', ' + CAST(cols.scale AS VARCHAR(100)) +')'
			WHEN 'varbinary' THEN datatyp.datatypen + '(' + case cols.max_length when -1 then 'max' else CAST(cols.max_length AS VARCHAR(100)) end + ')'
			WHEN 'datetime2' THEN datatyp.datatypen + '(' + CAST(cols.scale AS VARCHAR(100)) + ')'
			ELSE datatyp.datatypen
		END
    , OrdinalPosition = column_id
    , IsIdentity = is_identity
    , IsNullable = is_nullable
	, IsForeignKey = cast(case when foreignInfo.ReferencedTable is null then 0 else 1 end as bit)
	, ReferencedTableSchema = foreignInfo.ReferencedTableSchema
	, ReferencedTable = foreignInfo.ReferencedTable
	, ReferencedColumn = foreignInfo.ReferencedColumn
    , ConstraintDefinition = constraintDef 
from 
    sys.columns cols WITH(NOLOCK)
cross apply
(
	SELECT schema_name(o.schema_id) 
	FROM sys.objects o  WITH(NOLOCK)
	where o.object_id = cols.object_id
) obj(table_schema)

cross apply
(
	select 	 	top 1
		COALESCE(bt.name, t.name) as DataTypen
	from
		sys.types AS t WITH(NOLOCK)
		--ON c.user_type_id = t.user_type_id
	LEFT OUTER JOIN 
		sys.types AS bt WITH(NOLOCK)
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
		sys.tables AS tbl WITH(NOLOCK)
	LEFT JOIN 
		sys.foreign_keys AS fkeys WITH(NOLOCK)
		ON fkeys.parent_object_id = tbl.object_id
	LEFT JOIN 
		sys.foreign_key_columns AS fkcols WITH(NOLOCK)
		ON fkcols.constraint_object_id = fkeys.object_id

	LEFT JOIN 
		sys.columns AS referencedCols WITH(NOLOCK)
		ON fkcols.parent_column_id = referencedCols.column_id
		AND fkcols.parent_object_id = referencedCols.object_id

	left join 
		sys.tables as referencedTable WITH(NOLOCK)
		on referencedTable.object_id = fkeys.referenced_object_id
	LEFT JOIN 
		sys.columns AS crk WITH(NOLOCK)
		ON fkcols.referenced_column_id = crk.column_id
		AND fkcols.referenced_object_id = crk.object_id

		where 
			referencedCols.column_id = cols.column_id
			and cols.object_id = tbl.object_id

) foreignInfo( ReferencedTableSchema,ReferencedTable, ReferencedColumn)


OUTER APPLY
		(
		SELECT '\n' + c.CHECK_CLAUSE + '\n'  AS [text()] 
		FROM
			INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE cu   WITH(NOLOCK)
		INNER JOIN
			INFORMATION_SCHEMA.CHECK_CONSTRAINTS c  WITH(NOLOCK)
			ON cu.CONSTRAINT_NAME = c.CONSTRAINT_NAME
		WHERE
			cu.CONSTRAINT_NAME = c.CONSTRAINT_NAME
			AND cu.COLUMN_NAME = cols.name AND object_name(cols.object_id) = cu.TABLE_NAME
		FOR XML PATH('')
				
        ) AS  constr(constraintDef)   

where 
	object_id in (SELECT object_id(TABLE_SCHEMA + '.' + TABLE_NAME) FROM INFORMATION_SCHEMA.tables WITH(NOLOCK) where table_type = 'base table' ) 

and cols.is_computed = 0

order by table_schema, table_name


";


        public TableEntityDataAccess(string connectionString) 
            : base(connectionString)
        {
        }

        /// <summary>
        /// function that will create a TableEntity from a sqldatareader
        /// </summary>
        private static Func<SqlDataReader, TableEntity> CreateTableEntity = reader =>
        {
            return new TableEntity(
                reader.GetString(reader.GetOrdinal("Table_Schema")),
                reader.GetString(reader.GetOrdinal("Table_Name"))
                );
        };

        /// <summary>
        /// function that will create a TableEntity with all the columns from a sqldatareader
        /// </summary>
        private static Func<SqlDataReader, TableEntity> CreateTableAndColumnsEntity = reader =>
        {
            var t = new
            {
                TableName = reader.GetString(reader.GetOrdinal("Table_Name")),
                TableSchema = reader.GetString(reader.GetOrdinal("Table_Schema"))
            };

            var table = new TableEntity(t.TableSchema, t.TableName);
            do
            {
                table.Columns.Add(ColumnEntityDataAccess.CreateColumn(reader));

            } while (reader.Read() && (
                reader.GetString(reader.GetOrdinal("Table_Name")) == t.TableName &&
                t.TableSchema == reader.GetString(reader.GetOrdinal("Table_Schema"))));
	    
            
            return table;
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
                TableEntity table = GetOne(string.Format("select Table_Name, Table_Schema from information_Schema.Tables WITH(NOLOCK) where table_schema = '{0}' and table_name = '{1}'", tableSchema, tableName), CreateTableEntity);
                table.Columns.AddRange(colDa.GetAllColumnsForTable(table));
                //    // TODO: Dont add these foreign key generators here. Should be handled more central
                foreach (var item in table.Columns.Where(x => x.IsForeignKey))
                {
                    GetForeignKeyGeneratorsForColumn(item);
                }
                
                table.RefreshWarnings();
                return table;
            }
        }

       
        /// <summary>
        /// Begin get all tables and their columns, does not block the caller. The provided callback method will be called when the exection is done.
        /// </summary>
        /// <param name="callback">the callback method to be called when the execution is done.</param>
        public void BeginGetAllTablesAndColumns(Action<TableEntityCollection> callback)
        {
            Action a = new Action( () =>
                {
                    callback(GetAllTablesAndColumns());
                });
            a.BeginInvoke(null, null);
        }


        public TableEntityCollection GetAllTablesAndColumns()
        {
            TableEntityCollection tables = new TableEntityCollection(GetMany(ALL_Tables_And_All_Columns_SQL_Query, CreateTableAndColumnsEntity));

            
            return tables;

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

        public ObservableCollection<string> GetPrimaryKeysForColumnInTable(TableEntity table, string primaryKeyColumn)
        {
            string s = string.Format("SELECT TOP {0} {1} FROM {2}.{3} WITH(NOLOCK)", 1000, primaryKeyColumn, table.TableSchema, table.TableName);
            Func<SqlDataReader, string> createKey = reader =>
            {
                return reader.GetValue(0).ToString();
            };
            return new ObservableCollection<string>(GetMany(s, createKey));
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

        public List<TableEntity> CloneTables(IEnumerable<TableEntity> tables)
        {
            List<TableEntity> list = new List<TableEntity>();
            foreach (var table in tables)
            {
                list.Add(CloneTable(table));
            }

            return list;
        }

        public List<TableEntity> GetTreeStructureWithTableAsLeaf(TableEntity tableAsLeafOfTree, TableEntityCollection tablesAvailAble)
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
