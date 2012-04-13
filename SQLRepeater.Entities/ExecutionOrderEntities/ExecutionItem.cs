using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLRepeater.DatabaseEntities.Entities;

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

        string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
    }
}
