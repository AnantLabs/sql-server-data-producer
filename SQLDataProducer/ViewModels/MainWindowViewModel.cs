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

using System;
using System.Linq;
using System.Windows;
using SQLDataProducer.DataAccess;
using SQLDataProducer.Entities.ExecutionEntities;
using System.Windows.Threading;
using System.Threading;
using SQLDataProducer.Entities.OptionEntities;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.DataAccess.Factories;
using System.ComponentModel;
using SQLDataProducer.Entities.DatabaseEntities.Collections;
using SQLDataProducer.Helpers;


namespace SQLDataProducer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public DelegateCommand OpenSqlConnectionBuilderCommand { get; private set; }
        public DelegateCommand LoadTablesCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand LoadCommand { get; private set; }
        public DelegateCommand RunCommand { get; private set; }

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

        ExecutionOrderViewModel _executionOrderVM;
        public ExecutionOrderViewModel ExecutionOrderVM
        {
            get
            {
                return _executionOrderVM;
            }
            set
            {
                if (_executionOrderVM != value)
                {
                    _executionOrderVM = value;
                    OnPropertyChanged("ExecutionOrderVM");
                }
            }
        }

        ExecutionDetailsViewModel _executionDetailsVM;
        public ExecutionDetailsViewModel ExecutionDetailsVM
        {
            get
            {
                return _executionDetailsVM;
            }
            set
            {
                if (_executionDetailsVM != value)
                {
                    _executionDetailsVM = value;
                    OnPropertyChanged("ExecutionDetailsVM");
                }
            }
        }


        SidePanelViewModel _sidepanelVM;
        public SidePanelViewModel SidePanelVM
        {
            get
            {
                return _sidepanelVM;
            }
            set
            {
                if (_sidepanelVM != value)
                {
                    _sidepanelVM = value;
                    OnPropertyChanged("SidePanelVM");
                }
            }
        }



        private void LoadTables()
        {
            Model.ExecutionItems.Clear();
            // TODO: Also clear caches
            Model.IsQueryRunning = true;
            
            TableEntityCollection tables = null;

            Action a = new Action(() =>
                {
                    using (var tda = new TableEntityDataAccess(Model.ConnectionString))
                    {
                        tables = tda.GetAllTablesAndColumns();
                    }

                    DispatcherSupplier.CurrentDispatcher.Invoke(
                       DispatcherPriority.Normal,
                       (Action)delegate
                       {
                           Model.Tables = tables;
                           Model.SelectedTable = Model.Tables.FirstOrDefault();
                           Model.SelectedColumn = Model.SelectedTable.Columns.FirstOrDefault();

                           Model.SetTablesView();
                           Model.SelectedExecutionItem = Model.ExecutionItems.FirstOrDefault();
                           Model.IsQueryRunning = false;
                       });  
                });


            a.BeginInvoke(null, null);
        }

        public MainWindowViewModel(ExecutionTaskOptions options)
        {
            Model = new SQLDataProducer.Model.ApplicationModel();
            
            Model.Options = options;
            ExecutionOrderVM = new ExecutionOrderViewModel(Model);
            ExecutionDetailsVM = new ExecutionDetailsViewModel(Model);
            SidePanelVM = new SidePanelViewModel(Model);

            OpenSqlConnectionBuilderCommand = new DelegateCommand(ShowSQLConnectionStringBuilder);
            LoadTablesCommand = new DelegateCommand(LoadTables);
            SaveCommand = new DelegateCommand(SaveExecutionListToFile);
            LoadCommand = new DelegateCommand(LoadExecutionListFromFile);

            RunCommand = new DelegateCommand(RunExecution);
        }


        private void RunExecution()
        {
            Model.RunExecution();
        }

        private void LoadExecutionListFromFile()
        {

            //if (string.IsNullOrEmpty(Model.ConnectionString))
            //{
            //    MessageBox.Show("The connection string must be set before loading");
            //    return;
            //}


            //System.Windows.Forms.OpenFileDialog dia = new System.Windows.Forms.OpenFileDialog();
            //dia.Filter = "Saved Files (*.xml)|*.xml";
            //dia.Multiselect = false;
            //dia.AddExtension = true;
            //dia.CheckFileExists = true;
            //dia.DefaultExt = ".xml";
            ////dia.InitialDirectory = ""; // TODO: Remember the last path

            //System.Windows.Forms.DialogResult diaResult = dia.ShowDialog();
            //if (diaResult != System.Windows.Forms.DialogResult.Cancel)
            //{
            //    string fileName = dia.FileName;
            //    //ExecutionItemManager m = new ExecutionItemManager(Model.ConnectionString);
            //    var loaded = ExecutionItemManager.Load(fileName, new TableEntityDataAccess(Model.ConnectionString));
            //    Model.ExecutionItems.Clear();
            //    foreach (var item in loaded)
            //    {
            //        Model.ExecutionItems.Add(item);
            //    }
            //    Model.SelectedExecutionItem = Model.ExecutionItems.FirstOrDefault();
            //}
            throw new NotImplementedException("Load and save");
        }

        private void SaveExecutionListToFile()
        {
            //if (Model.ExecutionItems.Count > 0)
            //{
            //    System.Windows.Forms.SaveFileDialog dia = new System.Windows.Forms.SaveFileDialog();
            //    dia.Filter = "Saved Files (*.xml)|*.xml";
            //    dia.AddExtension = true;
            //    dia.DefaultExt = ".xml";

            //    System.Windows.Forms.DialogResult diaResult = dia.ShowDialog();
            //    if (diaResult != System.Windows.Forms.DialogResult.Cancel)
            //    {
            //        string fileName = dia.FileName;
            //        ExecutionItemManager.Save(Model.ExecutionItems, fileName);
            //    }
            //}
            throw new NotImplementedException("Load and save");
        }

        private void ShowSQLConnectionStringBuilder()
        {
            ConnectionStringCreatorGUI.SqlConnectionString initialConnStr;

            try
            {
                initialConnStr = new ConnectionStringCreatorGUI.SqlConnectionString(Model.ConnectionString);
            }
            catch (Exception)
            {
                initialConnStr = new ConnectionStringCreatorGUI.SqlConnectionString();
            }

            Window win = new ConnectionStringCreatorGUI.ConnectionStringBuilderWindow(initialConnStr, returnConnBuilder =>
            {
                Model.ConnectionString = returnConnBuilder.ToString();
                try
                {
                    AdhocDataAccess adhd = new AdhocDataAccess(Model.ConnectionString);
                    
                    adhd.TestConnection();
                }
                catch (Exception e)
                {
                    MessageBox.Show("The connection is not valid, Check Server, Database and credentials: " + Environment.NewLine + Environment.NewLine + e.Message);
                }
            });

            win.Show();
        }



    }
}
