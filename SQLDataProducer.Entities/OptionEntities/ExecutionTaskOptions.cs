using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using SQLDataProducer.Entities.ExecutionEntities;

namespace SQLDataProducer.Entities.OptionEntities
{
    public class ExecutionTaskOptions : EntityBase
    {
        public ExecutionTaskOptions()
        {

        }

        bool _onlyOutputToFile = true;
        /// <summary>
        /// Get and set wether this execution should only write the scripts to disk instead of running them on the sql server.
        /// </summary>
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

        string _scriptOutputFolder = @"c:\temp\repeater";
        /// <summary>
        /// Get and Set the folder to where the sql script files should be stored when the <see cref="OnlyOutputToFile"/> option is set to True.
        /// </summary>
        public string ScriptOutputFolder
        {
            get
            {
                return _scriptOutputFolder;
            }
            set
            {
                if (_scriptOutputFolder != value)
                {
                    _scriptOutputFolder = value;
                    OnPropertyChanged("ScriptOutputFolder");
                }
            }
        }


        //long _startValue = 1;
        
        //public long StartValue
        //{
        //    get
        //    {
        //        return _startValue;
        //    }
        //    set
        //    {
        //        if (_startValue != value)
        //        {
        //            _startValue = value;
        //            OnPropertyChanged("StartValue");
        //        }
        //    }
        //}

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

        int _fixedExecutions = 10;
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

        int _percentCompleted = 0;
        /// <summary>
        /// The percentage of how far the task have been executed. 
        /// Changing this manually will not change the remaining amount of work in the task and the manual change will be overwritten.
        /// </summary>
        public int PercentCompleted
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
    }

    public enum ExecutionTypes
    {
        DurationBased,
        ExecutionCountBased
    } 
}
