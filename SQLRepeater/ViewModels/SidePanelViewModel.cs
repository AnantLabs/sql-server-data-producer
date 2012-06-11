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

        private WorkflowManager _wfm;

        public SidePanelViewModel(SQLRepeater.Model.ApplicationModel model)
        {
            this.Model = model;
            
            RunSQLQueryCommand = new DelegateCommand(() =>
            {
                if (Model.ExecutionItems.Count == 0)
                    return;

                IsQueryRunning = true;

                _wfm = new WorkflowManager();
                _wfm.RunWorkFlowAsync(Model.Options, Model.ConnectionString, Model.ExecutionItems, (setsInserted) =>
                    {
                        IsQueryRunning = false;
                        MessageBox.Show(setsInserted.ToString());
                    });
            });

            StopExecutionCommand = new DelegateCommand(() =>
            {
                if (_wfm != null)
                {
                    _wfm.StopAsync();
                    IsQueryRunning = false;
                }
            });
        }

      
    }
}
