using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using SQLDataProducer.Entities.DatabaseEntities.Collections;

namespace SQLDataProducer.DatabaseEntities.Entities
{
    public class TableEntity : SQLDataProducer.Entities.EntityBase, IEquatable<TableEntity>
    {

        public TableEntity(string tableSchema, string tableName)
        {
            Columns = new ColumnEntityCollection();
            TableName = tableName;
            TableSchema = tableSchema;
        }

        ColumnEntityCollection _columns;
        public ColumnEntityCollection Columns
        {
            get
            {
                return _columns;
            }
            set
            {
                if (_columns != value)
                {
                    _columns = value;
                    OnPropertyChanged("Columns");
                }
            }
        }

        string _tableSchema;
        public string TableSchema
        {
            get
            {
                return _tableSchema;
            }
            set
            {
                if (_tableSchema != value)
                {
                    _tableSchema = value;
                    OnPropertyChanged("TableSchema");
                }
            }
        }

        string _tableName;
        public string TableName
        {
            get
            {
                return _tableName;
            }
            set
            {
                if (_tableName != value)
                {
                    _tableName = value;
                    OnPropertyChanged("TableName");
                }
            }
        }


        public override string ToString()
        {
            return string.Format("{0}.{1}", TableSchema, TableName);
        }



        public bool Equals(TableEntity other)
        {
            // Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            // Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            // Check whether the objects’ properties are equal.
            return this.ToString().Equals(other.ToString());
        }

        // If Equals returns true for a pair of objects,
        // GetHashCode must return the same value for these objects.

        public override int GetHashCode()
        {
            // Get the hash code for the Textual field if it is not null.
            return ToString().GetHashCode();

        }

        internal TableEntity Clone()
        {
            return TableEntity.Create(this.TableSchema, this.TableName, this.Columns.Clone());
        }

        private static TableEntity Create(string tableSchema, string tableName, ColumnEntityCollection cols)
        {
            var t = new TableEntity(tableSchema, tableName);
            t.Columns = cols;
            return t;
        }
    }
}
