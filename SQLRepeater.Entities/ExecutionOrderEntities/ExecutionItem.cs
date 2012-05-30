using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLRepeater.DatabaseEntities.Entities;
using System.Collections.ObjectModel;

namespace SQLRepeater.Entities.ExecutionOrderEntities
{
    public class ExecutionItem : EntityBase
    {
        TableEntity _targetTable;
        public TableEntity TargetTable
        {
            get
            {
                return _targetTable;
            }
            set
            {
                if (_targetTable != value)
                {
                    _targetTable = value;
                    OnPropertyChanged("TargetTable");
                }
            }
        }


        int _order;
        public int Order
        {
            get
            {
                return _order;
            }
            set
            {
                if (_order != value)
                {
                    _order = value;
                    OnPropertyChanged("Order");
                }
            }
        }

        public ExecutionItem(TableEntity table, int order)
        {
            TargetTable = table;
            Order = order;
        }



        string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        bool _truncateBeforeExecution;
        public bool TruncateBeforeExecution
        {
            get
            {
                return _truncateBeforeExecution;
            }
            set
            {
                if (_truncateBeforeExecution != value)
                {
                    _truncateBeforeExecution = value;
                    OnPropertyChanged("TruncateBeforeExecution");
                }
            }
        }

        //public static ObservableCollection<ExecutionItem> FromTables(IEnumerable<TableEntity> tables)
        //{
        //    ObservableCollection<ExecutionItem> c = new ObservableCollection<ExecutionItem>();
        //    int count = 1;
        //    foreach (var item in tables)
        //    {
        //        c.Add(new ExecutionItem(item, count++));
        //    }
        //    return c;
        //}
    }
}
