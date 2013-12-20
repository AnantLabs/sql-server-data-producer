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
using SQLDataProducer.Entities.DatabaseEntities.Factories;
using SQLDataProducer.Entities.ExecutionEntities;
using System.Collections.Generic;
using SQLDataProducer.Entities.DataEntities;

namespace SQLDataProducer.Tests.ConsumerTests.InsertConsumer
{
    [TestFixture]
    [MSTest.TestClass]
    public class InsertConsumerTest
    {
        TableEntity customerTable;

        public InsertConsumerTest()
        {
            customerTable = new TableEntity("dbo", "Customer");
            var customerId = DatabaseEntityFactory.CreateColumnEntity("CustomerId", new ColumnDataTypeDefinition("int", false), true, 1, false, null, null);
            customerTable.Columns.Add(customerId);
            customerTable.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("CustomerType", new ColumnDataTypeDefinition("int", false), false, 2, false, null, null));
            customerTable.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("Name", new ColumnDataTypeDefinition("varchar", false), false, 3, false, null, null));
            customerTable.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("IsActive", new ColumnDataTypeDefinition("bit", false), false, 4, false, null, null));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldInsertOneRow()
        {
            SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer.InsertConsumer consumer = new DataConsumers.DataToMSSSQLInsertionConsumer.InsertConsumer();

            var valueStore = new ValueStore();
            DataProducer producer = new DataProducer(valueStore);

            IEnumerable<DataRowEntity> rows = new List<DataRowEntity> { producer.ProduceRow(customerTable, 1) };

            consumer.Init("", new Dictionary<string, string>());
            consumer.Consume(rows, valueStore);
            Assert.That(consumer.TotalRows, Is.EqualTo(1));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldInsertLotsOfRows()
        {
            SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer.InsertConsumer consumer = new DataConsumers.DataToMSSSQLInsertionConsumer.InsertConsumer();

            var valueStore = new ValueStore();
            DataProducer producer = new DataProducer(valueStore);

            List<DataRowEntity> rows = new List<DataRowEntity>();
            for (int i = 0; i < 150; i++)
            {
                rows.Add(producer.ProduceRow(customerTable, i));
            }
           
            consumer.Init("", new Dictionary<string, string>());
            consumer.Consume(rows, valueStore);
            Assert.That(consumer.TotalRows, Is.EqualTo(150));
        }
    }
}
