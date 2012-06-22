using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Entities.ExecutionEntities
{
    public class ExecutionResult : EntityBase
    {

        public ExecutionResult(DateTime startTime)
        {
            StartTime = startTime;
            ErrorList = new List<string>();
            InsertCount = 0;
            EndTime = startTime;
        }


        long  _insertCount;
        public long InsertCount
        {
            get
            {
                return _insertCount;
            }
            set
            {
                if (_insertCount != value)
                {
                    _insertCount = value;
                    // OnPropertyChanged for this property is called when EndTime is set for performance reasons
                }
            }
        }

        List<string> _errorList;
        public List<string> ErrorList
        {
            get
            {
                return _errorList;
            }
            set
            {
                if (_errorList != value)
                {
                    _errorList = value;
                    OnPropertyChanged("ErrorList");
                }
            }
        }

        DateTime _startTime;
        public DateTime StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    OnPropertyChanged("StartTime");
                }
            }
        }

        DateTime _endTime;
        public DateTime EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                if (_endTime != value)
                {
                    _endTime = value;
                    Duration = StartTime - _endTime;
                    OnPropertyChanged("EndTime");
                    OnPropertyChanged("Duration");
                    // In case it affects performance, this should not be called until endTime is set
                    OnPropertyChanged("InsertCount");
                }
            }
        }

        TimeSpan _duration;
        public TimeSpan Duration
        {
            get
            {
                return _duration;
            }
            private set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged("Duration");
                }
            }
        }    

    }
}
