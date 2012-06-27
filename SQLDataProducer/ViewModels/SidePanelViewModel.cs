// Copyright 2012 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDataProducer.DataAccess;
using SQLDataProducer.DatabaseEntities.Entities;
using System.Windows;
using SQLDataProducer.EntityQueryGenerator;
using SQLDataProducer.Entities.OptionEntities;
using SQLDataProducer.Entities;
using SQLDataProducer.TaskExecuter;
using SQLDataProducer.Views;

namespace SQLDataProducer.ViewModels
{
    public class SidePanelViewModel : ViewModelBase
    {
        public DelegateCommand RunSQLQueryCommand { get; private set; }
        public DelegateCommand StopExecutionCommand { get; private set; }
        public DelegateCommand EditPreScriptCommand { get; private set; }
        public DelegateCommand EditPostScriptCommand { get; private set; }
            
        SQLDataProducer.Model.ApplicationModel _model;
        public SQLDataProducer.Model.ApplicationModel Model
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

       

        private WorkflowManager _wfm;

        public SidePanelViewModel(SQLDataProducer.Model.ApplicationModel model)
        {
            this.Model = model;
            PostScriptViewModel = new ScriptEditViewModel();
            PreScriptViewModel = new ScriptEditViewModel();

            RunSQLQueryCommand = new DelegateCommand(() =>
            {
                if (Model.ExecutionItems.Count == 0)
                    return;
                
                if (string.IsNullOrEmpty(Model.ConnectionString))
                    MessageBox.Show("The connection string must be set before executing");
                

                Model.IsQueryRunning = true;

                _wfm = new WorkflowManager();
                _wfm.RunWorkFlowAsync(Model.Options, Model.ConnectionString, Model.ExecutionItems, (setsInserted) =>
                    {
                        Model.IsQueryRunning = false;
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
                    Model.IsQueryRunning = false;
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
