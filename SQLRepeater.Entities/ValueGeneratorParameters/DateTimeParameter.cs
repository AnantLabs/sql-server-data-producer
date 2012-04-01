using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.Entities.ValueGeneratorParameters
{
    public class DateTimeParameter : EntityBase
    {
        private DateTime _startDate;

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged("StartDate");
                }
            }
        }

        DateTime _maxValue;
        public DateTime MaxDate
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
                    OnPropertyChanged("MaxDate");
                }
            }
        }

        DateTime _minValue;
        public DateTime MinDate
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
                    OnPropertyChanged("MinDate");
                }
            }
        }
    }
}
