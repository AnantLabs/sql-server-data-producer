// Copyright 2012 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Collections.Generic;
using SQLDataProducer.Entities.DatabaseEntities.Collections;

namespace SQLDataProducer.Entities.ExecutionEntities
{
    public class ExecutionResult : EntityBase
    {

        public ExecutionResult()
        {
            //StartTime = startTime;
            ErrorList = new ErrorList();
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

        ErrorList _errorList;
        public ErrorList ErrorList
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
