using SQLDataProducer.Entities.DataEntities;
using SQLDataProducer.Entities.ExecutionEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer
{
    public class TableQueryGenerator
    {
        private readonly Entities.DatabaseEntities.TableEntity table;
        private readonly string columnList;
        private readonly string insertStatement;

        public string ColumnList
        {
            get { return columnList; }
        }
       
        public string InsertStatement
        {
            get { return insertStatement; }
        }

        public TableQueryGenerator(Entities.DatabaseEntities.TableEntity table)
        {
            this.table = table;
            this.columnList = string.Join(", ", table.Columns.Where(x => x.IsIdentity == false).Select(x => x.ColumnName));
            this.insertStatement = "INSERT " + table.TableName + "(" + columnList + ")";

        }

        public string GenerateValuesStatement(DataRowEntity row, ValueStore valueStore)
        {
            return "VALUES (" +
                    string.Join(", ", row.Fields
                    .Where(x => x.ProducesValue == false)
                    .Select( x => string.Format(x.DataType.StringFormatter, valueStore.GetByKey(x.KeyValue))))
                    + ")";
        }

        public IEnumerable<string> GenerateInsertStatements(IEnumerable<DataRowEntity> rows, ValueStore valueStore)
        {
            foreach (var row in rows)
            {
                yield return insertStatement + " " + GenerateValuesStatement(row, valueStore);
            }
        }
    }
}
