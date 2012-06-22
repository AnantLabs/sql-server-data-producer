using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SQLDataProducer.Entities.ExecutionEntities;

namespace SQLDataProducer.ViewModels
{
    public class ExecutionDetailsViewModel : ViewModelBase
    {

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


        ExecutionItem _execItem;
        public ExecutionItem ExecutionItem
        {
            get
            {
                return _execItem;
            }
            set
            {
                if (_execItem != value)
                {
                    _execItem = value;
                    OnPropertyChanged("ExecutionItem");
                }
            }
        }

        public ExecutionDetailsViewModel(SQLDataProducer.Model.ApplicationModel model, ExecutionItem execItem)
        {
            Model = model;
        }
    }
}
