using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLRepeater.DataAccess;
using SQLRepeater.DatabaseEntities.Entities;
using System.Windows;

namespace SQLRepeater.ViewModels
{
    public class SidePanelViewModel : ViewModelBase
    {
        public DelegateCommand RunSQLQueryCommand { get; private set; }
        public DelegateCommand StopExecutionCommand { get; private set; }

        SQLRepeater.Model.ApplicationModel _model;
        public SQLRepeater.Model.ApplicationModel Model
        {
            get
            {
                return _model;
            }
            set
            {
                if (_model != value)
                {
                    _model = value;
                    OnPropertyChanged("Model");
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
        private SQLRepeater.Model.ApplicationModel Model_2;

        public SidePanelViewModel()
        {
            RunSQLQueryCommand = new DelegateCommand(() =>
            {
                EntityQueryGenerator.InsertQueryGenerator ig =
                    new EntityQueryGenerator.InsertQueryGenerator();

                //string sql = ig.GenerateQueryForExecutionItems(Model.SelectedTable);
                //_executor = new TaskExecuter.TaskExecuter();

                //Action<int> sqlTask = _executor.CreateSQLTask(sql
                //    , Model.SelectedTable.GetParamValueCreator()
                //    , Model.ConnectionString);

                //IsQueryRunning = true;
                //_executor.BeginExecute(sqlTask
                //    , DateTime.Now.AddSeconds(DurationInSeconds)
                //    , NumTasks
                //    , count =>
                //    {
                //        IsQueryRunning = false;
                //        MessageBox.Show(count.ToString());
                //    });
            });

            StopExecutionCommand = new DelegateCommand(() =>
            {
                if (_executor != null)
                {
                    _executor.EndExecute();
                }
            });
        }

        public SidePanelViewModel(SQLRepeater.Model.ApplicationModel Model_2)
        {
            // TODO: Complete member initialization
            this.Model_2 = Model_2;
        }
    }
}
