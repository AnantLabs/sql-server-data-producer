using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Data.Common;
using SQLRepeater.Entities;
using SQLRepeater.DataAccess;
using System.Collections.ObjectModel;
using System.Data.SqlClient;


namespace SQLRepeater
{
    public class MainWindowViewModel : System.ComponentModel.INotifyPropertyChanged
    {

        public DelegateCommand OpenSqlConnectionBuilderCommand { get; private set; }
        public DelegateCommand RunSQLQueryCommand { get; private set; }
        public DelegateCommand StopExecutionCommand { get; private set; }
        public DelegateCommand LoadTablesCommand { get; private set; }
        public DelegateCommand<ColumnEntity> OpenValueEditWindowCommand { get; private set; }


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

        private void GetColumnsForTable(TableEntity table)
        {
            if (table.Columns.Count > 0)
                return;

            ColumnEntityDataAccess da = new ColumnEntityDataAccess(_connectionString);
            da.BeginGetAllColumnsForTable(table, cols =>
                {
                    table.Columns = cols;
                });
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



        bool _isQueryRunning = true;
        public bool IsQueryRunning
        {
            get
            {
                return _isQueryRunning;
            }
            set
            {
                if (_isQueryRunning != value)
                {
                    _isQueryRunning = value;
                    OnPropertyChanged("IsQueryRunning");
                }
            }
        }


        int _durationInSeconds = 60;
        public int DurationInSeconds
        {
            get
            {
                return _durationInSeconds;
            }
            set
            {
                if (_durationInSeconds != value)
                {
                    _durationInSeconds = value;
                    OnPropertyChanged("DurationInSeconds");
                }
            }
        }


        int _numTasks = 25;
        public int NumTasks
        {
            get
            {
                return _numTasks;
            }
            set
            {
                if (_numTasks != value)
                {
                    _numTasks = value;
                    OnPropertyChanged("NumTasks");
                }
            }
        }


        int _taskProgPercent;
        public int TaskProgressPercentage
        {
            get
            {
                return _taskProgPercent;
            }
            set
            {
                if (_taskProgPercent != value)
                {
                    _taskProgPercent = value;
                    OnPropertyChanged("TaskProgressPercentage");
                }
            }
        }

     

        TaskExecuter.TaskExecuter _executor;

        private void LoadTables()
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(_connectionString);
            tda.BeginGetAllTables(res =>
            {
                Tables = res;
                SelectedTable = Tables.FirstOrDefault();
            });
        }

        public MainWindowViewModel()
        {
            RunSQLQueryCommand = new DelegateCommand(() =>
                {
                    EntityQueryGenerator.InsertQueryGenerator ig = 
                        new EntityQueryGenerator.InsertQueryGenerator();
                    
                    string sql = ig.GenerateQueryFor(SelectedTable);
                    _executor = new TaskExecuter.TaskExecuter();

                    Action<int> sqlTask = _executor.CreateSQLTask(sql
                        , SelectedTable.GetParamCreator() 
                        , ConnectionString);

                    IsQueryRunning = true;
                    _executor.BeginExecute(sqlTask
                        , DateTime.Now.AddSeconds(DurationInSeconds)
                        , NumTasks
                        , count =>
                        {
                            IsQueryRunning = false;
                            MessageBox.Show(count.ToString());
                        });
                });

            StopExecutionCommand = new DelegateCommand(() =>
                {
                    if (_executor != null)
                    {
                        _executor.EndExecute();
                    }    
                });

            OpenSqlConnectionBuilderCommand = new DelegateCommand(() =>
            {
                ConnectionStringCreatorGUI.SqlConnectionString initialConnStr;

                try
                {
                    initialConnStr = new ConnectionStringCreatorGUI.SqlConnectionString(ConnectionString);
                }
                catch (Exception)
                {
                    initialConnStr = new ConnectionStringCreatorGUI.SqlConnectionString();
                }

                Window win = new ConnectionStringCreatorGUI.ConnectionStringBuilderWindow(initialConnStr, returnConnBuilder =>
                {
                    ConnectionString = returnConnBuilder.ToString();
                });

                win.Show();

            });
            
            LoadTablesCommand = new DelegateCommand(() =>
                {
                    LoadTables();                        
                });

            OpenValueEditWindowCommand = new DelegateCommand<ColumnEntity>(colEntity =>
                {
                    ColumnEntityValueConfigurationView valueEditView = 
                        new ColumnEntityValueConfigurationView(
                            new ColumnEntityValueConfigurationViewModel(colEntity));
                    valueEditView.Show();
                });

        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

    }
}
