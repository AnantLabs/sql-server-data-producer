using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDataProducer.EntityQueryGenerator;
using SQLDataProducer.TaskExecuter;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.Entities;
using SQLDataProducer.Entities.OptionEntities;
using SQLDataProducer.DataAccess;
using SQLDataProducer.Entities.Generators;

namespace SQLDataProducer.TaskExecuter
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
            return InternalRunWorkFlow(options, connectionString, executionItems, preScript, postScript);
        }
        public void RunWorkFlowAsync(ExecutionTaskOptions options, string connectionString, ExecutionItemCollection executionItems, Action<int> onCompletedCallback, string preScript = null, string postScript = null)
        {
            Action a = new Action(() =>
                {
                    int res = InternalRunWorkFlow(options, connectionString, executionItems, preScript, postScript);
                    onCompletedCallback(res);
                });
            a.BeginInvoke(null, null);
        }

        private int InternalRunWorkFlow(ExecutionTaskOptions options, string connectionString, ExecutionItemCollection executionItems, string preScript = null, string postScript = null)
        {
            Executor = new TaskExecuter(options, connectionString);
            RunTruncationOnExecutionItems(connectionString, executionItems);

            RunPrepare(connectionString, preScript);
            int result = Execute(connectionString, executionItems);
            RunPostScript(connectionString, postScript);
            return result;
        }

        private void RunTruncationOnExecutionItems(string connectionString, ExecutionItemCollection executionItems)
        {
            AdhocDataAccess ahd = new AdhocDataAccess(connectionString);
            foreach (var item in executionItems.Where(x => x.TruncateBeforeExecution).Select(x => x.TargetTable).Distinct())
            {
                string sql = string.Format("DELETE {0}.{1};", item.TableSchema, item.TableName);
                ahd.ExecuteNonQuery(sql);
            }
        }

        private void RunPostScript(string connectionString, string postScript)
        {
            if (string.IsNullOrEmpty(postScript))
                return;
            AdhocDataAccess adhd = new AdhocDataAccess(connectionString);
            adhd.ExecuteNonQuery(postScript);
        }

        private void RunPrepare(string connectionString, string preScript)
        {
            if (string.IsNullOrEmpty(preScript))
                return;
            AdhocDataAccess adhd = new AdhocDataAccess(connectionString);
            adhd.ExecuteNonQuery(preScript);
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
