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

                var res = wfm.RunWorkFlow(options, Connection(), items);

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
            //{
            //    var options = new ExecutionTaskOptions();
            //    options.ExecutionType = ExecutionTypes.ExecutionCountBased;
            //    options.FixedExecutions = 100000;
            //    options.NumberGeneratorMethod = NumberGeneratorMethods.NewNForEachExecution;
            //    ExecutionItemCollection items = new ExecutionItemCollection();
            //    items.Add(i1);
            //    // new N for each execution
            //    var res = wfm.RunWorkFlow(options, Connection(), items);

            //    Console.WriteLine(res.ToString());
            //    Assert.AreEqual(1000000, res.InsertCount, "InsertCount should be 1000000");
            //    Assert.AreEqual(0, res.ErrorList.Count, "InsertCount should be 0");
            //    Assert.AreEqual(100000, res.ExecutedItemCount, "ExecutedItemCount should be 100000");
            //    Assert.Greater(res.Duration, TimeSpan.Zero, "Duration should > 0");
            //}
        }

        [Test]
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
                var res = wfm.RunWorkFlow(options, Connection(), items);

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
            //{
            //    var options = new ExecutionTaskOptions();
            //    options.ExecutionType = ExecutionTypes.ExecutionCountBased;
            //    options.FixedExecutions = 100000;
            //    options.MaxThreads = 10;
            //    options.NumberGeneratorMethod = NumberGeneratorMethods.NewNForEachExecution;
            //    ExecutionItemCollection items = new ExecutionItemCollection();
            //    items.Add(i1);
            //    // new N for each execution
            //    var res = wfm.RunWorkFlow(options, Connection(), items);

            //    Console.WriteLine(res.ToString());
            //    Assert.AreEqual(1000000, res.InsertCount, "InsertCount should be 1000000");
            //    Assert.AreEqual(0, res.ErrorList.Count, "InsertCount should be 0");
            //    Assert.AreEqual(100000, res.ExecutedItemCount, "ExecutedItemCount should be 100000");
            //    Assert.Greater(res.Duration, TimeSpan.Zero, "Duration should > 0");
            //}
        }

//        private static string Connection()
//        {
//            return "Data Source=localhost;Initial Catalog=AdventureWorks;Integrated Security=True";
//        }

//        public LongRunningTests()
//        {
//            string sql = @"
//
//if exists (select 1 from INFORMATION_SCHEMA.tables where TABLE_NAME = 'AnotherTable' and TABLE_SCHEMA = 'Person')
//	drop table Person.AnotherTable;
//
//if exists (select 1 from INFORMATION_SCHEMA.tables where TABLE_NAME = 'NewPerson' and TABLE_SCHEMA = 'Person')
//	drop table Person.NewPerson;
//	
//create table Person.NewPerson(
//	NewPersonId int identity(1, 1) primary key,
//	Name varchar(500) not null,
//	BitColumn bit not null, 
//	DecimalColumn decimal(10, 4) not null,
//	BigintColumn bigint not null, 
//	VarcharMaxColumn varchar(max)  not null,
//	FloatColumn float not null,
//	DateTime2Column datetime2 not null,
//	DateTimeColumn datetime not null,
//	NCharFiveColumn nchar(5) not null,
//	DateColumn date not null, 
//	TimeColumn time not null,
//	SmallIntColumn smallint not null,
//	SmallDateTimeColumn smalldatetime not null,
//	SmallMoneyColumn smallmoney  not null
//);
//
//create table Person.AnotherTable(
//	NewPersonId int foreign key references Person.NewPerson(NewPersonId),
//	AnotherColumn char(1),
//    ThirdColumn char(1) not null
//);";
//            AdhocDataAccess adhd = new AdhocDataAccess(Connection());
//            adhd.ExecuteNonQuery(sql);

//        }
    }
}
