using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SQLRepeater.Entities.ExecutionOrderEntities;
using SQLRepeater.DatabaseEntities.Entities;

namespace SQLRepeater.ViewModels
{
    public class ExecutionOrderViewModel : ViewModelBase
    {

        public DelegateCommand MoveItemUpCommand { get; private set; }
        public DelegateCommand MoveItemLeftCommand { get; private set; }
        public DelegateCommand MoveItemRightCommand { get; private set; }
        public DelegateCommand MoveAllItemsRightCommand { get; private set; }
        public DelegateCommand MoveAllItemsLeftCommand { get; private set; }
        public DelegateCommand MoveItemDownCommand { get; private set; }

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

        ExecutionDetailsViewModel _currentExecutionDetailVM;
        public ExecutionDetailsViewModel SelectedExecutionDetailVM
        {
            get
            {
                return _currentExecutionDetailVM;
            }
            set
            {
                if (_currentExecutionDetailVM != value)
                {
                    _currentExecutionDetailVM = value;
                    OnPropertyChanged("SelectedExecutionDetailVM");
                }
            }
        }


        ObservableCollection<ExecutionDetailsViewModel> _detailsVM;
        public ObservableCollection<ExecutionDetailsViewModel> DetailsVM
        {
            get
            {
                return _detailsVM;
            }
            set
            {
                if (_detailsVM != value)
                {
                    _detailsVM = value;
                    OnPropertyChanged("DetailsVM");
                }
            }
        }
       
        public ExecutionOrderViewModel(SQLRepeater.Model.ApplicationModel model)
        {
            Model = model;

            MoveItemRightCommand = new DelegateCommand(() =>
                {
                    if (Model.SelectedTable != null)
                    {
                        AddExecutionItem(Model.SelectedTable);
                    }
                });

            MoveAllItemsRightCommand = new DelegateCommand(() =>
                {
                    foreach (var tabl in Model.Tables)
                    {
                        AddExecutionItem(tabl);
                    }
                });

            MoveAllItemsLeftCommand = new DelegateCommand(() =>
                {
                    Model.ExecutionItems.Clear();
                });
            
            MoveItemLeftCommand = new DelegateCommand(() =>
                {
                    if (Model.SelectedExecutionItem != null)
                    {
                        Model.ExecutionItems.Remove(Model.SelectedExecutionItem);
                        for (int i = 0; i < Model.ExecutionItems.Count; i++)
                        {
                            Model.ExecutionItems[i].Order = i + 1;
                        }
                    }
                });
        }

        private void AddExecutionItem(TableEntity table)
        {
            Model.ExecutionItems.Add(new ExecutionItem(table, Model.ExecutionItems.Count + 1));
        }
    }
}
