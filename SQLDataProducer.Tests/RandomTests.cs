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
using NUnit.Framework;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.DataAccess;
using SQLDataProducer.Entities.DatabaseEntities.Collections;
using SQLDataProducer.TaskExecuter;
using SQLDataProducer.Entities;
using SQLDataProducer.Entities.OptionEntities;
//using SQLDataProducer.ContinuousInsertion.Builders;

namespace SQLDataProducer.RandomTests
{
    public class RandomTests : TestBase
    {
        //[Test]
        //public void ShouldGenerateValuesAndInsertStatementsForAllTables()
        //{
        //    //TableEntityDataAccess tda = new TableEntityDataAccess(Connection());

        //    //TableEntityCollection tables = tda.GetAllTablesAndColumns();
        //    //Assert.Greater(tables.Count, 0);
        //    //foreach (TableEntity table in tables)
        //    //{
        //    //    ExecutionItem ie = new ExecutionItem(table);
        //    //    ie.RepeatCount = 3;
        //    //    TableEntityInsertStatementBuilder builder = new TableEntityInsertStatementBuilder(ie);
        //    //    int i = 1;
        //    //    //builder.GenerateValues(() => i++);

        //    //    Assert.Greater(table.Columns.Count, 0);

        //    //    Console.WriteLine();
        //    //    Console.WriteLine("We dont really care about the errors, we just want to generate data for all the tables and columns.");
        //    //    Console.WriteLine(builder.InsertStatement);//.GenerateFullStatement());
        //    //    Console.WriteLine("GO");
        //    //}
        //    Assert.Fail("Test not implemented yet");
        //}

