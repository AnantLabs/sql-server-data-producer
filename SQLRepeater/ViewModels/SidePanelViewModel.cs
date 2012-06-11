using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLRepeater.DataAccess;
using SQLRepeater.DatabaseEntities.Entities;
using System.Windows;
using SQLRepeater.EntityQueryGenerator;
using SQLRepeater.Entities.OptionEntities;
using SQLRepeater.Entities;
using SQLRepeater.TaskExecuter;

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

        bool _isQueryRunning = false;
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

           //TaskExecuter.TaskExecuter _executor;

        public SidePanelViewModel(SQLRepeater.Model.ApplicationModel model)
        {
            this.Model = model;
            
            Options = (ExecutionTaskOptions)SQLRepeaterSettings.Default.ExecutionOptions;
            if (Options == null)
                Options = new ExecutionTaskOptions();

            // If any changes are made to the options, then save the configuration
            Options.PropertyChanged += (sender, e) =>
                {
                    SQLRepeaterSettings.Default.ExecutionOptions = Options;
                    SQLRepeaterSettings.Default.Save();
                };

            RunSQLQueryCommand = new DelegateCommand(() =>
            {
                if (Model.ExecutionItems.Count == 0)
                    return;

                WorkflowManager wfm = new WorkflowManager();
                IsQueryRunning = true;
                wfm.RunWorkFlowAsync(Options, Model.ConnectionString, Model.ExecutionItems, (setsInserted) =>
                    {
                        IsQueryRunning = false;
                        MessageBox.Show(setsInserted.ToString());
                    });
            });

            StopExecutionCommand = new DelegateCommand(() =>
            {
                //if (_executor != null)
                //{
                //    _executor.EndExecute();
                //    IsQueryRunning = false;
                //}
            });
        }

      
    }
}
