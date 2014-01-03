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

using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.Generators.DecimalGenerators;
using SQLDataProducer.Entities.ExecutionEntities;
using System.Linq;
using SQLDataProducer.TaskExecuter;
using SQLDataProducer.Entities.DataConsumers;
using SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer;
using SQLDataProducer.Entities.OptionEntities;
using SQLDataProducer.Entities.Generators.IntGenerators;
using System;

namespace SQLDataProducer.Tests.IntegrationTests
{
    [TestFixture]
    [MSTest.TestClass]
    public class WorkflowManagerIT : TestBase
    {
        private ExecutionTaskOptions options = new ExecutionTaskOptions();
        

        public WorkflowManagerIT()
            : base()
        {
          
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldSimulateCompleteWorkflow()
        {
            DataAccess.TableEntityDataAccess tda = new DataAccess.TableEntityDataAccess(Connection());
            var person = tda.GetTableAndColumns("Person", "NewPerson");
            var personDetail = tda.GetTableAndColumns("Person", "AnotherTable");
            personDetail.Columns.First().Generator = new ValueFromOtherColumnIntGenerator(personDetail.Columns.First().ColumnDataType);
            personDetail.Columns.First().Generator.GeneratorParameters["Value From Column"].Value = person.Columns.First();

            ExecutionNode node = ExecutionNode.CreateLevelOneNode(100, "Root");
            node.AddTable(person);
            node.AddTable(personDetail);

            var consumerMeta = PluginLoader.GetMetaDataOfType(typeof(InsertConsumer));
            var wrapper = new DataConsumerPluginWrapper(consumerMeta.ConsumerName, typeof(InsertConsumer), consumerMeta.OptionsTemplate);
            ExecutionResultBuilder builder = new ExecutionResultBuilder();

            WorkflowManager manager = new WorkflowManager();
            manager.RunWorkFlow(Connection(), wrapper, builder, options, node);
            var result = builder.Build();
            Assert.That(result.InsertCount, Is.EqualTo(200));
            Assert.That(result.Errors.Count, Is.EqualTo(0));
            Assert.That(result.Duration, Is.GreaterThan(new TimeSpan(1)));
        }
    }
}
