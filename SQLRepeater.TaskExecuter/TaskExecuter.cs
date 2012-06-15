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
    internal class TaskExecuter
    {
        //private int Counter { get; set; }

        private System.Threading.CancellationTokenSource _cancelTokenSource;
        private string _connectionString;
        public ExecutionTaskOptions Options { get; private set; }

        public TaskExecuter(ExecutionTaskOptions options, string connectionString)
        {
            this._connectionString = connectionString;
            this.Options = options;
            //Counter = 0;
        }

        private System.Threading.CancellationTokenSource CancelTokenSource
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
                try
                {
                    // Generate the final query.
                    string finalResult = GenerateFinalQuery(baseQuery, execItems);

                    if (Options.OnlyOutputToFile)
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
                }
                catch (Exception ex)
                {
                    // TODO: Count the error, save it in some list, and then show it to the user
                    System.IO.File.AppendAllText(@"c:\temp\repeater\log.txt", ex.ToString());
                }
            });
        }

        /// <summary>
        /// Used to set the filenames of the script files.
        /// </summary>
        static SetCounter _setCounter = new SetCounter();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseQuery"></param>
        /// <exception cref="ArgumentNullException">When the Options.ScriptOutputFolder have not been set</exception>
        private void WriteScriptToFile(string baseQuery)
        {
            if (Options.ScriptOutputFolder == null)
                throw new ArgumentNullException("Options.ScriptOutputFolder");

            long c = _setCounter.GetNext();
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
        public int Execute(ExecutionTaskDelegate task)
        {
            
            Action a = null;

            SetCounter setCounter = new SetCounter();
            switch (Options.ExecutionType)
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

            a();
            return setCounter.Peek();

        }

        /// <summary>
        /// Creates an Action that will run the provided task a selected amount of times. 
        /// The singleton ExecutionTaskOptionsManager will be used to get the configuration needed for this function.
        /// </summary>
        /// <param name="task">the task to run.</param>
        /// <returns>the action that will run the task</returns>
        private Action CreateExecutionCountBasedAction(ExecutionTaskDelegate task, SetCounter counter)
        {
            int numThreads = Options.MaxThreads;
            int targetNumExecutions = Options.FixedExecutions;

            Action a = () =>
            {
                // Reset percent done to zero before starting
                Options.PercentCompleted = 0;
                List<BackgroundWorker> workers = new List<BackgroundWorker>();
                for (int i = 0; i < numThreads; i++)
                {
                    workers.Add(new BackgroundWorker());
                    workers[i].DoWork += (sender, e) =>
                    {
                        while (counter.GetNext() <= targetNumExecutions && !CancelTokenSource.IsCancellationRequested)
                        {
                            task();
                            float percentDone = (float)counter.Peek() / (float)Options.FixedExecutions;
                            // TODO: Find out if this is eating to much performance (Sending many OnPropertyChanged events..
                            Options.PercentCompleted = (int)(percentDone * 100);
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
        private Action CreateDurationBasedAction(ExecutionTaskDelegate task, SetCounter counter)
        {
            Action a = () =>
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
                            counter.Increment();
                            float percentDone = ((float)(DateTime.Now.Ticks - beginTime.Ticks) / (float)(until.Ticks - beginTime.Ticks));
                            Options.PercentCompleted = (int)(percentDone * 100);
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

    }
}
