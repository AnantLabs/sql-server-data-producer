// Copyright 2012-2013 Peter Henell

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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SQLDataProducer.TaskExecuter;
using SQLDataProducer.DataAccess;
using SQLDataProducer.Entities.OptionEntities;
using SQLDataProducer.Entities;
using SQLDataProducer.Entities.ExecutionEntities;

namespace SQLDataProducer.RandomTests
{

    public class LongRunningTests : TestBase
    {
        public LongRunningTests()
            :base()
        {
        }

        [Test]
        [Ignore]
        public void ShouldExecute_50000_Executions()
        {
            var wfm = new WorkflowManager();
            var tda = new TableEntityDataAccess(Connection());
            var adressTable = tda.GetTableAndColumns("Person", "NewPerson");
            var i1 = new ExecutionItem(adressTable);
            i1.ExecutionCondition = ExecutionConditions.None;
            i1.RepeatCount = 10;

            {
                var options = new ExecutionTaskOptions();
                options.ExecutionType = ExecutionTypes.ExecutionCountBased;
                options.FixedExecutions = 50000;
                options.NumberGeneratorMethod = NumberGeneratorMethods.NewNForEachRow;
                ExecutionItemCollection items = new ExecutionItemCollection();
                
                items.Add(i1);
                // new N for each row
                foreach (var c in i1.TargetTable.Columns)
                {
                    Console.WriteLine("Generator: {0} - DataType: {1}", c.Generator.GeneratorName, c.ColumnDataType.DBType.ToString());
                }

                var res = wfm.RunWorkFlow(new TaskExecuter.TaskExecuter(options, Connection(), items, DefaultDataConsumer));

                foreach (var s in res.ErrorList)
                {
                    Console.WriteLine(s);
                }
                

                Console.WriteLine(res.ToString());
                Assert.AreEqual(500000, res.InsertCount, "InsertCount should be 500000");
                Assert.AreEqual(0, res.ErrorList.Count, "InsertCount should be 0");
                Assert.AreEqual(50000, res.ExecutedItemCount, "ExecutedItemCount should be 50000");
                Assert.Greater(res.Duration, TimeSpan.Zero, "Duration should > 0");
            }
        }

        [Test]
        [Ignore]
        public void ShouldExecute_50000_Executions_Threaded()
        {
            var wfm = new WorkflowManager();
            var tda = new TableEntityDataAccess(Connection());
            var adressTable = tda.GetTableAndColumns("Person", "NewPerson");
            var i1 = new ExecutionItem(adressTable);
            i1.ExecutionCondition = ExecutionConditions.None;
            i1.RepeatCount = 10;

            {
                var options = new ExecutionTaskOptions();
                options.ExecutionType = ExecutionTypes.ExecutionCountBased;
                options.FixedExecutions = 50000;
                options.MaxThreads = 10;
                options.NumberGeneratorMethod = NumberGeneratorMethods.NewNForEachRow;
                ExecutionItemCollection items = new ExecutionItemCollection();
                items.Add(i1);

                foreach (var c in i1.TargetTable.Columns)
                {
                    Console.WriteLine("Generator: {0} - DataType: {1}", c.Generator.GeneratorName, c.ColumnDataType.DBType.ToString());
                }

                // new N for each row
                var res = wfm.RunWorkFlow(new TaskExecuter.TaskExecuter(options, Connection(), items, DefaultDataConsumer));

                foreach (var s in res.ErrorList)
                {
                    Console.WriteLine(s);
                }

                Console.WriteLine(res.ToString());
                Assert.AreEqual(500000, res.InsertCount, "InsertCount should be 500000");
                Assert.AreEqual(0, res.ErrorList.Count, "InsertCount should be 0");
                Assert.AreEqual(50000, res.ExecutedItemCount, "ExecutedItemCount should be 50000");
                Assert.Greater(res.Duration, TimeSpan.Zero, "Duration should > 0");
            }
           
        }

    }
}
