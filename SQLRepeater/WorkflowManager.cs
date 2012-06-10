using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLRepeater.EntityQueryGenerator;
using SQLRepeater.TaskExecuter;
using SQLRepeater.Entities.ExecutionOrderEntities;
using SQLRepeater.Entities;

namespace SQLRepeater
{
    public class WorkflowManager
    {
        //private WorkFlowStates 
        WorkflowData _currentWorkFlowData;

        public WorkflowManager()
        {
            
        }

        public void Stop()
        {
            
            _currentWorkFlowData.Executor.EndExecute();
        }

        public void RunWorkFlowAsync(string connectionString, ExecutionItemCollection executionItems, Action<int> completedCallback)
        {
            _currentWorkFlowData = new WorkflowData();
            _currentWorkFlowData.OnCompleteCallback = completedCallback;
            _currentWorkFlowData.Executor = new TaskExecuter.TaskExecuter(connectionString);
            _currentWorkFlowData.ExecutionItems = executionItems;

            Action a = new Action(() =>
            {
                Prepare();
                Execute();
                FinalizeExecution();
            });
        }

        private void Prepare()
        {
            
        }

        private void Execute()
        {
            

            InsertQueryGenerator queryGenerator = new InsertQueryGenerator();
            string basequery = queryGenerator.GenerateQueryForExecutionItems(_currentWorkFlowData.ExecutionItems);
            ExecutionTaskDelegate taskToExecute = _currentWorkFlowData.Executor.CreateSQLTaskForExecutionItems(
                // The items to generate data for
               _currentWorkFlowData.ExecutionItems,
                // The basequery containing all the insert statements
               basequery,
                // The function to call to generate the final VALUES for the insertion
               queryGenerator.GenerateFinalQuery);

            _currentWorkFlowData.Executor.BeginExecute(taskToExecute, FinalizeExecution);
        }

        private void FinalizeExecution()
        {
            
            
            _currentWorkFlowData.OnCompleteCallback(count);
            
        }

        
    }

    private class WorkflowData
    {
        TaskExecuter.TaskExecuter _executor;
        public TaskExecuter.TaskExecuter Executor
        {
            get { return _executor; }
            set { _executor = value; }
        }

        public string PreScript { get; set; }
        public string PostScript { get; set; }
        
        Action<int> _onCompleteCallback;
        public Action<int> OnCompleteCallback
        {
            get { return _onCompleteCallback; }
            set { _onCompleteCallback = value; }
        }

        private ExecutionItemCollection _executionItems;
        public ExecutionItemCollection ExecutionItems
        {
            get { return _executionItems; }
            set { _executionItems = value; }
        }

    }
}
