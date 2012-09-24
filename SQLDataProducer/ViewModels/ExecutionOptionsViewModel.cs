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
using SQLDataProducer.Model;
using System.Windows;
using SQLDataProducer.TaskExecuter;

namespace SQLDataProducer.ViewModels
{
    public class ExecutionOptionsViewModel : ViewModelBase, IYesNoViewModel
    {

        ApplicationModel _model;
        public ApplicationModel Model
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

        public ExecutionOptionsViewModel(ApplicationModel model)
        {
            this.Model = model;
        }

        public void RunExecution()
        {
            if (Model.ExecutionItems.Count == 0)
                return;

            if (string.IsNullOrEmpty(Model.ConnectionString))
                MessageBox.Show("The connection string must be set before executing");

            Model.IsQueryRunning = true;

            Model.WorkFlowManager = new WorkflowManager();
            Model.WorkFlowManager.RunWorkFlowAsync(Model.Options, Model.ConnectionString, Model.ExecutionItems, (setsInserted) =>
            {
                Model.IsQueryRunning = false;
                MessageBox.Show(setsInserted.ToString());
            }
                , string.Empty
                , string.Empty);
        }

        public void OnYes()
        {
            RunExecution();
        }

        public void OnNo()
        {
            
        }
    }
}
