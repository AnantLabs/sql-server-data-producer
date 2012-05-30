using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SQLRepeater.Entities.OptionEntities
{
    public class ExecutionTaskOptions : EntityBase
    {
        public ExecutionTaskOptions()
        {

        }

        bool _onlyOutputToFile = true;
        public bool OnlyOutputToFile
        {
            get
            {
                return _onlyOutputToFile;
            }
            set
            {
                if (_onlyOutputToFile != value)
                {
                    _onlyOutputToFile = value;
                    OnPropertyChanged("OnlyOutputToFile");
                }
            }
        }

        long _startValue = 1;
        public long StartValue
        {
            get
            {
                return _startValue;
            }
            set
            {
                if (_startValue != value)
                {
                    _startValue = value;
                    OnPropertyChanged("StartValue");
                }
            }
        }

        DateTime _dateTimeGenerationStartTime = DateTime.Now;
        public DateTime DateTimeGenerationStartTime
        {
            get
            {
                return _dateTimeGenerationStartTime;
            }
            set
            {
                if (_dateTimeGenerationStartTime != value)
                {
                    _dateTimeGenerationStartTime = value;
                    OnPropertyChanged("DateTimeGenerationStartTime");
                }
            }
        }

        int _fixedExecutions = 10;
        public int FixedExecutions
        {
            get
            {
                return _fixedExecutions;
            }
            set
            {
                if (_fixedExecutions != value)
                {
                    _fixedExecutions = value;
                    OnPropertyChanged("FixedExecutions");
                }
            }
        }

        ExecutionTypes _executionType = ExecutionTypes.DurationBased;
        public ExecutionTypes ExecutionType
        {
            get
            {
                return _executionType;
            }
            set
            {
                if (_executionType != value)
                {
                    _executionType = value;
                    OnPropertyChanged("ExecutionType");
                }
            }
        }
       
        int _secondsToRun = 2;
        public int SecondsToRun
        {
            get
            {
                return _secondsToRun;
            }
            set
            {
                if (_secondsToRun != value)
                {
                    _secondsToRun = value;
                    OnPropertyChanged("SecondsToRun");
                }
            }
        }

        int _maxThreads = 1;
        public int MaxThreads
        {
            get
            {
                return _maxThreads;
            }
            set
            {
                if (_maxThreads != value)
                {
                    _maxThreads = value;
                    OnPropertyChanged("MaxThreads");
                }
            }
        }

    }

    public enum ExecutionTypes
    {
        DurationBased,
        ExecutionCountBased
    } 
}
