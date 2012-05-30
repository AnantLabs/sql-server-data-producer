using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLRepeater.DataAccess;
using SQLRepeater.DatabaseEntities.Entities;
using System.Windows;
using SQLRepeater.EntityQueryGenerator;
using SQLRepeater.Entities.OptionEntities;

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



        ExecutionTaskOptions _options;
        public ExecutionTaskOptions Options
        {
            get
            {
                return _options;
            }
            set
            {
                if (_options != value)
                {
                    _options = value;
                    OnPropertyChanged("Options");
                }
            }
        }

        //  int _taskProgPercent;
        //public int TaskProgressPercentage
        //{
        //    get
        //    {
        //        return _taskProgPercent;
        //    }
        //    set
        //    {
        //        if (_taskProgPercent != value)
        //        {
        //            _taskProgPercent = value;
        //            OnPropertyChanged("TaskProgressPercentage");
        //        }
        //    }
        //}

 


        TaskExecuter.TaskExecuter _executor;

        public SidePanelViewModel(SQLRepeater.Model.ApplicationModel model)
        {
            this.Model = model;
            _executor = new TaskExecuter.TaskExecuter();
            Options = ExecutionTaskOptions.Instance;

            RunSQLQueryCommand = new DelegateCommand(() =>
            {
                if (Model.ExecutionItems.Count == 0)
                    return;

                EntityQueryGenerator.InsertQueryGenerator ig =
                    new EntityQueryGenerator.InsertQueryGenerator();

                InsertQueryGenerator queryGenerator = new InsertQueryGenerator();
                string baseQuery = queryGenerator.GenerateQueryForExecutionItems(Model.ExecutionItems);

                Action<int> taskToExecute = _executor.CreateSQLTaskForExecutionItems(
                    // The items to generate data for
                    Model.ExecutionItems, 
                    // The basequery containing all the insert statements
                    baseQuery, 
                    // The connection string to use
                    Model.ConnectionString, 
                    // The function to call to generate the final declare @.. statements for each iteration
                    queryGenerator.GenerateFinalQuery);

                IsQueryRunning = true;
               _executor.BeginExecute(taskToExecute, count =>
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
        }

      
    }
}
