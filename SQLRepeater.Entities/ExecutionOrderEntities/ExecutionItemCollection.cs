using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SQLRepeater.Entities.ExecutionOrderEntities
{
    public class ExecutionItemCollection : ObservableCollection<ExecutionItem>
    {

        public ExecutionItemCollection()
            : base()
        {
            
        }

        string _collectionName;
        public string CollectionName
        {
            get
            {
                return _collectionName;
            }
            set
            {
                if (_collectionName != value)
                {
                    _collectionName = value;
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("CollectionName"));
                }
            }
        }


        object _executionGroupOptions;
        public object ExecutionGroupOptions
        {
            get
            {
                return _executionGroupOptions;
            }
            set
            {
                if (_executionGroupOptions != value)
                {
                    _executionGroupOptions = value;
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("ExecutionGroupOptions"));
                }
            }
        }

    }
}
