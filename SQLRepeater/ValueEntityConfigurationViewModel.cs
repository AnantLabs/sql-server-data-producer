using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using SQLRepeater.Entities;
using System.Collections.ObjectModel;

namespace SQLRepeater 
{
    public class ColumnEntityValueConfigurationViewModel : INotifyPropertyChanged
    {
        ColumnEntity _currentColumn;
        public ColumnEntity CurrentColumnEntity
        {
            get
            {
                return _currentColumn;
            }
            set
            {
                if (_currentColumn != value)
                {
                    _currentColumn = value;
                    OnPropertyChanged("CurrentColumnEntity");
                }
            }
        }

        ObservableCollection<ValueCreatorDelegate> _dataGenerationSnippets;
        public ObservableCollection<ValueCreatorDelegate> DataGenerationSnippets
        {
            get
            {
                return _dataGenerationSnippets;
            }
            set
            {
                if (_dataGenerationSnippets != value)
                {
                    _dataGenerationSnippets = value;
                    OnPropertyChanged("DataGenerationSnippets");
                }
            }
        }

        public ColumnEntityValueConfigurationViewModel(ColumnEntity colEntity)
        {
            this.CurrentColumnEntity = colEntity;
            DataGenerationSnippets = Snippets.SnippetSupplier.GetSnippetsForDataType(colEntity.ColumnDataType);
        }

       

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

    }
}
