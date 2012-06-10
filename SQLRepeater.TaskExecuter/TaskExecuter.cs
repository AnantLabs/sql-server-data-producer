using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SqlClient;
using SQLRepeater.Entities.ExecutionOrderEntities;
using SQLRepeater.DatabaseEntities.Entities;
using SQLRepeater.Entities.OptionEntities;
using SQLRepeater.Entities;
using SQLRepeater.Entities.Generators;
using System.ComponentModel;
using System.Threading;
using System.IO;

namespace SQLRepeater.TaskExecuter
{
    public class TaskExecuter
    {
        private int Counter { get; set; }

        private static System.Threading.CancellationTokenSource _cancelTokenSource;
        private string _connectionString;

        public TaskExecuter(string connectionString)
        {
            this._connectionString = connectionString;
            Counter = 0;
        }

        private static System.Threading.CancellationTokenSource CancelTokenSource
        {
            get
            {
                if (_cancelTokenSource == null)
                    _cancelTokenSource = new System.Threading.CancellationTokenSource();

                return _cancelTokenSource;
            }
            set { _cancelTokenSource = value; }
        }

        public void EndExecute()
        {
            CancelTokenSource.Cancel();
        }


        /// <summary>
        /// Creates an Action with insert statements and variables and then executes the query.
        /// </summary>
        /// <param name="execItems">the items that should be included in the execution</param>
        /// <param name="baseQuery">The query containing insert statements for all the items and a placeholder for the variables and their values</param>
        /// <param name="GenerateFinalQuery">The FinalQueryGeneratorDelegate that will be run to create the final query. The final query will include the actuall values</param>
        /// <returns></returns>
        public ExecutionTaskDelegate CreateSQLTaskForExecutionItems(IEnumerable<ExecutionItem> execItems, string baseQuery, FinalQueryGeneratorDelegate GenerateFinalQuery)
        {
            return new ExecutionTaskDelegate(() =>
            {
                // Generate the final query.
                string finalResult = GenerateFinalQuery(baseQuery, execItems);

                if (ExecutionTaskOptionsManager.Instance.Options.OnlyOutputToFile)
                    WriteScriptToFile(finalResult);
                else
                {
                    using (SqlConnection con = new SqlConnection(_connectionString))
                    {
                        // For each time this Action is called, generate the final query. This will create the "declaration" part of the script with the generated data.
                        // The "base" of the script will be kept from its original, we only generate the actual data here.
                        using (SqlCommand cmd = new SqlCommand(finalResult, con))
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            });
        }

        static SetCounter _setCounter = new SetCounter();

        private static void WriteScriptToFile(string baseQuery)
        {
            long c = _setCounter.GetNext();
            string dir = ExecutionTaskOptionsManager.Instance.Options.ScriptOutputFolder;
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
        public void BeginExecute(ExecutionTaskDelegate task, ExecutionDoneCallbackDelegate onCompletedCallback)
        {
            Action a = null;

            SetCounter setCounter = new SetCounter();
            switch (ExecutionTaskOptionsManager.Instance.Options.ExecutionType)
            {
                case ExecutionTypes.DurationBased:
                    a = CreateDurationBasedAction(task, setCounter);
                    break;
                case ExecutionTypes.ExecutionCountBased:
                    a = CreateExecutionCountBasedAction(task, setCounter);
                    break;
                default:
                    break;
            }

            a.BeginInvoke(ar =>
            {
                onCompletedCallback(setCounter.Peek());
            }, null);

        }

        /// <summary>
        /// Creates an Action that will run the provided task a selected amount of times. 
        /// The singleton ExecutionTaskOptionsManager will be used to get the configuration needed for this function.
        /// </summary>
        /// <param name="task">the task to run.</param>
        /// <returns>the action that will run the task</returns>
        public Action CreateExecutionCountBasedAction(ExecutionTaskDelegate task, SetCounter counter)
        {
            int numThreads = ExecutionTaskOptionsManager.Instance.Options.MaxThreads;
            int targetNumExecutions = ExecutionTaskOptionsManager.Instance.Options.FixedExecutions;

            Action a = () =>
            {
                List<BackgroundWorker> workers = new List<BackgroundWorker>();
                for (int i = 0; i < numThreads; i++)
                {
                    workers.Add(new BackgroundWorker());
                    workers[i].DoWork += (sender, e) =>
                    {
                        while (counter.Peek() < targetNumExecutions && !CancelTokenSource.IsCancellationRequested)
                        {
                            task();
                            counter.Increment();
                        }
                    };
                    workers[i].RunWorkerAsync();
                }
                while (workers.Any(x => x.IsBusy))
                {
                    Thread.Sleep(100);
                }
            };

            return a;
        }

        /// <summary>
        /// Creates an Action that will run the provided task until a configured DateTime.
        /// The singleton ExecutionTaskOptionsManager will be used to get the configuration needed for this function.
        /// </summary>
        /// <param name="task">the task to run</param>
        /// <returns>the action that will run the task</returns>
        /// <param name="counter"></param>
        public Action CreateDurationBasedAction(ExecutionTaskDelegate task, SetCounter counter)
        {
            DateTime until = DateTime.Now.AddSeconds(ExecutionTaskOptionsManager.Instance.Options.SecondsToRun);
            int numThreads = ExecutionTaskOptionsManager.Instance.Options.MaxThreads;

            Action a = () =>
            {
                List<BackgroundWorker> workers = new List<BackgroundWorker>();
                for (int i = 0; i < numThreads; i++)
                {
                    workers.Add(new BackgroundWorker());
                    workers[i].DoWork += (sender, e) =>
                        {
                            while (DateTime.Now < until && !CancelTokenSource.IsCancellationRequested)
                            {
                                task();
                                counter.Increment();
                            }
                        };
                    workers[i].RunWorkerAsync();
                }
            };

            return a;
        }

       
        ///// <summary>
        ///// Get the next number in the sequence to generate data with.
        ///// </summary>
        ///// <returns>the next number in the sequence</returns>
        //private int GetNextSerialNumber()
        //{
        //    return GenerationNumberSupplier.GetNextNumber();
        //}

        /// <summary>
        /// Execute one action one time with the supplied serial number
        /// </summary>
        /// <param name="action">action to execute</param>
        /// <param name="n">the serial number to use</param>
        public void ExecuteOneTime(ExecutionTaskDelegate action)
        {
            action();
        }
       

        
    }
}
