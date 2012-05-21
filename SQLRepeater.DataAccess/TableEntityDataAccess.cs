using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using SQLRepeater.Entities;
using System.Collections.ObjectModel;
using SQLRepeater.DatabaseEntities.Entities;

namespace SQLRepeater.DataAccess
{
    public partial class TableEntityDataAccess : DataAccessBase
    {
        readonly string ALL_TABLES_QUERY = "select Table_Name, Table_Schema from information_Schema.Tables order by table_Schema, table_name";

        public TableEntityDataAccess(string connectionString) 
            : base(connectionString)
        {
        }

        private Func<SqlDataReader, TableEntity> CreateTableEntity = reader =>
        {
            return new TableEntity
            {
                TableName = reader.GetString(0),
                TableSchema = reader.GetString(1)
            };
        };


        public TableEntity GetTableAndColumns(string tableSchema, string tableName)
        {
            ColumnEntityDataAccess colDa = new ColumnEntityDataAccess(this._connectionString);
            TableEntity table = GetOne(string.Format("select Table_Name, Table_Schema from information_Schema.Tables where table_schema = '{0}' and table_name = '{1}'", tableSchema, tableName), CreateTableEntity);
            table.Columns = colDa.GetAllColumnsForTable(table);

            return table;
        }

        public void BeginGetAllTables(Action<ObservableCollection<TableEntity>> callback)
        {
            BeginGetMany(ALL_TABLES_QUERY, CreateTableEntity, callback);
        }
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

        public ObservableCollection<TableEntity> GetAllTables()
        {
            return GetMany(ALL_TABLES_QUERY, CreateTableEntity);
        }

        public ObservableCollection<TableEntity> GetAllTablesWithColumns()
        {
            ObservableCollection<TableEntity> tables = GetAllTables();
            ColumnEntityDataAccess cda = new ColumnEntityDataAccess(_connectionString);
            foreach (var item in tables)
            {
                item.Columns = cda.GetAllColumnsForTable(item);
            }

            return tables;
        }
    }
}
