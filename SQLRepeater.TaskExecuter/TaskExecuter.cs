using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SqlClient;

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

        public Action<int> CreateSQLTask(string query, Func<int, SqlParameter[]> parameterCreator, string connectionString)
        {
            return new Action<int>( n =>
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Connection.Open();
                        cmd.Parameters.AddRange(parameterCreator(n));
                        cmd.ExecuteNonQuery();
                    }
                }
            });
        }

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
                            action(Counter);
                            Counter++;
                        });
                    }
                };
            
            a.BeginInvoke(ar =>
            {
                onCompletedCallback(Counter);
            }, null);
            
        }


       

        
    }
}
