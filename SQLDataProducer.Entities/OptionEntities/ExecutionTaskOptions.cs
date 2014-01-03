// Copyright 2012-2014 Peter Henell

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

namespace SQLDataProducer.Entities.OptionEntities
{
    public class ExecutionTaskOptions : EntityBase
    {
        public ExecutionTaskOptions()
        {
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
        /// <summary>
        /// Get and Set the datetime that the generators should use as their start dates during value generation
        /// </summary>
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

        int _maxThreads = 1;
        /// <summary>
        /// Set and get the maximum number of threads used in the execution of the task.
        /// </summary>
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
}
