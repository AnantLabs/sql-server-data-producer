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
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.Generators.DecimalGenerators;
using SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer;
using SQLDataProducer.Entities.DatabaseEntities.Factories;
using SQLDataProducer.Entities.DataEntities;
using System.Collections.Generic;
using System.Linq;
using SQLDataProducer.Entities.ExecutionEntities;

namespace SQLDataProducer.Tests.ConsumerTests.InsertConsumer
{
    [TestFixture]
    [MSTest.TestClass]
    public class TableQueryGeneratorTest
    {

        private TableEntity customerTable;
        private TableEntity tableWithIdentity;
        private TableEntity tableWithNullFields;

        public TableQueryGeneratorTest()
        {
            customerTable = new TableEntity("dbo", "Customer");
            var customerId = DatabaseEntityFactory.CreateColumnEntity("CustomerId", new ColumnDataTypeDefinition("int", false), true, 1, false, null, null);
            customerTable.AddColumn(customerId);
            customerTable.AddColumn(DatabaseEntityFactory.CreateColumnEntity("CustomerType", new ColumnDataTypeDefinition("int", false), false, 2, false, null, null));
            customerTable.AddColumn(DatabaseEntityFactory.CreateColumnEntity("Name", new ColumnDataTypeDefinition("varchar", false), false, 3, false, null, null));
            customerTable.AddColumn(DatabaseEntityFactory.CreateColumnEntity("IsActive", new ColumnDataTypeDefinition("bit", false), false, 4, false, null, null));

            tableWithIdentity = new TableEntity("dbo", "Order");
            tableWithIdentity.AddColumn(DatabaseEntityFactory.CreateColumnEntity("OrderId", new ColumnDataTypeDefinition("int", false), true, 1, false, null, null));
            tableWithIdentity.AddColumn(DatabaseEntityFactory.CreateColumnEntity("OrderDate", new ColumnDataTypeDefinition("datetime", false), false, 2, false, null, null));

            tableWithNullFields = new TableEntity("dbo", "OrderDetails");
            tableWithNullFields.AddColumn(DatabaseEntityFactory.CreateColumnEntity("OrderDetailId", new ColumnDataTypeDefinition("int", false), true, 1, false, null, null));
            tableWithNullFields.AddColumn(DatabaseEntityFactory.CreateColumnEntity("Details", new ColumnDataTypeDefinition("varchar(max)", true), false, 2, false, null, null));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldInstantiateGenerator()
        {
            TableQueryGenerator generator = new TableQueryGenerator(customerTable);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGenerateInsertStatement()
        {
            TableQueryGenerator generator = new TableQueryGenerator(customerTable);
            string statement = generator.InsertStatement;
            
            Assert.That(statement, Is.Not.Null);
            Assert.That(statement, Is.Not.Empty);

            Assert.That(statement, Is.EqualTo("INSERT INTO dbo.Customer(" + generator.ColumnList + ")"));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGenerateColumnListForTableForAllColumnExceptIdentityColumn()
        {
            TableQueryGenerator generator = new TableQueryGenerator(customerTable);
            string columnList = generator.ColumnList;

            Assert.That(columnList, Is.Not.Null);
            Assert.That(columnList, Is.Not.Empty);

            Assert.That(columnList, Is.EqualTo("CustomerType, Name, IsActive"));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGenerateValuesStatement()
        {
            TableQueryGenerator generator = new TableQueryGenerator(customerTable);
            var valueStore = new ValueStore();
            DataProducer producer = new DataProducer(valueStore);
            var dataRow = producer.ProduceRow(customerTable, 1);

            string values = generator.GenerateValuesStatement(dataRow, valueStore);

            Assert.That(values, Is.Not.Empty);
            Assert.That(values, Is.Not.Null);

            Assert.That(values, Is.EqualTo("VALUES (0, 'Arboga', 0)"));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGenerateFullInsertStatementForOneRow()
        {
            TableQueryGenerator generator = new TableQueryGenerator(customerTable);
            var valueStore = new ValueStore();
            DataProducer producer = new DataProducer(valueStore);

            IEnumerable<DataRowEntity> rows = new List<DataRowEntity> { producer.ProduceRow(customerTable, 1)};

            string firstValues = generator.GenerateInsertStatement(rows.First(), valueStore);
            Assert.That(firstValues, Is.StringStarting("INSERT INTO dbo.Customer(" + generator.ColumnList + ") OUTPUT INSERTED.CustomerId")
                                    .And.StringEnding(" VALUES (0, 'Arboga', 0)"));

        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGenerateFullInsertStatementForAllRows()
        {
            var valueStore = new ValueStore();
            DataProducer producer = new DataProducer(valueStore);

            IEnumerable<DataRowEntity> rows = new List<DataRowEntity> { producer.ProduceRow(customerTable, 1), producer.ProduceRow(customerTable, 2) };

            string firstValues = TableQueryGenerator.GenerateInsertStatements(rows, valueStore).First();
            Assert.That(firstValues, Is.StringStarting("INSERT INTO dbo.Customer(CustomerType, Name, IsActive) OUTPUT INSERTED.CustomerId AS")
                                    .And.StringEnding(" VALUES (0, 'Arboga', 0)"));

            string secondValues = TableQueryGenerator.GenerateInsertStatements(rows, valueStore).Skip(1).First();
            Assert.That(secondValues, Is.StringStarting("INSERT INTO dbo.Customer(CustomerType, Name, IsActive) OUTPUT INSERTED.CustomerId AS")
                                        .And.StringEnding(" VALUES (1, 'Arvika', 1)"));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGenerateInsertStatementForValueProducingColumns()
        {
            var valueStore = new ValueStore();
            DataProducer producer = new DataProducer(valueStore);

            TableQueryGenerator generator = new TableQueryGenerator(tableWithIdentity);

            IEnumerable<DataRowEntity> rows = new List<DataRowEntity> { producer.ProduceRow(tableWithIdentity, 1) };


            Assert.That(generator.ColumnList, Is.EqualTo("OrderDate"));
            Assert.That(
                generator.GenerateInsertStatement(rows.First(), valueStore),
                Is.StringStarting("INSERT INTO dbo.Order(" + generator.ColumnList + ") OUTPUT INSERTED.OrderId"));

        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldKnowAboutNullValues()
        {
            var valueStore = new ValueStore();
            DataProducer producer = new DataProducer(valueStore);

            TableQueryGenerator generator = new TableQueryGenerator(tableWithNullFields);

            IEnumerable<DataRowEntity> rows = new List<DataRowEntity> { producer.ProduceRow(tableWithNullFields, 1) };

            string actual = generator.GenerateValuesStatement(rows.First(), valueStore);
            Assert.That(actual, Is.EqualTo("VALUES (NULL)"));
        }
    }
}