        [Test]
        public void ShouldExecuteWithNewNForEachExecution()
        {
            WorkflowManager wfm;
            ExecutionTaskOptions options;
            ExecutionItemCollection items;
            Setup(out wfm, out options, out items);

            {
                // new N for each Execution
                options.NumberGeneratorMethod = NumberGeneratorMethods.NewNForEachExecution;
                var executor = new TaskExecuter.TaskExecuter(options, Connection(), items, DefaultDataConsumer);
                var res = wfm.RunWorkFlow(new TaskExecuter.TaskExecuter(options, Connection(), items, DefaultDataConsumer));

                Console.WriteLine(res.ToString());
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
            ExecutionTaskOptions options;
            ExecutionItemCollection items;
            Setup(out wfm, out options, out items);

            {
                // new N for each row
                options.NumberGeneratorMethod = NumberGeneratorMethods.NewNForEachRow;
                var res = wfm.RunWorkFlow(new TaskExecuter.TaskExecuter(options, Connection(), items, DefaultDataConsumer));

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
            ExecutionTaskOptions options;
            ExecutionItemCollection items;
            Setup(out wfm, out options, out items);
           
            {
                // ConstantN
                options.NumberGeneratorMethod = NumberGeneratorMethods.ConstantN;
                var res = wfm.RunWorkFlow(new TaskExecuter.TaskExecuter(options, Connection(), items, DefaultDataConsumer));

                Console.WriteLine(res.ToString());
                Assert.AreEqual(13, res.InsertCount, "InsertCount should be 13");
                Assert.AreEqual(0, res.ErrorList.Count, "InsertCount should be 0");
                Assert.AreEqual(1, res.ExecutedItemCount, "ExecutedItemCount should be 1");
                Assert.Greater(res.Duration, TimeSpan.Zero, "Duration should > 0");
            }
        }

        [Test]
        public void ShouldExecuteOnlyOnCondition_EQUALTO()
        {
            var wfm = new WorkflowManager();
            var tda = new TableEntityDataAccess(Connection());
            var adressTable = tda.GetTableAndColumns("Person", "NewPerson");
            var i1 = new ExecutionItem(adressTable);
            i1.ExecutionCondition = ExecutionConditions.EqualTo;
            i1.ExecutionConditionValue = 2;
            i1.RepeatCount = 10;

            var options = new ExecutionTaskOptions();
            options.ExecutionType = ExecutionTypes.ExecutionCountBased;
            options.FixedExecutions = 3;
            options.NumberGeneratorMethod = NumberGeneratorMethods.NewNForEachRow;
            ExecutionItemCollection items = new ExecutionItemCollection();
            items.Add(i1);

            {
                // new N for each row
                var res = wfm.RunWorkFlow(new TaskExecuter.TaskExecuter(options, Connection(), items, DefaultDataConsumer));

                Console.WriteLine(res.ToString());
                Assert.AreEqual(0, res.InsertCount, "InsertCount should be 10");
                Assert.AreEqual(0, res.ErrorList.Count, "InsertCount should be 0");
                Assert.AreEqual(3, res.ExecutedItemCount, "ExecutedItemCount should be 1");
                Assert.Greater(res.Duration, TimeSpan.Zero, "Duration should > 0");
            }
        }

        [Test]
        public void ShouldExecuteOnlyOnCondition_GREATERTHAN()
        {
            var wfm = new WorkflowManager();
            var tda = new TableEntityDataAccess(Connection());
            var adressTable = tda.GetTableAndColumns("Person", "NewPerson");
            var i1 = new ExecutionItem(adressTable);
            i1.ExecutionCondition = ExecutionConditions.GreaterThan;
            i1.ExecutionConditionValue = 2;
            i1.RepeatCount = 10;

            var options = new ExecutionTaskOptions();
            options.ExecutionType = ExecutionTypes.ExecutionCountBased;
            options.FixedExecutions = 3;
            options.NumberGeneratorMethod = NumberGeneratorMethods.NewNForEachRow;
            ExecutionItemCollection items = new ExecutionItemCollection();
            items.Add(i1);

            {
                // new N for each row
                var res = wfm.RunWorkFlow(new TaskExecuter.TaskExecuter(options, Connection(), items, DefaultDataConsumer));

                Console.WriteLine(res.ToString());
                Assert.AreEqual(2, res.InsertCount, "InsertCount should be 10");
                Assert.AreEqual(0, res.ErrorList.Count, "ErrorList should be 0");
                Assert.AreEqual(3, res.ExecutedItemCount, "ExecutedItemCount should be 3");
                Assert.Greater(res.Duration, TimeSpan.Zero, "Duration should > 0");
            }
        }

        [Test]
        public void ShouldExecuteOnlyOnCondition_EqualOrGreaterThan()
        {
            var wfm = new WorkflowManager();
            var tda = new TableEntityDataAccess(Connection());
            var adressTable = tda.GetTableAndColumns("Person", "NewPerson");
            var i1 = new ExecutionItem(adressTable);
            i1.ExecutionCondition = ExecutionConditions.EqualOrGreaterThan;
            i1.ExecutionConditionValue = 2;
            i1.RepeatCount = 10;

            var options = new ExecutionTaskOptions();
            options.ExecutionType = ExecutionTypes.ExecutionCountBased;
            options.FixedExecutions = 3;
            options.NumberGeneratorMethod = NumberGeneratorMethods.NewNForEachRow;
            ExecutionItemCollection items = new ExecutionItemCollection();
            items.Add(i1);

            {
                // new N for each row
                var res = wfm.RunWorkFlow(new TaskExecuter.TaskExecuter(options, Connection(), items, DefaultDataConsumer));

                Console.WriteLine(res.ToString());
                Assert.AreEqual(3, res.InsertCount, "InsertCount should be 3");
                Assert.AreEqual(0, res.ErrorList.Count, "ErrorList should be 0");
                Assert.AreEqual(3, res.ExecutedItemCount, "ExecutedItemCount should be 3");
                Assert.Greater(res.Duration, TimeSpan.Zero, "Duration should > 0");
            }
        }

        [Test]
        public void ShouldExecuteOnlyOnCondition_LessThan()
        {
            var wfm = new WorkflowManager();
            var tda = new TableEntityDataAccess(Connection());
            var adressTable = tda.GetTableAndColumns("Person", "NewPerson");
            var i1 = new ExecutionItem(adressTable);
            i1.ExecutionCondition = ExecutionConditions.LessThan;
            i1.ExecutionConditionValue = 2;
            i1.RepeatCount = 10;

            var options = new ExecutionTaskOptions();
            options.ExecutionType = ExecutionTypes.ExecutionCountBased;
            options.FixedExecutions = 3;
            options.NumberGeneratorMethod = NumberGeneratorMethods.NewNForEachRow;
            ExecutionItemCollection items = new ExecutionItemCollection();
            items.Add(i1);

            {
                // new N for each row
                var res = wfm.RunWorkFlow(new TaskExecuter.TaskExecuter(options, Connection(), items, DefaultDataConsumer));

                Console.WriteLine(res.ToString());
                Assert.AreEqual(0, res.InsertCount, "InsertCount should be 3");
                Assert.AreEqual(0, res.ErrorList.Count, "InsertCount should be 0");
                Assert.AreEqual(3, res.ExecutedItemCount, "ExecutedItemCount should be 3");
                Assert.Greater(res.Duration, TimeSpan.Zero, "Duration should > 0");
            }
        }

        [Test]
        public void ShouldExecuteOnlyOnCondition_EveryOtherX()
        {
            var wfm = new WorkflowManager();
            var tda = new TableEntityDataAccess(Connection());
            var adressTable = tda.GetTableAndColumns("Person", "NewPerson");
            var i1 = new ExecutionItem(adressTable);
            i1.ExecutionCondition = ExecutionConditions.EveryOtherX;
            i1.ExecutionConditionValue = 2;
            i1.RepeatCount = 10;

            var options = new ExecutionTaskOptions();
            options.ExecutionType = ExecutionTypes.ExecutionCountBased;
            options.FixedExecutions = 3;
            options.NumberGeneratorMethod = NumberGeneratorMethods.NewNForEachRow;
            ExecutionItemCollection items = new ExecutionItemCollection();
            items.Add(i1);

            {
                // new N for each row
                var res = wfm.RunWorkFlow(new TaskExecuter.TaskExecuter(options, Connection(), items, DefaultDataConsumer));

                Console.WriteLine(res.ToString());
                Assert.AreEqual(3, res.InsertCount, "InsertCount should be 0");
                Assert.AreEqual(0, res.ErrorList.Count, "InsertCount should be 0");
                Assert.AreEqual(3, res.ExecutedItemCount, "ExecutedItemCount should be 1");
                Assert.Greater(res.Duration, TimeSpan.Zero, "Duration should > 0");
            }
        }

       
        


        private static void Setup(out WorkflowManager wfm, out ExecutionTaskOptions options, out ExecutionItemCollection items)
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());
            TableEntity adressTable = tda.GetTableAndColumns("Person", "NewPerson");
            TableEntity departmentTable = tda.GetTableAndColumns("Person", "AnotherTable");


            wfm = new WorkflowManager();
            options = new ExecutionTaskOptions();
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

        [Test]
        public void SetCounterShouldIncrementAndAdd()
        {
            // TODO: Test thread safety of set counter methods
            SetCounter sc = new SetCounter();

            Assert.AreEqual(0, sc.Peek());

            sc.Increment();
            Assert.AreEqual(1, sc.Peek());

            sc.Add(10);
            Assert.AreEqual(11, sc.Peek());

        }

        private static string Connection()
        {
            return "Data Source=localhost;Initial Catalog=AdventureWorks;Integrated Security=True";
        }

        public RandomTests()
        {
            string sql = @"

if exists (select 1 from INFORMATION_SCHEMA.tables where TABLE_NAME = 'AnotherTable' and TABLE_SCHEMA = 'Person')
	drop table Person.AnotherTable;

if exists (select 1 from INFORMATION_SCHEMA.tables where TABLE_NAME = 'NewPerson' and TABLE_SCHEMA = 'Person')
	drop table Person.NewPerson;
	
	

create table Person.NewPerson(
	NewPersonId int identity(1, 1) primary key,
	Name varchar(500) not null,
	BitColumn bit not null, 
	DecimalColumn decimal(10, 4) not null,
	BigintColumn bigint not null, 
	VarcharMaxColumn varchar(max)  not null,
	FloatColumn float not null,
	DateTime2Column datetime2 not null,
	DateTimeColumn datetime not null,
	NCharFiveColumn nchar(5) not null,
	DateColumn date not null, 
	TimeColumn time not null,
	SmallIntColumn smallint not null,
	SmallDateTimeColumn smalldatetime not null,
	SmallMoneyColumn smallmoney  not null
);

create table Person.AnotherTable(
	NewPersonId int foreign key references Person.NewPerson(NewPersonId),
	AnotherColumn char(1),
    ThirdColumn char(1) not null
);";
            AdhocDataAccess adhd = new AdhocDataAccess(Connection());
            adhd.ExecuteNonQuery(sql);

        }
    }
}
