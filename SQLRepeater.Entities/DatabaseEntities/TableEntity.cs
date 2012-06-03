using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace SQLRepeater.DatabaseEntities.Entities
{
    public class TableEntity : SQLRepeater.Entities.EntityBase
    {

        public TableEntity()
        {
            Columns = new ObservableCollection<ColumnEntity>();
        }


        //public Func<int, SqlParameter[]> GetParamValueCreator()
        //{
        //    Func<int, SqlParameter[]> paramCreator = n =>
        //        {
        //            List<SqlParameter> pars = new List<SqlParameter>();
        //            foreach (var col in Columns)
        //            {
        //                if (!col.IsIdentity)
        //                {
        //                    pars.Add(new SqlParameter(
        //                        string.Format("@{0}", col.ColumnName)
        //                        , col.Generator.GenerateValue(n)));
        //                }
        //            }

        //            return pars.ToArray();
        //        };
        //    return paramCreator;
        //}

        ObservableCollection<ColumnEntity> _columns;
        public ObservableCollection<ColumnEntity> Columns
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
    }
}
