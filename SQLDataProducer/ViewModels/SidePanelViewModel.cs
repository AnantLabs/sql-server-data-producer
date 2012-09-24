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
using SQLDataProducer.ModalWindows;

namespace SQLDataProducer.ViewModels
{
    public class SidePanelViewModel : ViewModelBase
    {
        public DelegateCommand ConfigureAndRunExecutionCommand { get; private set; }
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




        public SidePanelViewModel(SQLDataProducer.Model.ApplicationModel model)
        {
            this.Model = model;

            ConfigureAndRunExecutionCommand = new DelegateCommand(ShowConfigureAndRunWindow);
            StopExecutionCommand = new DelegateCommand(StopRunningExecution);
            EditPostScriptCommand = new DelegateCommand(ShowWindowToEditPostScript);
            EditPreScriptCommand = new DelegateCommand(ShowWindoToEditPreScript);
        }

        private void ShowWindoToEditPreScript()
        {
            Window win = new Window();
            win.Title = "Edit Pre Script. Close window when done";
            win.Content = new ScriptEditView(Model);
            win.Show();
        }

        private void ShowWindowToEditPostScript()
        {
            Window win = new Window();
            win.Title = "Edit Post Script. Close window when done";
            win.Content = new ScriptEditView(Model);
            win.Show();
        }

        private void StopRunningExecution()
        {
            if (Model.WorkFlowManager != null)
            {
                Model.WorkFlowManager.StopAsync();
                Model.IsQueryRunning = false;
            }
        }

        private void ShowConfigureAndRunWindow()
        {
            ExecutionOptionsViewModel opVM = new ExecutionOptionsViewModel(Model);
            ExecutionsOptionsView view = new ExecutionsOptionsView(opVM);
            YesNoWindow win = new YesNoWindow(view, opVM);
            win.ShowDialog();
        }
    }
}
