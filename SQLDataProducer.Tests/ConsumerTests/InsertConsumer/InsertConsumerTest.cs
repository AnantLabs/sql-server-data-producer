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
        private SqlCeEngine engine;
        private string connectionString;

        public InsertConsumerTest()
        {
            customerTable = new TableEntity("dbo", "Customer");
            var customerId = DatabaseEntityFactory.CreateColumnEntity("CustomerId", new ColumnDataTypeDefinition("int", false), true, 1, false, null, null);
            customerTable.Columns.Add(customerId);
            customerTable.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("CustomerType", new ColumnDataTypeDefinition("int", false), false, 2, false, null, null));
            customerTable.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("Name", new ColumnDataTypeDefinition("varchar", false), false, 3, false, null, null));
            customerTable.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("IsActive", new ColumnDataTypeDefinition("bit", false), false, 4, false, null, null));

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

                List<DataRowEntity> rows = new List<DataRowEntity>();
                for (int i = 0; i < 150; i++)
                {
                    rows.Add(producer.ProduceRow(customerTable, i));
                }

                consumer.Init(connectionString, new Dictionary<string, string>());
                consumer.Consume(rows, valueStore);
                Assert.That(consumer.TotalRows, Is.EqualTo(150));
            }

            verifyRowCount("Customer", 150);

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
                    cmd.CommandText = "DROP table Customer";
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {

                }

                cmd.CommandText = "create table Customer (CustomerId INT IDENTITY primary key, CustomerType int not null, Name nvarchar(100), IsActive bit)";
                cmd.ExecuteNonQuery();
            }
        }

    }
}
