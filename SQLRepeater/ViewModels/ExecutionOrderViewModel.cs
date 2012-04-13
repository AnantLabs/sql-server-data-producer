using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SQLRepeater.Entities.ExecutionOrderEntities;

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
        }
    }
}
