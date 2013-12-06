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
using SQLDataProducer.Entities.DataConsumers;

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
        ExecutionNode _execItems;
        private IDataConsumer _consumer;
        private bool _isInitialized = false;
        private Dictionary<string, string> _consumerOptions;

        public TaskExecuter(ExecutionTaskOptions options, string connectionString, ExecutionNode execItems, IDataConsumer consumer, Dictionary<string, string> consumerOptions = null)
        {
            _doneMyWork = false;
            _connectionString = connectionString;
            Options = options;
            _cancelTokenSource = new CancellationTokenSource();
            _execItems = execItems;
            _consumer = consumer;
            _consumerOptions = consumerOptions;

            // Create the method to be used to generate the N values.
            _nGenerator = delegate
            {
                switch (Options.NumberGeneratorMethod)
                {
                    case NumberGeneratorMethods.NewNForEachExecution:
                        // The executionCounter is incremented for each Execution, just return the current value. It will be incremented in the big loop
                        System.Diagnostics.Debug.WriteLine("N generation called: _executionCounter.Peek()");
                        return _executionCounter.Peek();
                    case NumberGeneratorMethods.NewNForEachRow:
                        // Insert counter is used to generated per row, it should be incremented by "something else" for each row that is inserted
                        System.Diagnostics.Debug.WriteLine("N generation called: _rowInsertCounter.Peek()");
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

        public ExecutionResult Execute()
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Method cannot be run before Init have been called");

            // Lazy fix to avoid having to clean up and reset everything.
            if (_doneMyWork)
                throw new NotSupportedException("This TaskExecuter have already been used. It may not be used again. Create a new one and try again");

            //SQLDataProducer.Entities.Generators.Generator.InitGeneratorStartValues(Options);

            //ExecutionResult result = new ExecutionResult();

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
               // result.ErrorList.Add(e.ToString());
            }
            finally
            {
                //result.ErrorList.AddRange(_errorMessages);
                //result.InsertCount = _rowInsertCounter.Peek();
                //result.TablesTouched = _executionCounter.Peek();
                _doneMyWork = true;
            }

            return null;// result;

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
                        // TODO: Testing if this would change the value only every 5 percent.
                        if (percentDone % 5 == 0)
                        {
                            Options.PercentCompleted = percentDone;
                        }
                    }

                };
                workers[i].RunWorkerAsync();
                
            }
            // TODO: This does not feel optimal
            while (workers.Any(x => x.IsBusy))
            {
                Thread.Sleep(100);
            }
            foreach (var bw in workers)
            {
                bw.Dispose();
            }
        }

        private void RunOneExecution()
        {
            throw new NotImplementedException("Running not implemented");
            //foreach (var ei in _execItems)
            //{
            //    var data = ei.CreateData(_nGenerator, _rowInsertCounter);
            //    if (data.Count == 0)
            //        continue;
            //    _consumer.Consume(data);
            //}
            //_executionCounter.Increment();
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

            throw new NotImplementedException("cleanup not implemented");
            //_consumer.CleanUp(_execItems.Select(e => e.TargetTable.TableName).ToList());
        }

        internal void PreAction(string preScript)
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Method cannot be run before Init have been called");

            //_consumer.PreAction(preScript);
        }

        internal void PostAction(string postScript)
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Method cannot be run before Init have been called");

           // _consumer.PostAction(postScript);
        }

        internal void Init()
        {
            _consumer.Init(_connectionString, _consumerOptions);
            _isInitialized = true;
        }
    }
}
