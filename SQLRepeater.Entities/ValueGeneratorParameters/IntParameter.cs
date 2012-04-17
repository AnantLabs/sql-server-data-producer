using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.Entities.ValueGeneratorParameters
{
    public class IntParameter : EntityBase
    {
        int _maxValue;
        public int MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                if (_maxValue != value)
                {
                    _maxValue = value;
                    OnPropertyChanged("MaxValue");
                }
            }
        }

        int _minValue;
        public int MinValue
        {
            get
            {
                return _minValue;
            }
            set
            {
                if (_minValue != value)
                {
                    _minValue = value;
                    OnPropertyChanged("MinValue");
                }
            }
        }


        int _specificValue;
        public int SpecifiedValue
        {
            get
            {
                return _specificValue;
            }
            set
            {
                if (_specificValue != value)
                {
                    _specificValue = value;
                    OnPropertyChanged("SpecifiedValue");
                }
            }
        }

    }
}
