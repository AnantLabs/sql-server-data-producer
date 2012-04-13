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

        public void BeginGetAllTables(Action<ObservableCollection<TableEntity>> callback)
        {
            BeginGetMany("select Table_Name, Table_Schema from information_Schema.Tables order by table_Schema, table_name", CreateTableEntity, callback);
        }
    }
}
