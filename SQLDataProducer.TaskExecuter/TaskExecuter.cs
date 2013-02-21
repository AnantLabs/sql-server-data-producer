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
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.Entities.OptionEntities;
using SQLDataProducer.Entities;
using System.ComponentModel;
using System.Threading;
using System.IO;
//using SQLDataProducer.ContinuousInsertion;
using SQLDataProducer.Entities.DatabaseEntities.Collections;
using SQLDataProducer.DataConsumers;

namespace SQLDataProducer.TaskExecuter
{
    public class TaskExecuter : IDisposable
    {
        private System.Threading.CancellationTokenSource _cancelTokenSource;

        ErrorList _errorMessages = new ErrorList(20);

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
        ExecutionItemCollection _execItems;
        private IDataConsumer _consumer;
        private bool _isInitialized = false;

        public TaskExecuter(ExecutionTaskOptions options, string connectionString, ExecutionItemCollection execItems, IDataConsumer consumer)
        {
            _doneMyWork = false;
            _connectionString = connectionString;
            Options = options;
            _cancelTokenSource = new CancellationTokenSource();
            _execItems = execItems;
            _consumer = consumer;

            // Create the method to be used to generate the N values.
            _nGenerator = delegate
            {
                switch (Options.NumberGeneratorMethod)
                {
                    case NumberGeneratorMethods.NewNForEachExecution:
                        // The executionCounter is incremented for each Execution, just return the current value. It will be incremented in the big loop
                        Console.WriteLine("_executionCounter.Peek()");
                        return _executionCounter.Peek();
                    case NumberGeneratorMethods.NewNForEachRow:
                        // Insert counter is used to generated per row, it should be incremented by "something else" for each row that is inserted
                        Console.WriteLine("_rowInsertCounter.Peek()");
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


        //private IDataConsumer GetDataConsumer()
        //{
        //    return new DataConsumers.DataToCSVConsumer.DataToCSVConsumer();

        //    //return new ExecutionTaskDelegate( () =>
        //    //{
        //    //    try
        //    //    {
        //    //        if (!Options.OnlyOutputToFile)
        //    //        {
        //    //            throw new NotImplementedException("Fix after refactoring av DataConsumers");
        //    //           // manager.DoOneExecution(_nGenerator, _rowInsertCounter, _executionCounter.Peek());
        //    //        }
        //    //        else
        //    //        {
        //    //            throw new NotImplementedException("Fix after refactoring av DataConsumers");
        //    //            //string finalResult = ContinuousInsertionManager.OneExecutionToString(_execItems, _nGenerator, _rowInsertCounter);
        //    //            //WriteScriptToFile(finalResult);
        //    //        }
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        // TODO: Count the error, save it in some list, and then show it to the user
        //    //        lock (_logFileLockObjs)
        //    //        {
        //    //            _errorMessages.Add(ex.ToString());
        //    //            System.IO.File.AppendAllText("log.txt", ex.ToString());
        //    //        }
        //    //    }
        //    //});
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="baseQuery"></param>
        ///// <exception cref="ArgumentNullException">When the Options.ScriptOutputFolder have not been set</exception>
        //private void WriteScriptToFile(string baseQuery)
        //{
        //    if (Options.ScriptOutputScriptName == null)
        //        throw new ArgumentNullException("Options.ScriptOutputFolder");


        //    System.IO.File.AppendAllText(Options.ScriptOutputScriptName, baseQuery);
        //}


        /// <summary>
        /// Start executing tasks until suplied datetime, using the supplied number of threads then call the callback once completed.
        /// </summary>
        /// <param name="task">The Action to execute</param>
        /// <param name="until">datetime when the execution should stop</param>
        /// <param name="numThreads">maximum number of threads to use, not guaranteed to use these many treads </param>
        /// <param name="onCompletedCallback">the callback that will be called when execution is done or stopped</param>
        public ExecutionResult Execute()
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Method cannot be run before Init have been called");

            // Lazy fix to avoid having to clean up and reset everything.
            if (_doneMyWork)
                throw new NotSupportedException("This TaskExecuter have already been used. It may not be used again. Create a new one and try again");

            SQLDataProducer.Entities.Generators.Generator.InitGeneratorStartValues(Options);

            ExecutionResult result = new ExecutionResult();

            try
            {
                switch (Options.ExecutionType)
                {
                    case ExecutionTypes.DurationBased:
                        RunTaskDurationBased();
                        break;
                    case ExecutionTypes.ExecutionCountBased:
                        RunTaskExecutionCountBased();
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
        /// <param name="consumer">the task to run.</param>
        /// <returns>the action that will run the task</returns>
        private void RunTaskExecutionCountBased()
        {
            // Reset percent done to zero before starting
            Options.PercentCompleted = 0;
            List<BackgroundWorker> workers = new List<BackgroundWorker>();
            for (int i = 0; i < Options.MaxThreads; i++)
            {

                workers.Add(new BackgroundWorker());
                workers[i].DoWork += (sender, e) =>
                {
                    while (_executionCounter.Peek() < Options.FixedExecutions && !CancelTokenSource.IsCancellationRequested)
                    {

                        RunOneExecution();

                        float percentDone = (float)_executionCounter.Peek() / (float)Options.FixedExecutions;
                        // TODO: Find out if this is eating to much performance (Sending many OnPropertyChanged events..
                        Options.PercentCompleted = percentDone;
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

        private void RunOneExecution()
        {
            foreach (var ei in _execItems)
            {
                var data = ei.CreateData(_nGenerator, _rowInsertCounter);
                _consumer.Consume(data, ei.TargetTable.TableName);
            }
            _executionCounter.Increment();
        }

        /// <summary>
        /// Creates an Action that will run the provided task until a configured DateTime.
        /// </summary>
        /// <param name="task">the task to run</param>
        /// <returns>the action that will run the task</returns>
        /// <param name="counter"></param>
        private void RunTaskDurationBased()
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
                        RunOneExecution();

                        //foreach (var ei in _execItems)
                        //{
                        //    var data = ei.CreateData(_nGenerator, _rowInsertCounter);
                        //    _consumer.Consume(data, ei.TargetTable.TableName);
                        //}
                        //_executionCounter.Increment();
                        
                        float percentDone = ((float)(DateTime.Now.Ticks - beginTime.Ticks) / (float)(until.Ticks - beginTime.Ticks));
                        Options.PercentCompleted = percentDone;
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

        public void Dispose()
        {
            this._cancelTokenSource.Dispose();
        }

        internal void CleanUp()
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Method cannot be run before Init have been called");

            _consumer.CleanUp(_execItems.Select(e => e.TargetTable.TableName).ToList());
        }

        internal void PreAction(string preScript)
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Method cannot be run before Init have been called");

            _consumer.PreAction(preScript);
        }

        internal void PostAction(string postScript)
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Method cannot be run before Init have been called");

            _consumer.PostAction(postScript);
        }

        internal void Init()
        {
            _consumer.Init(_connectionString);
            _isInitialized = true;
        }
    }
}
