// Copyright 2012-2014 Peter Henell

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
using SQLDataProducer.DataConsumers;

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
            throw new NotImplementedException("fix after refactoring");
            //if (_executor != null)
            //{
            //    _executor.EndExecute();
            //}
        }

        public ExecutionResult RunWorkFlow(TaskExecuter executor, string preScript = null, string postScript = null)
        {
            return InternalRunWorkFlow(executor, preScript, postScript);
        }

        public void RunWorkFlowAsync(TaskExecuter executor, Action<ExecutionResult> onCompletedCallback, string preScript = null, string postScript = null)
        {
            Action a = new Action(() =>
                {
                    ExecutionResult res = InternalRunWorkFlow(executor, preScript, postScript);
                    onCompletedCallback(res);
                });
            a.BeginInvoke(null, null);
        }


        private ExecutionResult InternalRunWorkFlow(TaskExecuter executor, string preScript = null, string postScript = null)
        {
            DateTime startTime = DateTime.Now;

            // Initialize the start values for the generators
            using (executor)
            {
                executor.Init();

                executor.CleanUp();

                executor.PreAction(preScript);
                ExecutionResult execResult = executor.Execute();

                executor.PostAction(postScript);

                //execResult.StartTime = startTime;
                //execResult.EndTime = DateTime.Now;

                return execResult;
            }
        }

    }
}
