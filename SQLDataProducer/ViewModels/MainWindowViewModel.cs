using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Data.Common;
using SQLDataProducer.Entities;
using SQLDataProducer.DataAccess;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using SQLDataProducer.DatabaseEntities.Entities;
using SQLDataProducer.Entities.ExecutionEntities;
using System.Windows.Threading;
using System.Threading;
using SQLDataProducer.Entities.OptionEntities;


namespace SQLDataProducer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public DelegateCommand OpenSqlConnectionBuilderCommand { get; private set; }
        public DelegateCommand LoadTablesCommand { get; private set; }
        
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


        ObservableCollection<ExecutionDetailsViewModel> _executionDetailsVMs;
        public ObservableCollection<ExecutionDetailsViewModel> ExecutionDetailsVMS
        {
            get
            {
                return _executionDetailsVMs;
            }
            set
            {
                if (_executionDetailsVMs != value)
                {
                    _executionDetailsVMs = value;
                    OnPropertyChanged("ExecutionDetailsVMS");
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
            TableEntityDataAccess tda = new TableEntityDataAccess(Model.ConnectionString);
            tda.BeginGetAllTablesAndColumns(res =>
            {
                Model.Tables = res;
                Model.SelectedTable = Model.Tables.FirstOrDefault();
                Model.SelectedColumn = Model.SelectedTable.Columns.FirstOrDefault();

                Application.Current.Dispatcher.Invoke(
                    DispatcherPriority.Normal,
                    new ThreadStart( () =>
                        {
                            Model.ExecutionItems.Clear();
                            Model.SelectedExecutionItem = Model.ExecutionItems.FirstOrDefault();
                        })
                    );
            });
        }

        public MainWindowViewModel(ExecutionTaskOptions options)
        {
            Model = new SQLDataProducer.Model.ApplicationModel();
            Model.ExecutionItems = new ExecutionItemCollection();
            Model.SelectedExecutionItem = Model.ExecutionItems.FirstOrDefault();
            Model.Options = options;
            ExecutionOrderVM = new ExecutionOrderViewModel(Model);
            SidePanelVM = new SidePanelViewModel(Model);


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

    }
}
