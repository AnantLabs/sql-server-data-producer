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
using System.Collections.Generic;
using SQLDataProducer.Entities.DataConsumers;

namespace SQLDataProducer.TaskExecuter
{
    public class WorkflowManager
    {

        private NodeIterator iterator;

        public WorkflowManager()
        {
            
        }

        /// <summary>
        /// Attempt to stop ongoing Async workflow
        /// </summary>
        public void StopAsync()
        {
            if (iterator != null)
            {
                iterator.Cancel();
            }
        }

        public void RunWorkFlowAsync(string connectionString, DataConsumerPluginWrapper consumerWrapper, ExecutionResultBuilder builder, ExecutionTaskOptions options, ExecutionNode rootNode)
        {
            Action a = new Action(() =>
                {
                    RunWorkFlow(connectionString, consumerWrapper, builder, options, rootNode);
                });
            a.BeginInvoke(null, null);
        }

        public void RunWorkFlow(string connectionString, DataConsumerPluginWrapper consumerWrapper, ExecutionResultBuilder builder, ExecutionTaskOptions options, ExecutionNode rootNode)
        {
          //  counter = 0;

            using (var consumer = consumerWrapper.CreateInstance())
            using (iterator = new NodeIterator(rootNode))
            {
                consumer.ReportInsertion = builder.Increment;
                consumer.ReportError = builder.AddError;

                ValueStore valueStore = new ValueStore();
                DataProducer producer = new DataProducer(valueStore);

                builder.Begin();

                consumer.Init(connectionString, consumerWrapper.OptionsTemplate);

                consumer.Consume(producer.ProduceRows(iterator.GetTablesRecursive()), valueStore);
            }
        }
    }
}
