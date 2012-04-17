using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SqlClient;
using SQLRepeater.Entities.ExecutionOrderEntities;
using SQLRepeater.DatabaseEntities.Entities;

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

        //public Action<int> CreateSQLTask(string query, Func<int, SqlParameter[]> parameterCreator, string connectionString)
        //{
        //    return new Action<int>( n =>
        //    {
        //        using (SqlConnection con = new SqlConnection(connectionString))
        //        {
        //            using (SqlCommand cmd = new SqlCommand(query, con))
        //            {
        //                cmd.Connection.Open();
        //                cmd.Parameters.AddRange(parameterCreator(n));
        //                cmd.ExecuteNonQuery();
        //            }
        //        }
        //    });
        //}

        public Action<int> CreateSQLTask(string query, string connectionString)
        {
            return new Action<int>(n =>
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            });
        }

        /// <summary>
        /// Creates an Action with insert statements and variables and then executes the query.
        /// </summary>
        /// <param name="execItems">the items that should be included in the execution</param>
        /// <param name="baseQuery">The query containing insert statements for all the items and a placeholder for the variables and their values</param>
        /// <param name="connectionString">connection string to use to execute the query</param>
        /// <returns></returns>
        public Action<int> CreateSQLTaskForExecutionItems(IEnumerable<ExecutionItem> execItems, string baseQuery, string connectionString)
        {
            return new Action<int>(n =>
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string declarations = GenerateVariableDeclarationAndValuesForExecutionItems(n, execItems);

                    // This will contain the insertstatements and the declare variables with values, ready to be executed
                    string finalResult = string.Format(baseQuery, declarations);

                    using (SqlCommand cmd = new SqlCommand(finalResult, con))
                    {
                        //cmd.Connection.Open();
                        //cmd.ExecuteNonQuery();

                        Console.WriteLine(finalResult);
                        System.IO.File.WriteAllText(string.Format(@"c:\temp\test{0}.sql", n), finalResult);
                    }
                }
            });
        }

        /// <summary>
        /// Generate the declaration section of the sqlquery, including the values for the variables
        /// </summary>
        /// <param name="n">The serial number to use when creating the values for the variables</param>
        /// <param name="execItems">the executionItems to be included in the variable declarations</param>
        /// <returns></returns>
        private string GenerateVariableDeclarationAndValuesForExecutionItems(int n, IEnumerable<ExecutionItem> execItems)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var tabl in execItems)
            {
                foreach (ColumnEntity col in tabl.TargetTable.Columns)
                {
                    sb.AppendFormat("DECLARE @i{0}_{1} {2} = {3}", tabl.Order, col.ColumnName, col.ColumnDataType, col.ValueGenerator(n, col.GeneratorParameter));
                    sb.AppendLine();
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Start executing tasks until suplied datetime, using the supplied number of threads then call the callback once completed.
        /// </summary>
        /// <param name="task">The Action to execute</param>
        /// <param name="until">datetime when the execution should stop</param>
        /// <param name="numThreads">maximum number of threads to use, not guaranteed to use these many treads </param>
        /// <param name="onCompletedCallback">the callback that will be called when execution is done or stopped</param>
        public void BeginExecute(Action<int> task, DateTime until, int numThreads, Action<int> onCompletedCallback)
        {
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
                            action(++Counter);
                        });
                    }
                };

            a.BeginInvoke(ar =>
            {
                onCompletedCallback(Counter);
            }, null);

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
