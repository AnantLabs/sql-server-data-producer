using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace SQLRepeater.Entities
{
    public class TableEntity : EntityBase
    {

        public TableEntity()
        {
            Columns = new ObservableCollection<ColumnEntity>();
        }


        public Func<int, SqlParameter[]> GetParamCreator()
        {
            Func<int, SqlParameter[]> paramCreator = n =>
                {
                    SqlParameter[] pars = new SqlParameter[Columns.Count];
                    for (int i = 0; i < pars.Length; i++)
                    {
                        pars[i] = new SqlParameter(
                            string.Format("@{0}", Columns[i].ColumnName)
                            , Columns[i].ColumnValue.ValueGenerator(n));
                    }

                    return pars;
                };
            return paramCreator;
        }

        //public Func<int, SqlParameter[]> paramCreator = n =>
        //{
        //    SqlParameter[] pars = new SqlParameter[Columns.Count];
        //    for (int i = 0; i < pars.Length; i++)
        //    {
        //        pars[i] = new SqlParameter(
        //            string.Format("@{0}", Columns[i].ColumnName)
        //            , Columns[i].ColumnValue.ValueGenerator(n));
        //    }

        //    return pars;
        //};


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
