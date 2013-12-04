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
using SQLDataProducer.Entities.DataEntities;
using System.Collections.Generic;
using System;
using SQLDataProducer.Entities.DataConsumers;
using SQLDataProducer.DataConsumers.DataToCSVConsumer;
using SQLDataProducer.Entities.ExecutionEntities;

namespace SQLDataProducer.Tests.ConsumerTests
{
    [TestFixture]
    [MSTest.TestClass]
    public class DataToCSVConsumerTest
    {
        List<DataRowEntity> singleRowDataSet;
        private List<DataRowEntity> tenRowsDataSet;
        private List<DataRowEntity> mixedDataSet;

        ValueStore valueStore = new ValueStore();

        Dictionary<string, string> options;

        public DataToCSVConsumerTest()
        {
            var table = new TableEntity("dbo", "Customer");
            singleRowDataSet = new List<DataRowEntity>();
            singleRowDataSet.Add(new DataRowEntity(table).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));

            tenRowsDataSet = new List<DataRowEntity>();
            tenRowsDataSet.Add(new DataRowEntity(table).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));
            tenRowsDataSet.Add(new DataRowEntity(table).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));
            tenRowsDataSet.Add(new DataRowEntity(table).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));
            tenRowsDataSet.Add(new DataRowEntity(table).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));
            tenRowsDataSet.Add(new DataRowEntity(table).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));
            tenRowsDataSet.Add(new DataRowEntity(table).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));
            tenRowsDataSet.Add(new DataRowEntity(table).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));
            tenRowsDataSet.Add(new DataRowEntity(table).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));
            tenRowsDataSet.Add(new DataRowEntity(table).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));
            tenRowsDataSet.Add(new DataRowEntity(table).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));

            mixedDataSet = new List<DataRowEntity>();
            mixedDataSet.Add(new DataRowEntity(table).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));
            mixedDataSet.Add(new DataRowEntity(table).AddField("OrderId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));

            options = new Dictionary<string, string>();
            options.Add("Output Folder", @"c:\temp");
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldConsumeAllValues()
        {
            IDataConsumer consumer = new DataToCSVConsumer();

            consumer.Init("", options);

            consumer.Consume(tenRowsDataSet, valueStore);
            Assert.That(consumer.TotalRows, Is.EqualTo(10));

        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldProduceValuesForIdentityColumns()
        {

        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldReturnResultAfterConsumption()
        {
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldConsumeDataFromDifferentTables()
        {
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldThrowExceptionIfNotInitiatedBeforeRunning()
        {
            IDataConsumer consumer = new DataToCSVConsumer();
            bool exceptionHappened = false;
            try
            {
                consumer.Consume(new List<DataRowEntity>(), valueStore);
            }
            catch (System.Exception)
            {
                exceptionHappened = true;
            }
            Assert.That(exceptionHappened);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldThrowExceptionIfConsumingNull()
        {
            IDataConsumer consumer = new DataToCSVConsumer();
            bool exceptionHappened = false;
            try
            {
                consumer.Init("", options);
                consumer.Consume(null, valueStore);
            }
            catch (System.Exception)
            {
                exceptionHappened = true;
            }
            Assert.That(exceptionHappened);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldResetTheTotalRowsConsumedAtInit()
        {
            IDataConsumer consumer = new DataToCSVConsumer();
            consumer.Init("", options);

            consumer.Consume(singleRowDataSet, valueStore);
            Assert.That(consumer.TotalRows, Is.EqualTo(1));

            consumer.Init("", options);
            Assert.That(consumer.TotalRows, Is.EqualTo(0));

            consumer.Consume(singleRowDataSet, valueStore);
            Assert.That(consumer.TotalRows, Is.EqualTo(1));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldCountNumberOfRowsConsumed()
        {
            IDataConsumer consumer = new DataToCSVConsumer();
            consumer.Init("", options);

            consumer.Consume(singleRowDataSet, valueStore);
            Assert.That(consumer.TotalRows, Is.EqualTo(1));
        }


    }
}
