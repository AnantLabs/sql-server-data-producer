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
using System.Linq;
using System.Data.SqlClient;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.Entities.OptionEntities;
using SQLDataProducer.Entities;
using System.ComponentModel;
using System.Threading;
using System.IO;
using SQLDataProducer.ContinuousInsertion;

namespace SQLDataProducer.TaskExecuter
{
    internal class TaskExecuter
    {
        private System.Threading.CancellationTokenSource _cancelTokenSource;
        
        //SetCounter _NSetCounter = new SetCounter();
        //SetCounter _insertCounter = new SetCounter();
        List<string> _errorMessages = new List<string>();

        SetCounter _executionCounter = new SetCounter();
        SetCounter _rowInsertCounter = new SetCounter();
        Func<long> _nGenerator;

        ExecutionTaskOptions Options { get; set; }

        private string _connectionString;

        /// <summary>
        /// Used to lock writing to the logfile.
        /// </summary>
        private object _logFileLockObjs = new object();

        private bool _doneMyWork = false;

        public TaskExecuter(ExecutionTaskOptions options, string connectionString)
        {
            _doneMyWork = false;
            _connectionString = connectionString;
            Options = options;
            _cancelTokenSource = new CancellationTokenSource();
            
            // Create the method to be used to generate the N values.
            _nGenerator = delegate
            {
                switch (Options.NumberGeneratorMethod)
                {
                    case NumberGeneratorMethods.NewNForEachExecution:
                        // The executionCounter is incremented for each Execution, just return the current value. It will be incremented in the big loop
                        return _executionCounter.Peek();
                    case NumberGeneratorMethods.NewNForEachRow:
                        // Insert counter is used to generated per row, it should be incremented by "something else" for each row that is inserted
                        return _rowInsertCounter.Peek();
                    case NumberGeneratorMethods.ConstantN:
                        return 1;
                    default:
                        return 1;
                }
            };
        }

      
        private System.Threading.CancellationTokenSource CancelTokenSource
        {
            get
            {
                return _cancelTokenSource;
            }
            set { _cancelTokenSource = value; }
        }

        public void EndExecute()
        {
            _doneMyWork = true;
            CancelTokenSource.Cancel();
        }


