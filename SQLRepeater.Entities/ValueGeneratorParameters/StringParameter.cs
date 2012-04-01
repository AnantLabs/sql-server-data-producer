using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.Entities.ValueGeneratorParameters
{
    public class StringParameter : EntityBase
    {
        int _maxLength;
        public int MaxLength
        {
            get
            {
                return _maxLength;
            }
            set
            {
                if (_maxLength != value)
                {
                    _maxLength = value;
                    OnPropertyChanged("MaxLength");
                }
            }
        }

      
    }
}
