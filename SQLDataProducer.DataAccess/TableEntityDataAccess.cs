using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using SQLDataProducer.Entities;
using System.Collections.ObjectModel;
using SQLDataProducer.DatabaseEntities.Entities;

namespace SQLDataProducer.DataAccess
{
    public partial class TableEntityDataAccess : DataAccessBase
    {
        readonly string ALL_TABLES_QUERY = "select Table_Name, Table_Schema from information_Schema.Tables order by table_Schema, table_name";

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
            ColumnEntityDataAccess colDa = new ColumnEntityDataAccess(this._connectionString);
            TableEntity table = GetOne(string.Format("select Table_Name, Table_Schema from information_Schema.Tables where table_schema = '{0}' and table_name = '{1}'", tableSchema, tableName), CreateTableEntity);
            table.Columns = colDa.GetAllColumnsForTable(table);

            return table;
        }

        /// <summary>
        /// Get all tables, does not block the caller. The provided callback method will be called when the exection is done.
        /// </summary>
        /// <param name="callback">the callback method to be called when the execution is done.</param>
        public void BeginGetAllTables(Action<ObservableCollection<TableEntity>> callback)
        {
            BeginGetMany(ALL_TABLES_QUERY, CreateTableEntity, callback);
        }
        /// <summary>
        /// Begin get all tables and their columns, does not block the caller. The provided callback method will be called when the exection is done.
        /// </summary>
        /// <param name="callback">the callback method to be called when the execution is done.</param>
        public void BeginGetAllTablesAndColumns(Action<ObservableCollection<TableEntity>> callback)
        {
            BeginGetMany(ALL_TABLES_QUERY, CreateTableEntity, tables =>
                {
                    ColumnEntityDataAccess colDa = new ColumnEntityDataAccess(this._connectionString);
                    foreach (var tabl in tables)
                    {
                        tabl.Columns = colDa.GetAllColumnsForTable(tabl);
                    }
                    callback(tables);
                });
        }

        /// <summary>
        /// Get TableEntitites for every table in the database.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<TableEntity> GetAllTables()
        {
            return GetMany(ALL_TABLES_QUERY, CreateTableEntity);
        }

        //public ObservableCollection<TableEntity> GetAllTablesWithColumns()
        //{
        //    ObservableCollection<TableEntity> tables = GetAllTables();
        //    ColumnEntityDataAccess cda = new ColumnEntityDataAccess(_connectionString);
        //    foreach (var item in tables)
        //    {
        //        item.Columns = cda.GetAllColumnsForTable(item);
        //    }

        //    return tables;
        //}

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

        public ObservableCollection<int> GetPrimaryKeysForColumnInTable(TableEntity table, string primaryKeyColumn)
        {
            string s = string.Format("SELECT TOP {0} {1} FROM {2}.{3}", 1000, primaryKeyColumn, table.TableSchema, table.TableName);
            Func<SqlDataReader, int> createKey = reader =>
            {
                int fk;
                if(int.TryParse(reader.GetValue(0).ToString(), out fk))
                    return fk;

                throw new NotImplementedException("Foreign keys can only be integer datatypes, not implemented for any other type of datatypes");
            };
            return GetMany(s, createKey);
        }
    }
}
