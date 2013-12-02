//// Copyright 2012-2013 Peter Henell

////   Licensed under the Apache License, Version 2.0 (the "License");
////   you may not use this file except in compliance with the License.
////   You may obtain a copy of the License at

////       http://www.apache.org/licenses/LICENSE-2.0

////   Unless required by applicable law or agreed to in writing, software
////   distributed under the License is distributed on an "AS IS" BASIS,
////   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
////   See the License for the specific language governing permissions and
////   limitations under the License.

//using NUnit.Framework;
//using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
//using SQLDataProducer.DataConsumers.DataToCSVConsumer;
//using SQLDataProducer.Entities.ExecutionEntities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SQLDataProducer.Tests.Helpers;
//using SQLDataProducer.Entities;

//namespace SQLDataProducer.Tests.ConsumerTests
//{
//    [MSTest.TestClass]
//    public class DataToCSVConsumerTests : TestBase
//    {

//        public DataToCSVConsumerTests()
//            : base()
//        {

//        }

//        [Test]
//        [MSTest.TestMethod]
//        public void shouldConsumeDataUsingCSVConsumer()
//        {
//            var table = ExecutionItemHelper.CreateTableWithIdenitityAnd5Columns("dbo", "Peter");
//            var nonIdentityTable = ExecutionItemHelper.CreateTableWith5Columns("dbo", "Peter2");
//            //long i = 0;
//            var ei = new ExecutionNode(table);
//            var ei2 = new ExecutionNode(nonIdentityTable);
//            ei.RepeatCount = 100;
//            ei.RepeatCount = 3;

//            var executionItems = new ExecutionItemCollection();
//            executionItems.Add(ei);
//            executionItems.Add(ei2);


//            var options = new SQLDataProducer.Entities.OptionEntities.ExecutionTaskOptions();
//            options.FixedExecutions = 3;

//            TaskExecuter.WorkflowManager wfm = new TaskExecuter.WorkflowManager();
//            Dictionary<string, string> consumerOptions = new Dictionary<string, string>();
//            consumerOptions.Add("Output Folder", @"c:\temp\repeater\");
//            using (var te = new TaskExecuter.TaskExecuter(options, @"c:\temp\repeater\CSVOutput.txt", executionItems, new DataToCSVConsumer(), consumerOptions))
//            {
//                wfm.RunWorkFlow(te);
//            }

//            ei.RepeatCount = 5;
//            options.NumberGeneratorMethod = NumberGeneratorMethods.NewNForEachExecution;

//            using (var te = new TaskExecuter.TaskExecuter(options, @"c:\temp\repeater\CSVOutput2.txt", executionItems, new DataToCSVConsumer(), consumerOptions))
//            {
//                wfm.RunWorkFlow(te);
//            }


//            //IDataConsumer consumer = new DataToCSVConsumer();
//            //consumer.Consume(rows);
//            //Assert.Fail();
//        }

//    }
//}
