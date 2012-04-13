using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SQLRepeater.DatabaseEntities.Entities;
using SQLRepeater.Entities.ExecutionOrderEntities;
using SQLRepeater.DataAccess;

namespace SQLRepeater.Model
{
    public class ApplicationModel : Entities.EntityBase
    {
        ObservableCollection<TableEntity> _tables;
        public ObservableCollection<TableEntity> Tables
        {
            get
            {
                return _tables;
            }
            set
            {
                if (_tables != value)
                {
                    _tables = value;
                    OnPropertyChanged("Tables");
                }
            }
        }

        TableEntity _selectedTable;
        public TableEntity SelectedTable
        {
            get
            {
                return _selectedTable;
            }
            set
            {
                if (_selectedTable != value)
                {
                    _selectedTable = value;
                    GetColumnsForTable(_selectedTable);
                    OnPropertyChanged("SelectedTable");
                }
            }
        }

        private void GetColumnsForTable(TableEntity table)
        {
            if (table == null)
            {
                return;
            }
            //if (table.Columns.Count > 0)
            //    return;

            ColumnEntityDataAccess da = new ColumnEntityDataAccess(ConnectionString);
            da.BeginGetAllColumnsForTable(table, cols =>
            {
                table.Columns = cols;
            });
        }

        ObservableCollection<ExecutionItem> _executionItems;
        public ObservableCollection<ExecutionItem> ExecutionItems
        {
            get
            {
                return _executionItems;
            }
            set
            {
                if (_executionItems != value)
                {
                    _executionItems = value;
                    OnPropertyChanged("ExecutionItems");
                }
            }
        }

        ExecutionItem _currentExecutionItem;
        public ExecutionItem SelectedExecutionItem
        {
            get
            {
                return _currentExecutionItem;
            }
            set
            {
                if (_currentExecutionItem != value)
                {
                    _currentExecutionItem = value;
                    OnPropertyChanged("SelectedExecutionItem");
                }
            }
        }


        string _connectionString = string.Empty;
        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = SQLRepeaterSettings.Default.ConnectionString;
                }
                return _connectionString;
            }
            set
            {
                if (_connectionString != value)
                {
                    _connectionString = value;
                    SQLRepeaterSettings.Default.ConnectionString = value;
                    SQLRepeaterSettings.Default.Save();
                    OnPropertyChanged("ConnectionString");
                }
            }
        }
    }
}
