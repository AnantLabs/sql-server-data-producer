using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Entities.ExecutionEntities
{
    public class ExecutionResult : EntityBase
    {

        public ExecutionResult()
        {
            //StartTime = startTime;
            ErrorList = new List<string>();
            InsertCount = 0;
            //EndTime = startTime;
        }

        public override string ToString()
        {
            return string.Format(@"
Executed Items:                  {0},
Inserted Rows(Approximation):    {1},
Start Time:                      {2},
End Time:                        {3},
Duration:                        {4},
Errors:                          {5}
", ExecutedItemCount, InsertCount, StartTime.ToString(), EndTime.ToString(), Duration.ToString(), ErrorList.Count);
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
                    OnPropertyChanged("InsertCount");
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
                    if (StartTime != null)
                    {
                        Duration = _endTime - StartTime;
                        OnPropertyChanged("Duration");
                    }
                    
                    OnPropertyChanged("EndTime");
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

        long _executedItemCount;
        public long ExecutedItemCount
        {
            get
            {
                return _executedItemCount;
            }
            set
            {
                if (_executedItemCount != value)
                {
                    _executedItemCount = value;
                    OnPropertyChanged("ExecutedItems");
                }
            }
        }
     
    }
}
