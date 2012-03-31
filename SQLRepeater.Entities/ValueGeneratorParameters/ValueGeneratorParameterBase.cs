using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SQLRepeater.Entities.ValueGeneratorParameters
{
    public class ValueGeneratorParameterBase : INotifyPropertyChanged
    {

        public int MyProperty { get; set; }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
