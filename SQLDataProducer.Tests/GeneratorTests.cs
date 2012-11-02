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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DatabaseEntities.Factories;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.DataAccess;
using SQLDataProducer.ContinuousInsertion.Builders;
using SQLDataProducer.Entities.DatabaseEntities.Collections;
using SQLDataProducer.ContinuousInsertion;
using SQLDataProducer.DataAccess.Factories;
using SQLDataProducer.TaskExecuter;


namespace SQLDataProducer.Entities.Tests
{
    public class GeneratorTests
    {
        [Test]
        public void SmallTest()
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());

            TableEntityCollection tables = tda.GetAllTablesAndColumns();

            foreach (TableEntity table in tables)
            {
                ExecutionItem ie = new ExecutionItem(table);
                ie.RepeatCount = 3;
                TableEntityInsertStatementBuilder builder = new TableEntityInsertStatementBuilder(ie);
                int i = 1;
                builder.GenerateValues(() => i++);

                //foreach (var p in builder.Parameters)
                //{
                //    Console.WriteLine(p.Value + ":" + p.Value.Value);
                //}
                //Console.WriteLine(builder.InsertStatement);

                Console.WriteLine();
                Console.WriteLine(builder.GenerateFullStatement());
                Console.WriteLine("GO");
            }
        }

        [Test]
        public void ExecutionManagerDoOneExecution()
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());
            //TableEntityCollection tables = tda.GetAllTablesAndColumns();
            TableEntity table = tda.GetTableAndColumns("Person", "Address");
            ContinuousInsertionManager manager = new ContinuousInsertionManager(Connection());
            ExecutionItemCollection items = new ExecutionItemCollection();
            //items.AddRange(new ExecutionItemFactory(Connection()).GetExecutionItemsFromTables(tables));
            var ei = new ExecutionItem(table);
            ei.RepeatCount = 3;

            items.Add(ei);
            
            int i = 1;
            manager.DoOneExecution(items, () => i++);
        }

        [Test]
        public void ShouldExecuteWithNewNForEachExecution()
        {
            WorkflowManager wfm;
            OptionEntities.ExecutionTaskOptions options;
            ExecutionItemCollection items;
            Setup(out wfm, out options, out items);

            {
                // new N for each Execution
                options.NumberGeneratorMethod = NumberGeneratorMethods.NewNForEachExecution;
                var res = wfm.RunWorkFlow(options, Connection(), items);

                Console.WriteLine(res.ToString());
                // Issue 80: 
                Assert.AreEqual(13, res.InsertCount, "InsertCount should be 13");
                Assert.AreEqual(0, res.ErrorList.Count, "InsertCount should be 0");
                Assert.AreEqual(1, res.ExecutedItemCount, "ExecutedItemCount should be 1");
                Assert.Greater(res.Duration, TimeSpan.Zero, "Duration should > 0");
            }
        }

        [Test]
        public void ShouldExecuteWithNewNForEachRow()
        {
            WorkflowManager wfm;
            OptionEntities.ExecutionTaskOptions options;
            ExecutionItemCollection items;
            Setup(out wfm, out options, out items);

            {
                // new N for each row
                options.NumberGeneratorMethod = NumberGeneratorMethods.NewNForEachRow;
                var res = wfm.RunWorkFlow(options, Connection(), items);

                Console.WriteLine(res.ToString());
                Assert.AreEqual(13, res.InsertCount, "InsertCount should be 13");
                Assert.AreEqual(0, res.ErrorList.Count, "InsertCount should be 0");
                Assert.AreEqual(1, res.ExecutedItemCount, "ExecutedItemCount should be 1");
                Assert.Greater(res.Duration, TimeSpan.Zero, "Duration should > 0");
            }
        }

        [Test]
        public void ShouldExecuteWithConstantN()
        {
            WorkflowManager wfm;
            OptionEntities.ExecutionTaskOptions options;
            ExecutionItemCollection items;
            Setup(out wfm, out options, out items);
           
            {
                // ConstantN
                options.NumberGeneratorMethod = NumberGeneratorMethods.ConstantN;
                var res = wfm.RunWorkFlow(options, Connection(), items);

                Console.WriteLine(res.ToString());
                Assert.AreEqual(13, res.InsertCount, "InsertCount should be 13");
                Assert.AreEqual(0, res.ErrorList.Count, "InsertCount should be 0");
                Assert.AreEqual(1, res.ExecutedItemCount, "ExecutedItemCount should be 1");
                Assert.Greater(res.Duration, TimeSpan.Zero, "Duration should > 0");
            }
        }

        [Test]
        public void ShouldExecuteOnlyOnConditionEQUALTO()
        {
            var wfm = new WorkflowManager();
            var tda = new TableEntityDataAccess(Connection());
            var adressTable = tda.GetTableAndColumns("Person", "Address");
            var i1 = new ExecutionItem(adressTable);
            i1.ExecutionCondition = ExecutionConditions.EqualTo;
            i1.ExecutionConditionValue = 2;
            i1.RepeatCount = 10;

            var options = new OptionEntities.ExecutionTaskOptions();
            options.ExecutionType = ExecutionTypes.ExecutionCountBased;
            options.FixedExecutions = 3;
            options.NumberGeneratorMethod = NumberGeneratorMethods.NewNForEachRow;
            ExecutionItemCollection items = new ExecutionItemCollection();
            items.Add(i1);

            {
                // new N for each row
                

                var res = wfm.RunWorkFlow(options, Connection(), items);

                Console.WriteLine(res.ToString());
                Assert.AreEqual(10, res.InsertCount, "InsertCount should be 10");
                Assert.AreEqual(0, res.ErrorList.Count, "InsertCount should be 0");
                Assert.AreEqual(3, res.ExecutedItemCount, "ExecutedItemCount should be 1");
                Assert.Greater(res.Duration, TimeSpan.Zero, "Duration should > 0");
            }
        }


        private static void Setup(out WorkflowManager wfm, out OptionEntities.ExecutionTaskOptions options, out ExecutionItemCollection items)
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());
            TableEntity adressTable = tda.GetTableAndColumns("Person", "Address");
            TableEntity departmentTable = tda.GetTableAndColumns("HumanResources", "Department");


            wfm = new WorkflowManager();
            options = new OptionEntities.ExecutionTaskOptions();
            options.ExecutionType = ExecutionTypes.ExecutionCountBased;
            options.FixedExecutions = 1;

            items = new ExecutionItemCollection();

            var ei = new ExecutionItem(adressTable);
            ei.RepeatCount = 3;
            items.Add(ei);

            var eiDepartment = new ExecutionItem(departmentTable);
            eiDepartment.RepeatCount = 10;
            items.Add(eiDepartment);
        }


        private static string Connection()
        {
            return "Data Source=localhost;Initial Catalog=AdventureWorks;Integrated Security=True";
        }
    }
}
