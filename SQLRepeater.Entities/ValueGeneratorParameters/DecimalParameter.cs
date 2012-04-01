using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.Entities.ValueGeneratorParameters
{
    public class DecimalParameter : EntityBase
    {

        private int _decimals;
        public int Decimals
        {
            get { return _decimals; }
            set
            {
                if (_decimals != value)
                {
                    _decimals = value;
                    OnPropertyChanged("Decimals");
                }
            }
        }


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
    }
}
