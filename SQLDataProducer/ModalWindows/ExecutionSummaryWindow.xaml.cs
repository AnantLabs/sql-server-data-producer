// Copyright 2012-2013 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.


using System.Windows;
using SQLDataProducer.Helpers;

namespace SQLDataProducer.ModalWindows
{
    /// <summary>
    /// Interaction logic for ExecutionSummaryWindow.xaml
    /// </summary>
    public partial class ExecutionSummaryWindow : Window
    {
        private Entities.ExecutionEntities.ExecutionResult _executionResult;
        private Model.ApplicationModel _applicationModel;

        
        public ExecutionSummaryWindow(Entities.ExecutionEntities.ExecutionResult executionResult, Model.ApplicationModel applicationModel)
        {
            this._executionResult = executionResult;
            this._applicationModel = applicationModel;
            InitializeComponent();
            Loaded += new RoutedEventHandler(ExecutionSummaryWindow_Loaded);
        }

        void ExecutionSummaryWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = _executionResult;
        }
       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void showScript_Click(object sender, RoutedEventArgs e)
        {
            if (System.IO.File.Exists(_applicationModel.Options.ScriptOutputScriptName))
            {
                string str = System.IO.File.ReadAllText(_applicationModel.Options.ScriptOutputScriptName);
                NotepadHelper.ShowInNotepad(str);
            }
            else
                MessageBox.Show("Script file not found, probably due to some error during generation");

        }
    }
}
