using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLRepeater.Entities;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace SQLRepeater.DataAccess
{
    public class ColumnEntityDataAccess : DataAccessBase
    {
        public ColumnEntityDataAccess(string connectionString)
            : base(connectionString)
        { }

        private Func<SqlDataReader, ColumnEntity> CreateColumnEntity = reader =>
        {
            return new ColumnEntity
            {
                ColumnName = reader.GetString(0),
                ColumnDataType = reader.GetString(1),
                OrdinalPosition = reader.GetInt32(2),
                IsIdentity = (bool)reader["IsIdentity"] ,
                ColumnValue = new ValueEntity()
            };
        };


        public void BeginGetAllColumnsForTable(TableEntity table, Action<ObservableCollection<ColumnEntity>> callback)
        {
            BeginGetMany(
                string.Format("select name As ColumnName, type_name(system_type_id), column_id as OrdinalPosition, is_identity as IsIdentity from sys.columns where object_id=object_id('{1}.{0}')"
                        , table.TableName
                        , table.TableSchema)
                , CreateColumnEntity
                , callback);
        }

    }
}