        /// <summary>
        /// Creates an Action with insert statements and variables and then executes the query.
        /// </summary>
        /// <param name="execItems">the items that should be included in the execution</param>
        /// <param name="baseQuery">The query containing insert statements for all the items and a placeholder for the variables and their values</param>
        /// <param name="GenerateFinalQuery">The FinalQueryGeneratorDelegate that will be run to create the final query. The final query will include the actuall values</param>
        /// <returns></returns>
        public ExecutionTaskDelegate CreateSQLTaskForExecutionItems(ExecutionItemCollection execItems)
        {
            ContinuousInsertionManager manager = new ContinuousInsertionManager(_connectionString);

            return new ExecutionTaskDelegate(() =>
            {
                try
                {
                    

                    if (!Options.OnlyOutputToFile)
                    {
                        manager.DoOneExecution(execItems, _nGenerator, _rowInsertCounter, _executionCounter.Peek());
                        
                    }
                    else
                    {
                        string finalResult = manager.OneExecutionToString(execItems, _nGenerator, _rowInsertCounter);
                        WriteScriptToFile(finalResult);
                    }
                    
                }
                catch (Exception ex)
                {
                    // TODO: Count the error, save it in some list, and then show it to the user
                    lock (_logFileLockObjs)
                    {
                        _errorMessages.Add(ex.ToString());
                        System.IO.File.AppendAllText("log.txt", ex.ToString());
                    }
                }
            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseQuery"></param>
        /// <exception cref="ArgumentNullException">When the Options.ScriptOutputFolder have not been set</exception>
        private void WriteScriptToFile(string baseQuery)
        {
            if (Options.ScriptOutputFolder == null)
                throw new ArgumentNullException("Options.ScriptOutputFolder");

            long c = _executionCounter.Peek();
            string dir = Options.ScriptOutputFolder;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (!dir.EndsWith("\\"))
            {
                dir = dir + "\\";
            }
            System.IO.File.WriteAllText(string.Format("{1}GeneratedScript_{0}.sql", c, dir), baseQuery);
        }


        /// <summary>
        /// Start executing tasks until suplied datetime, using the supplied number of threads then call the callback once completed.
        /// </summary>
        /// <param name="task">The Action to execute</param>
        /// <param name="until">datetime when the execution should stop</param>
        /// <param name="numThreads">maximum number of threads to use, not guaranteed to use these many treads </param>
        /// <param name="onCompletedCallback">the callback that will be called when execution is done or stopped</param>
        public ExecutionResult Execute(ExecutionTaskDelegate task)
        {
            // Lazy fix to avoid having to clean up and reset everything.
            if (_doneMyWork)
                throw new NotSupportedException("This TaskExecuter have already been used. It may not be used again. Create a new one and try again");
            
            ExecutionResult result = new ExecutionResult();
            try
            {
                switch (Options.ExecutionType)
                {
                    case ExecutionTypes.DurationBased:
                        RunTaskDurationBased(task);
                        break;
                    case ExecutionTypes.ExecutionCountBased:
                        RunTaskExecutionCountBased(task);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                result.ErrorList.Add(e.ToString());
            }
            finally
            {
                result.ErrorList.AddRange(_errorMessages);
                result.InsertCount = _rowInsertCounter.Peek();
                result.ExecutedItemCount = _executionCounter.Peek();
                _doneMyWork = true;
            }
            
            
            return result;

        }

        /// <summary>
        /// Creates an Action that will run the provided task a selected amount of times. 
        /// </summary>
        /// <param name="task">the task to run.</param>
        /// <returns>the action that will run the task</returns>
        private void RunTaskExecutionCountBased(ExecutionTaskDelegate task)
        {
            int numThreads = Options.MaxThreads;
            int targetNumExecutions = Options.FixedExecutions;

            // Reset percent done to zero before starting
            Options.PercentCompleted = 0;
            List<BackgroundWorker> workers = new List<BackgroundWorker>();
            for (int i = 0; i < numThreads; i++)
            {
                workers.Add(new BackgroundWorker());
                workers[i].DoWork += (sender, e) =>
                {
                    while (_executionCounter.Peek() < targetNumExecutions && !CancelTokenSource.IsCancellationRequested)
                    {
                        _executionCounter.Increment();
                        task();
                        //_insertCounter.Peek();
                        float percentDone = (float)_executionCounter.Peek() / (float)Options.FixedExecutions;
                        // TODO: Find out if this is eating to much performance (Sending many OnPropertyChanged events..
                        Options.PercentCompleted = (int)(percentDone * 100);
                    }
                };
                workers[i].RunWorkerAsync();
            }
            // TODO: This does not feel optimal
            while (workers.Any(x => x.IsBusy))
            {
                Thread.Sleep(100);
            }

        }

        /// <summary>
        /// Creates an Action that will run the provided task until a configured DateTime.
        /// </summary>
        /// <param name="task">the task to run</param>
        /// <returns>the action that will run the task</returns>
        /// <param name="counter"></param>
        private void RunTaskDurationBased(ExecutionTaskDelegate task)
        {
            DateTime beginTime = new DateTime(DateTime.Now.Ticks);
            DateTime until = DateTime.Now.AddSeconds(Options.SecondsToRun);

            int numThreads = Options.MaxThreads;
            // Reset percent done to zero before starting
            Options.PercentCompleted = 0;

            List<BackgroundWorker> workers = new List<BackgroundWorker>();
            for (int i = 0; i < numThreads; i++)
            {
                workers.Add(new BackgroundWorker());
                workers[i].DoWork += (sender, e) =>
                {
                    while (DateTime.Now < until && !CancelTokenSource.IsCancellationRequested)
                    {
                        task();
                        //_insertCounter.Peek();
                        _executionCounter.Increment();
                        float percentDone = ((float)(DateTime.Now.Ticks - beginTime.Ticks) / (float)(until.Ticks - beginTime.Ticks));
                        Options.PercentCompleted = (int)(percentDone * 100);
                    }
                };
                workers[i].RunWorkerAsync();
            }
            
            // TODO: This does not feel optimal
            while (workers.Any(x => x.IsBusy))
            {
                Thread.Sleep(100);
            }
        }

    }
}
