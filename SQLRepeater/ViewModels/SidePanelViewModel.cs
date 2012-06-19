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
using SQLRepeater.Views;

namespace SQLRepeater.ViewModels
{
    public class SidePanelViewModel : ViewModelBase
    {
        public DelegateCommand RunSQLQueryCommand { get; private set; }
        public DelegateCommand StopExecutionCommand { get; private set; }
        public DelegateCommand EditPreScriptCommand { get; private set; }
        public DelegateCommand EditPostScriptCommand { get; private set; }
            
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



        ScriptEditViewModel _postScriptVM;
        public ScriptEditViewModel PostScriptViewModel
        {
            get
            {
                return _postScriptVM;
            }
            set
            {
                if (_postScriptVM != value)
                {
                    _postScriptVM = value;
                    OnPropertyChanged("PostScriptViewModel");
                }
            }
        }

        ScriptEditViewModel _preScriptVM;
        public ScriptEditViewModel PreScriptViewModel
        {
            get
            {
                return _preScriptVM;
            }
            set
            {
                if (_preScriptVM != value)
                {
                    _preScriptVM = value;
                    OnPropertyChanged("PreScriptViewModel");
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
            PostScriptViewModel = new ScriptEditViewModel();
            PreScriptViewModel = new ScriptEditViewModel();

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
                    }
                    , PreScriptViewModel.ScriptText
                    , PostScriptViewModel.ScriptText);
            });

            StopExecutionCommand = new DelegateCommand(() =>
            {
                if (_wfm != null)
                {
                    _wfm.StopAsync();
                    IsQueryRunning = false;
                }
            });

            EditPostScriptCommand = new DelegateCommand(() =>
            {
                Window win = new Window();
                win.Title = "Edit Post Script. Close window when done";
                win.Content = new ScriptEditView(PostScriptViewModel);
                win.Show();
            });
            EditPreScriptCommand = new DelegateCommand(() =>
            {
                Window win = new Window();
                win.Title = "Edit Pre Script. Close window when done";
                win.Content = new ScriptEditView(PreScriptViewModel);
                win.Show();
            });
        }

      
    }
}
