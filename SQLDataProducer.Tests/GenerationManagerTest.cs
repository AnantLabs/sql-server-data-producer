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
using SQLDataProducer.Entities;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.Entities.DatabaseEntities.Factories;

namespace SQLDataProducer.Tests
{
    [TestFixture]
    [MSTest.TestClass]
    public class GenerationManagerTest
    {
        [Test]
        [MSTest.TestMethod]
        public void ShouldInstanciateGenerationManager()
        {
            var nodeIterator = new NodeIterator(ExecutionNode.CreateLevelOneNode(1));
            var valueStore = new ValueStore();
            var dataProducer = new DataProducer(valueStore);
            GenerationManager manager = new GenerationManager(new MockDataConsumer(), nodeIterator, dataProducer, getN);
        }

        private long i = 1;

        private long getN()
        {
            return i++;
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldIterateThroughOneNodeAndGenerateValues()
        {
            var rootNode = ExecutionNode.CreateLevelOneNode(1);
            rootNode.AddTable(new TableEntity("dbo", "Customer").AddColumn(DatabaseEntityFactory.CreateColumnEntity("CustomerId", new ColumnDataTypeDefinition("int", false), false, 1, false, null, null)));
            
            var nodeIterator = new NodeIterator(rootNode);
            var valueStore = new ValueStore();
            var dataProducer = new DataProducer(valueStore);
            var consumerMock = new MockDataConsumer();

            GenerationManager manager = new GenerationManager(consumerMock, nodeIterator, dataProducer, getN);
            manager.Run("");
            Assert.That(valueStore.Count, Is.EqualTo(1), "valuestore");
            Assert.That(consumerMock.TotalRows, Is.EqualTo(1), "mocked consumations");
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldRequireConnectionStringInRun()
        {
            var rootNode = ExecutionNode.CreateLevelOneNode(1);
            rootNode.AddTable(new TableEntity("dbo", "Customer").AddColumn(DatabaseEntityFactory.CreateColumnEntity("CustomerId", new ColumnDataTypeDefinition("int", false), false, 1, false, null, null)));

            var nodeIterator = new NodeIterator(rootNode);
            var valueStore = new ValueStore();
            var dataProducer = new DataProducer(valueStore);
            var consumerMock = new MockDataConsumer();

            GenerationManager manager = new GenerationManager(consumerMock, nodeIterator, dataProducer, getN);
            manager.Run("");
        }
    }
}
