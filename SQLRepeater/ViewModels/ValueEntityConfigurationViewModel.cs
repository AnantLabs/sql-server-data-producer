using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using SQLRepeater.Entities;
using System.Collections.ObjectModel;
using SQLRepeater.Controls.GeneratorConfigurationControls;
using SQLRepeater.Entities.ValueGeneratorParameters;
using SQLRepeater.DatabaseEntities.Entities;

namespace SQLRepeater 
{
    public class ValueEntityConfigurationViewModel : INotifyPropertyChanged
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

        ObservableCollection<ValueCreatorDelegate> _dataGenerationGenerators;
        public ObservableCollection<ValueCreatorDelegate> DataGenerationGenerators
        {
            get
            {
                return _dataGenerationGenerators;
            }
            set
            {
                if (_dataGenerationGenerators != value)
                {
                    _dataGenerationGenerators = value;
                    OnPropertyChanged("DataGenerationGenerators");
                }
            }
        }

        private System.Windows.Controls.Control _configurator;
        /// <summary>
        /// The UserControl to be used to configure the generator
        /// </summary>
        public System.Windows.Controls.Control Configurator
        {
            get { return _configurator; }
            set 
            {
                if (_configurator != value)
                {
                    _configurator = value;
                    OnPropertyChanged("Configurator");
                }
                    
            }
        }


        public ValueEntityConfigurationViewModel(ColumnEntity colEntity)
        {
            this.CurrentColumnEntity = colEntity;
            DataGenerationGenerators = Generators.Generatorsupplier.GetGeneratorsForDataType(colEntity.ColumnDataType);
            CurrentColumnEntity.GeneratorParameter = ParameterSupplier.GetGeneratorParameterForDataType(colEntity.ColumnDataType);

            Configurator = ConfigurationViewFactory.GetConfiguratorForColumn(CurrentColumnEntity.GeneratorParameter);
        }

       

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

    }
}
