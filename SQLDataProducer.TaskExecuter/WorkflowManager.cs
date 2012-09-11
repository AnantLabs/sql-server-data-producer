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
using System.Linq;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.Entities;
using SQLDataProducer.Entities.OptionEntities;
using SQLDataProducer.DataAccess;

namespace SQLDataProducer.TaskExecuter
{
    public class WorkflowManager
    {
        public WorkflowManager()
        {
        }

        /// <summary>
        /// Attempt to stop ongoing Async workflow
        /// </summary>
        public void StopAsync()
        {
            if (_executor != null)
            {
                _executor.EndExecute();
            }
        }


        /// <summary>
        /// Run the execution syncronous, blocking the caller.
        /// It will, Truncate tables, Run PreScripts, Run the Execution, Run the PostScript and then assemble the Execution Result.
        /// Truncation of tables, PreScript and PostScript is not part of any explicit transaction.
        /// </summary>
        /// <param name="options">The options to use for this execution</param>
        /// <param name="connectionString">the connectionstring to the target database</param>
        /// <param name="executionItems">list of the execution items that should be processed</param>
        /// <param name="preScript">optional, the script that will run BEFORE everything else</param>
        /// <param name="postScript">optional, the script that will run AFTER everything else</param>
        public ExecutionResult RunWorkFlow(ExecutionTaskOptions options, string connectionString, ExecutionItemCollection executionItems, string preScript = null, string postScript = null)
        {
            return InternalRunWorkFlow(options, connectionString, executionItems, preScript, postScript);
        }
        /// <summary>
        /// Run the execution Async without blocking the caller.
        /// It will, Truncate tables, Run PreScripts, Run the Execution, Run the PostScript and then assemble the Execution Result.
        /// Truncation of tables, PreScript and PostScript is not part of any explicit transaction.
        /// </summary>
        /// <param name="onCompletedCallback">The callback that will be called when the execution is done</param>
        /// <param name="options">The options to use for this execution</param>
        /// <param name="connectionString">the connectionstring to the target database</param>
        /// <param name="executionItems">list of the execution items that should be processed</param>
        /// <param name="preScript">optional, the script that will run BEFORE everything else</param>
        /// <param name="postScript">optional, the script that will run AFTER everything else</param>
        public void RunWorkFlowAsync(ExecutionTaskOptions options, string connectionString, ExecutionItemCollection executionItems, Action<ExecutionResult> onCompletedCallback, string preScript = null, string postScript = null)
        {
            Action a = new Action(() =>
                {
                    ExecutionResult res = InternalRunWorkFlow(options, connectionString, executionItems, preScript, postScript);
                    onCompletedCallback(res);
                });
            a.BeginInvoke(null, null);
        }

        /// <summary>
        /// The main function that will run the execution. It will, Truncate tables, Run PreScripts, Run the Execution, Run the PostScript and then assemble the Execution Result.
        /// </summary>
        /// <param name="options">The options to use for this execution</param>
        /// <param name="connectionString">the connectionstring to the target database</param>
        /// <param name="executionItems">list of the execution items that should be processed</param>
        /// <param name="preScript">optional, the script that will run BEFORE everything else</param>
        /// <param name="postScript">optional, the script that will run AFTER everything else</param>
        /// <returns>An object that describes the result of the execution</returns>
        private ExecutionResult InternalRunWorkFlow(ExecutionTaskOptions options, string connectionString, ExecutionItemCollection executionItems, string preScript = null, string postScript = null)
        {
            DateTime startTime = DateTime.Now;
            _executor = new TaskExecuter(options, connectionString);
            RunTruncationOnExecutionItems(connectionString, executionItems);

            RunPrepare(connectionString, preScript);
            ExecutionResult execResult = Execute(connectionString, executionItems);

            RunPostScript(connectionString, postScript);

            execResult.StartTime = startTime;
            execResult.EndTime = DateTime.Now;
            // Calculate approximation of how many insertions we did
            //execResult.InsertCount = executionItems.Sum(ei => ei.RepeatCount * execResult.ExecutedItemCount);

            return execResult;
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

        private ExecutionResult Execute(string connectionString, ExecutionItemCollection executionItems)
        {
            InsertQueryGenerator queryGenerator = new InsertQueryGenerator();
            
            string basequery = queryGenerator.GenerateQueryForExecutionItems(executionItems);
            ExecutionTaskDelegate taskToExecute = _executor.CreateSQLTaskForExecutionItems(
                // The items to generate data for
               executionItems,
                // The basequery containing all the insert statements
               basequery,
                // The function to call to generate the final VALUES for the insertion
               queryGenerator.GenerateFinalQuery);

            return _executor.Execute(taskToExecute);
        }

        private TaskExecuter _executor;
        //private TaskExecuter Executor
        //{
        //    get { return _executor; }
        //    set { _executor = value; }
        //}
    }

    
}
