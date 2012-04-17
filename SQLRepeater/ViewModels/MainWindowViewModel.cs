using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Data.Common;
using SQLRepeater.Entities;
using SQLRepeater.DataAccess;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using SQLRepeater.DatabaseEntities.Entities;
using SQLRepeater.Entities.ExecutionOrderEntities;


namespace SQLRepeater.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public DelegateCommand OpenSqlConnectionBuilderCommand { get; private set; }
        public DelegateCommand LoadTablesCommand { get; private set; }
        
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

        private void LoadTables()
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Model.ConnectionString);
            tda.BeginGetAllTables(res =>
            {
                Model.Tables = res;
                Model.SelectedTable = Model.Tables.FirstOrDefault();
            });
        }

        public MainWindowViewModel()
        {
            Model = new SQLRepeater.Model.ApplicationModel();
            //CurrentExecutionDetailVM = new ExecutionDetailsViewModel(Model, null);
            ExecutionOrderVM = new ExecutionOrderViewModel(Model);

            Model.ExecutionItems = new ObservableCollection<ExecutionItem>();
            //Model.ExecutionItems.Add(new ExecutionItem {  Order = 1, TargetTable = new DatabaseEntities.Entities.TableEntity { TableName = "PetersTable1", TableSchema = "dbo" } });
            //Model.ExecutionItems.Add(new ExecutionItem {  Order = 1, TargetTable = new DatabaseEntities.Entities.TableEntity { TableName = "Ollese", TableSchema = "dbo" } });
            //Model.ExecutionItems.Add(new ExecutionItem {  Order = 1, TargetTable = new DatabaseEntities.Entities.TableEntity { TableName = "Peeele34", TableSchema = "dbo" } });
            //Model.ExecutionItems.Add(new ExecutionItem {  Order = 1, TargetTable = new DatabaseEntities.Entities.TableEntity { TableName = "PeeTable4", TableSchema = "dbo" } });
            //Model.ExecutionItems.Add(new ExecutionItem {  Order = 1, TargetTable = new DatabaseEntities.Entities.TableEntity { TableName = "Pe3e5", TableSchema = "dbo" } });


            OpenSqlConnectionBuilderCommand = new DelegateCommand(() =>
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
                });

                win.Show();

            });
            
            LoadTablesCommand = new DelegateCommand(() =>
                {
                    LoadTables();                        
                });

            //OpenValueEditWindowCommand = new DelegateCommand<ColumnEntity>(colEntity =>
            //    {
            //        ColumnEntityValueConfigurationView valueEditView = 
            //            new ColumnEntityValueConfigurationView(
            //                new ValueEntityConfigurationViewModel(colEntity));
            //        valueEditView.Show();
            //    });

        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

    }
}
