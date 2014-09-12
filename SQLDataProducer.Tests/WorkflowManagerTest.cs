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

using NUnit.Framework;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DataConsumers;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.Entities.OptionEntities;
using SQLDataProducer.TaskExecuter;
using System.Collections.Generic;
using System.Threading;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;



namespace SQLDataProducer.Tests
{
    [TestFixture]
    [MSTest.TestClass]
    public class WorkflowManagerTest
    {

        string connectionString = "";

        ExecutionResultBuilder builder = new ExecutionResultBuilder();
        ExecutionTaskOptions options = new ExecutionTaskOptions();
        DataConsumerPluginWrapper wrapper;

        WorkflowManager manager = new WorkflowManager();

        public WorkflowManagerTest()
        {
            var consumerMeta = PluginLoader.GetMetaDataOfType(typeof(MockDataConsumer));
            wrapper = new DataConsumerPluginWrapper(consumerMeta.ConsumerName, typeof(MockDataConsumer), consumerMeta.OptionsTemplate);
        }

      

        [Test]
        [MSTest.TestMethod]
        public void ShouldConsumeOneRow()
        {
            builder = new ExecutionResultBuilder();
            builder.Begin();
            ExecutionNode rootNode = ExecutionNode.CreateLevelOneNode(1, "Root");
            rootNode.AddTable(new TableEntity("dbo", "Customer"));
            
            manager.RunWorkFlow(connectionString, wrapper, builder, options, rootNode);
            
            var result = builder.Build();
            Assert.That(result.Errors.Count, Is.EqualTo(0));
            Assert.That(result.InsertCount, Is.EqualTo(1));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldRunWorkflowAsync()
        {
            builder = new ExecutionResultBuilder().Begin();
            ExecutionNode rootNode = ExecutionNode.CreateLevelOneNode(1, "Root");
            rootNode.AddTable(new TableEntity("dbo", "Customer"));

            manager.RunWorkFlowAsync(connectionString, wrapper, builder, options, rootNode);
            Thread.Sleep(10); // give some time for the async method to complete
            var result = builder.Build();
            Assert.That(result.Errors.Count, Is.EqualTo(0));
            Assert.That(result.InsertCount, Is.EqualTo(1));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldInstantiateWorkflowManager()
        {
            builder = new ExecutionResultBuilder().Begin();
            ExecutionNode rootNode = ExecutionNode.CreateLevelOneNode(1, "Root");
            manager.RunWorkFlow(connectionString, wrapper, builder, options, rootNode);

            var result = builder.Build();
            Assert.That(result.Errors.Count, Is.EqualTo(0), "error count");
            Assert.That(result.InsertCount, Is.EqualTo(0), "insert count");
        }

      
    }
}
