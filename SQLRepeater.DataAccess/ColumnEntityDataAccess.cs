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

        private Func<SqlDataReader, ColumnEntity> CreateColumnEntity = reader =>
        {
            ObservableCollection<GeneratorBase> generators = Generatorsupplier.GetGeneratorsForDataType(reader.GetString(1));
            return new ColumnEntity
            (
                reader.GetString(0),
                reader.GetString(1),
                (bool)reader["IsIdentity"],
                reader.GetInt32(2),
                generators.FirstOrDefault(),
                generators
            );
        };


        readonly string GET_COLUMNS_FOR_TABLE_QUERY = @"select name As ColumnName, CASE TYPE_NAME(cols.system_type_id)
    WHEN 'varchar' THEN TYPE_NAME(cols.system_type_id) + '(' + CAST(cols.max_length AS VARCHAR(100)) + ')'
    WHEN 'nvarchar' THEN TYPE_NAME(cols.system_type_id) + '(' + CAST(cols.max_length AS VARCHAR(100)) + ')'
    WHEN 'char' THEN TYPE_NAME(cols.system_type_id) + '(' + CAST(cols.max_length AS VARCHAR(100)) + ')'
    WHEN 'nchar' THEN TYPE_NAME(cols.system_type_id) + '(' + CAST(cols.max_length AS VARCHAR(100)) + ')'
    WHEN 'decimal' THEN TYPE_NAME(cols.system_type_id) + '(' + CAST(cols.precision AS VARCHAR(100)) + ', ' + CAST(cols.scale AS VARCHAR(100)) +')'
    WHEN 'varbinary' THEN TYPE_NAME(cols.system_type_id) + '(' + CAST(cols.max_length AS VARCHAR(100)) + ')'
    WHEN 'datetime2' THEN TYPE_NAME(cols.system_type_id) + '(' + CAST(cols.scale AS VARCHAR(100)) + ')'
    ELSE TYPE_NAME(cols.system_type_id)
END as DataType, column_id as OrdinalPosition, is_identity as IsIdentity from sys.columns cols where object_id=object_id('{1}.{0}')";


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
