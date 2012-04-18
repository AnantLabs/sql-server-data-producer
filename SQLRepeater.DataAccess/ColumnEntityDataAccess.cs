using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLRepeater.Entities;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using SQLRepeater.DatabaseEntities.Entities;
using SQLRepeater.Generators;
using SQLRepeater.Entities.ValueGeneratorParameters;

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
            (
                reader.GetString(0),
                reader.GetString(1),
                (bool)reader["IsIdentity"],
                reader.GetInt32(2),
                Generatorsupplier.GetDefaultGeneratorForDataType(reader.GetString(1)),
                ParameterSupplier.GetGeneratorParameterForDataType(reader.GetString(1))
            );
        };


        readonly string GET_COLUMNS_FOR_TABLE_QUERY = "select name As ColumnName, type_name(system_type_id), column_id as OrdinalPosition, is_identity as IsIdentity from sys.columns where object_id=object_id('{1}.{0}')";


        public void BeginGetAllColumnsForTable(TableEntity table, Action<ObservableCollection<ColumnEntity>> callback)
        {
            BeginGetMany(
                string.Format(GET_COLUMNS_FOR_TABLE_QUERY
                        , table.TableName
                        , table.TableSchema)
                , CreateColumnEntity
                , callback);
        }


        public ObservableCollection<ColumnEntity> GetAllColumnsForTable(TableEntity table)
        {
            return GetMany(
                string.Format(GET_COLUMNS_FOR_TABLE_QUERY
                        , table.TableName
                        , table.TableSchema)
                , CreateColumnEntity );
        }
    }
}
