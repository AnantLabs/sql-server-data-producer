using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLRepeater.EntityQueryGenerator;
using SQLRepeater.TaskExecuter;
using SQLRepeater.Entities.ExecutionOrderEntities;
using SQLRepeater.Entities;
using SQLRepeater.Entities.OptionEntities;

namespace SQLRepeater.TaskExecuter
{
    public class WorkflowManager
    {
        public WorkflowManager()
        {
        }

        public void StopAsync()
        {
            Executor.EndExecute();
        }

        public int RunWorkFlow(ExecutionTaskOptions options, string connectionString, ExecutionItemCollection executionItems, string preScript = null, string postScript = null)
        {
            Executor = new TaskExecuter(options, connectionString);

            Func<int> a = InitWorkFlowAction(options, connectionString, executionItems);
            return a();
        }
        public void RunWorkFlowAsync(ExecutionTaskOptions options, string connectionString, ExecutionItemCollection executionItems, Action<int> onCompletedCallback, string preScript = null, string postScript = null)
        {
            Action a = new Action(() =>
                {
                    Func<int> f = InitWorkFlowAction(options, connectionString, executionItems);
                    int res = f();
                    onCompletedCallback(res);
                });
            a.BeginInvoke(null, null);
        }

        private Func<int> InitWorkFlowAction(ExecutionTaskOptions options, string connectionString, ExecutionItemCollection executionItems, string preScript = null, string postScript = null)
        {
            Func<int> f = new Func<int>( () =>
            {
                RunPrepare(connectionString, preScript);
                int result = Execute(connectionString, executionItems);
                RunPostScript(connectionString, postScript);
                return result;
            });
            return f;
        }

        private void RunPostScript(string connectionString, string postScript)
        {
            if (postScript == null)
                return;
            throw new NotImplementedException();
        }

        private void RunPrepare(string connectionString, string preScript)
        {
            if (preScript == null)
                return;
            throw new NotImplementedException();
        }

        private int Execute(string connectionString, ExecutionItemCollection executionItems)
        {
            InsertQueryGenerator queryGenerator = new InsertQueryGenerator();
            string basequery = queryGenerator.GenerateQueryForExecutionItems(executionItems);
            ExecutionTaskDelegate taskToExecute = Executor.CreateSQLTaskForExecutionItems(
                // The items to generate data for
               executionItems,
                // The basequery containing all the insert statements
               basequery,
                // The function to call to generate the final VALUES for the insertion
               queryGenerator.GenerateFinalQuery);

            return Executor.Execute(taskToExecute);
        }

        private TaskExecuter _executor;
        private TaskExecuter Executor
        {
            get { return _executor; }
            set { _executor = value; }
        }
    }

    
}
