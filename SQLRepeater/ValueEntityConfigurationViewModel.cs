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
            DataGenerationSnippets = GetAppropriateSnippets(colEntity.ColumnDataType);
        }

        private ObservableCollection<ValueCreatorDelegate> GetAppropriateSnippets(string dataType)
        {
            switch (dataType)
            {
                case "int":
                case "smallint":
                case "tinyint":
                    return Snippets.IntSnippets.Snippets;

                case "decimal":
                case "float":
                    return Snippets.DecimalSnippets.Snippets;

                case "datetime":
                case "datetime2":
                    return Snippets.DateTimeSnippets.Snippets;

                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                    return Snippets.StringSnippets.Snippets;

                default:
                    return Snippets.StringSnippets.Snippets;
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

    }
}
