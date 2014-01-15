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
using SQLDataProducer.Entities.DatabaseEntities.Factories;
using SQLDataProducer.Entities.ExecutionEntities;
using System.Collections.Generic;
using SQLDataProducer.Entities.DataEntities;
using SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer.DataAccess;
using System.Data.SqlServerCe;
using System.IO;
using System;
using System.Data;
using System.Collections;
using System.Data.Common;

namespace SQLDataProducer.Tests.ConsumerTests.InsertConsumer
{
    [TestFixture]
    [MSTest.TestClass]
    public class InsertConsumerTest 
    {
        TableEntity customerTable;
        TableEntity orderTable;

      
        private string connectionString;
        private int insertCounter = 0;

        public InsertConsumerTest()
        {
            customerTable = new TableEntity("dbo", "Customer");
            var customerId = DatabaseEntityFactory.CreateColumnEntity("CustomerId", new ColumnDataTypeDefinition("int", false), true, 1, false, null, null);
            customerTable.AddColumn(customerId);
            customerTable.AddColumn(DatabaseEntityFactory.CreateColumnEntity("CustomerType", new ColumnDataTypeDefinition("int", false), false, 2, false, null, null));
            customerTable.AddColumn(DatabaseEntityFactory.CreateColumnEntity("Name", new ColumnDataTypeDefinition("varchar", false), false, 3, false, null, null));
            customerTable.AddColumn(DatabaseEntityFactory.CreateColumnEntity("IsActive", new ColumnDataTypeDefinition("bit", false), false, 4, false, null, null));

            orderTable = new TableEntity("dbo", "Orders");
            orderTable.AddColumn(DatabaseEntityFactory.CreateColumnEntity("OrderId", new ColumnDataTypeDefinition("int", false), true, 1, false, null, null));

            var fk = new ForeignKeyEntity();
            fk.ReferencingColumn = "CustomerId";
            fk.ReferencingTable = customerTable;

            var customerIdColumn = DatabaseEntityFactory.CreateColumnEntity("CustomerId", new ColumnDataTypeDefinition("int", false), false, 2, true, null, fk);
            customerIdColumn.Generator = new SQLDataProducer.Entities.Generators.IntGenerators.ValueFromOtherColumnIntGenerator(customerIdColumn.ColumnDataType);
            customerIdColumn.Generator.GeneratorParameters["Value From Column"].Value = customerId;

            orderTable.AddColumn(customerIdColumn);
            orderTable.AddColumn(DatabaseEntityFactory.CreateColumnEntity("ArticleId", new ColumnDataTypeDefinition("int", false), false, 3, false, null, null));
            orderTable.AddColumn(DatabaseEntityFactory.CreateColumnEntity("TotalAmount", new ColumnDataTypeDefinition("decimal(19, 6)", false), false, 4, false, null, null));


            var builder  = new System.Data.SqlClient.SqlConnectionStringBuilder();
            builder.DataSource = "localhost";
            builder.InitialCatalog = "AdventureWorks";
            builder.IntegratedSecurity = true;
            connectionString = builder.ToString();
        }


        [Test]
        [MSTest.TestMethod]
        public void ShouldInsertLotsOfRows()
        {

            prepareTables();

            using (SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer.InsertConsumer consumer = new DataConsumers.DataToMSSSQLInsertionConsumer.InsertConsumer())
            {
                var valueStore = new ValueStore();
                DataProducer producer = new DataProducer(valueStore);

                consumer.Init(connectionString, new Dictionary<string, string>());
                int rowCount = 0;
                consumer.ReportInsertion = new Action(() =>
                {
                    rowCount++;
                });

                for (int i = 0; i < 150; i++)
                {
                      consumer.Consume(producer.ProduceRows(new List<TableEntity> { customerTable, orderTable }, getN), valueStore);
                }

                Assert.That(rowCount, Is.EqualTo(300));
            }

            verifyRowCount("Customer", 150);
            verifyRowCount("Orders", 150);
        }

        private long getN()
        {
            return insertCounter++;
        }

        private void verifyRowCount(string tableName, int expectedCount)
        {
            using (var conn = CommandFactory.CreateDbConnection(connectionString))
            {
                conn.Open();
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "Select * From " + tableName;
                DataAdapter adapter = CommandFactory.CreateSelectDataAdapter(cmd);
                DataSet ds = new DataSet();

                adapter.Fill(ds);
               int totalRows =  ds.Tables[0].Rows.Count;

               Assert.That(totalRows, Is.EqualTo(expectedCount));
            }
        }

        private void prepareTables()
        {
            using (var conn = CommandFactory.CreateDbConnection(connectionString))
            {
                conn.Open();
                DbCommand cmd = conn.CreateCommand();

                try
                {
                    cmd.CommandText = "DROP TABLE [Orders]; DROP table Customer;";
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    //Assert.Fail(e.ToString());
                }

                cmd.CommandText = "create table Customer (CustomerId INT IDENTITY primary key, CustomerType int not null, Name nvarchar(100), IsActive bit)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "create table [Orders] (OrderId int identity primary key, CustomerId INT not null foreign key references Customer(CustomerId), ArticleId int not null, TotalAmount decimal(19, 6))";
                cmd.ExecuteNonQuery();
            }
        }

    }
}
