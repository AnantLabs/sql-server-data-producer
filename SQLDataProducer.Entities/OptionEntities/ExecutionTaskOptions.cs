// Copyright 2012-2013 Peter Henell

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

        int _fixedExecutions = 1;
        /// <summary>
        /// Get and Set the number of executions to run when using the Count based execution type.
        /// </summary>
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

        ExecutionTypes _executionType = ExecutionTypes.ExecutionCountBased;
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
        /// <summary>
        /// Set and get the number of seconds that this task should be run when using the DurationBased Execution
        /// </summary>
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

        float _percentCompleted = 0;
        /// <summary>
        /// The percentage of how far the task have been executed. 
        /// Changing this manually will not change the remaining amount of work in the task and the manual change will be overwritten.
        /// </summary>
        public float PercentCompleted
        {
            get
            {
                return _percentCompleted;
            }
            set
            {
                if (_percentCompleted != value)
                {
                    _percentCompleted = value;
                    OnPropertyChanged("PercentCompleted");
                }
            }
        }


        private NumberGeneratorMethods _numberGeneratorMethod = NumberGeneratorMethods.NewNForEachRow;
        /// <summary>
        /// Get set the method to use when generating N for the insertions.
        /// </summary>
        public NumberGeneratorMethods NumberGeneratorMethod
        {
            get
            {
                return _numberGeneratorMethod;
            }
            set
            {
                if (_numberGeneratorMethod != value)
                {
                    _numberGeneratorMethod = value;
                    OnPropertyChanged("NumberGeneratorMethod");
                }
            }
        }
    }
}
