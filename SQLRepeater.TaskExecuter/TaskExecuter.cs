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

namespace SQLRepeater.TaskExecuter
{
    public class TaskExecuter
    {
        private int Counter { get; set; }

        private static System.Threading.CancellationTokenSource _cancelTokenSource;

        private static System.Threading.CancellationTokenSource CancelTokenSource
        {
            get {
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
        /// <param name="connectionString">connection string to use to execute the query</param>
        /// <param name="GenerateParameters">The method to call to generate the final query, the parameters are: The list of ExecutionItems, the original query, the serial number. It will return the final query</param>
        /// <returns></returns>
        public Action<int> CreateSQLTaskForExecutionItems(IEnumerable<ExecutionItem> execItems, string baseQuery, string connectionString, Func<string, int, IEnumerable<ExecutionItem>, string> GenerateFinalQuery)
        {
            return new Action<int>(n =>
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // For each time this Action is called, generate the final query. This will create the "declaration" part of the script with the generated data.
                    // The "base" of the script will be kept from its original, we only generate the actual data here.
                    //IEnumerable<SqlParameter> parms = GenerateParameters(baseQuery, n, execItems);

                    string finalResult = GenerateFinalQuery(baseQuery, n, execItems);

                    using (SqlCommand cmd = new SqlCommand(finalResult, con))
                    {
                        WriteTaskCommandDebug(finalResult, n, null);

                        //cmd.Parameters.AddRange(parms.ToArray());
                        //cmd.Connection.Open();
                        //cmd.ExecuteNonQuery();

                        
                    }
                }
            });
        }

        private static void WriteTaskCommandDebug(string baseQuery, int n, IEnumerable<SqlParameter> parms)
        {
            //Console.WriteLine(finalResult);
            StringBuilder sb = new StringBuilder();
            //foreach (var item in parms)
            //{
            //    sb.AppendFormat("{0} = {1}{2}", item.ParameterName, item.Value, Environment.NewLine);
            //}
            System.IO.File.WriteAllText(string.Format(@"c:\temp\repeater\test{0}.sql", n), baseQuery + sb.ToString());
        }


        /// <summary>
        /// Start executing tasks until suplied datetime, using the supplied number of threads then call the callback once completed.
        /// </summary>
        /// <param name="task">The Action to execute</param>
        /// <param name="until">datetime when the execution should stop</param>
        /// <param name="numThreads">maximum number of threads to use, not guaranteed to use these many treads </param>
        /// <param name="onCompletedCallback">the callback that will be called when execution is done or stopped</param>
        public void BeginExecute(Action<int> task, Action<int> onCompletedCallback)
        {
            Action a = null;

            switch (ExecutionTaskOptionsManager.Instance.Options.ExecutionType)
            {
                case ExecutionTypes.DurationBased:
                    a = CreateDurationBasedAction(task);
                    break;
                case ExecutionTypes.ExecutionCountBased:
                    a = CreateExecutionCountBasedAction(task);
                    break;
                default:
                    break;
            }

            a.BeginInvoke(ar =>
            {
                onCompletedCallback(Counter);
            }, null);

        }

        /// <summary>
        /// Creates an Action that will run the provided task a selected amount of times. 
        /// The singleton ExecutionTaskOptionsManager will be used to get the configuration needed for this function.
        /// </summary>
        /// <param name="task">the task to run.</param>
        /// <returns>the action that will run the task</returns>
        private Action CreateExecutionCountBasedAction(Action<int> task)
        {
            Action a = () =>
            {
                for (int i = 0; i < ExecutionTaskOptionsManager.Instance.Options.FixedExecutions; i++)
                {
                    task(GetNextSerialNumber());
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
        private Action CreateDurationBasedAction(Action<int> task)
        {
            DateTime until = DateTime.Now.AddSeconds(ExecutionTaskOptionsManager.Instance.Options.SecondsToRun);
            int numThreads = ExecutionTaskOptionsManager.Instance.Options.MaxThreads;

            Action a = () =>
            {
                List<Action<int>> actions = new List<Action<int>>();
                for (int i = 0; i < numThreads; i++)
                {
                    actions.Add(task);
                }

                while (DateTime.Now < until && !CancelTokenSource.IsCancellationRequested)
                {
                    Parallel.ForEach(actions, action =>
                    {
                        action(GetNextSerialNumber());
                    });
                }
            };

            return a;
        }

       
        /// <summary>
        /// Get the next number in the sequence to generate data with.
        /// </summary>
        /// <returns>the next number in the sequence</returns>
        private int GetNextSerialNumber()
        {
            return ++Counter;
        }

        /// <summary>
        /// Execute one action one time with the supplied serial number
        /// </summary>
        /// <param name="action">action to execute</param>
        /// <param name="n">the serial number to use</param>
        public void ExecuteOneTime(Action<int> action, int n)
        {
            action(n);
        }
       

        
    }
}
