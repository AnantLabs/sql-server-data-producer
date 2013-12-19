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

        TableEntity customerTable;

        public TableQueryGeneratorTest()
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
            
            Assert.That(statement, Is.EqualTo("INSERT Customer(" + generator.ColumnList + ")"));
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
        public void ShouldGenerateFullInsertStatement()
        {
            TableQueryGenerator generator = new TableQueryGenerator(customerTable);
            var valueStore = new ValueStore();
            DataProducer producer = new DataProducer(valueStore);

            IEnumerable<DataRowEntity> rows = new List<DataRowEntity> { producer.ProduceRow(customerTable, 1), producer.ProduceRow(customerTable, 2) };

            string firstValues = generator.GenerateInsertStatements(rows, valueStore).First();
            Assert.That(firstValues, Is.EqualTo("INSERT Customer(" + generator.ColumnList + ")" + 
                                            " VALUES (0, 'Arboga', 0)"));

            string secondValues = generator.GenerateInsertStatements(rows, valueStore).Skip(1).First();
            Assert.That(secondValues, Is.EqualTo("INSERT Customer(" + generator.ColumnList + ")" +
                                            " VALUES (1, 'Arvika', 1)"));
        }
    }
}
