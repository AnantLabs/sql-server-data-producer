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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DatabaseEntities.Factories;
using Generators = SQLDataProducer.Entities.Generators;
using SQLDataProducer.Entities;
using SQLDataProducer.Entities.DataEntities;



namespace SQLDataProducer.Tests
{
    [TestFixture]
    [MSTest.TestClass]
    public class DataProducerTest : TestBase
    {

        TableEntity customerTable;
        TableEntity orderTable;

        public DataProducerTest() 
            : base()
        {
            customerTable = new TableEntity("dbo", "Customer");
            var customerId = DatabaseEntityFactory.CreateColumnEntity("CustomerId", new ColumnDataTypeDefinition("int", false), true, 1, false, null, null);
            customerTable.Columns.Add(customerId);
            customerTable.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("CustomerType", new ColumnDataTypeDefinition("int", false), false, 2, false, null, null));
            customerTable.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("Name", new ColumnDataTypeDefinition("varchar", true), false, 3, false, null, null));
            customerTable.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("IsActive", new ColumnDataTypeDefinition("bit", true), false, 4, false, null, null));

            var fk = new ForeignKeyEntity();
            fk.ReferencingColumn = "CustomerId";
            fk.ReferencingTable = customerTable;
            
            orderTable = new TableEntity("dbo", "Order");
            var customerIdColumn = DatabaseEntityFactory.CreateColumnEntity("CustomerId", new ColumnDataTypeDefinition("int", false), false, 2, true, null, fk);
            customerIdColumn.Generator = Generators.Generator.CreateValueFromOtherColumnGenerator_NewWay();
            customerIdColumn.Generator.GeneratorParameters[0].Value = customerId;

            orderTable.Columns.Add(customerIdColumn);
            orderTable.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("OrderId", new ColumnDataTypeDefinition("int", false), true, 1, false, null, null));
            orderTable.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("Amount", new ColumnDataTypeDefinition("decimal(19, 6)", false), false, 3, false, null, null));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldBeAbleToPutAndGetValueFromValueStore()
        {
            ValueStore valueStore = new ValueStore();
            Guid g = Guid.NewGuid();
            valueStore.Put(g, "Stored Value");

            var retreived = valueStore.GetByKey(g);
            Assert.That(retreived, Is.EqualTo("Stored Value"));
            Assert.That("Stored Value", Is.EqualTo(retreived));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldNotAllowNullValuesInTheDataProducer()
        {

            Action nullValuesInConstructor = new Action ( () =>
                { DataProducer dp = new DataProducer(null); });

            Action nullValuesInProduceRow = new Action ( () =>
                { 
                    DataProducer dp = new DataProducer(new ValueStore()); 
                    dp.ProduceRow(null, 1);
                });

            Action nullTablesInProduceRows = new Action ( () =>
                { 
                    DataProducer dp = new DataProducer(new ValueStore());
                    dp.ProduceRows(null, new Func<long>(() => { return 1; })).Count();
                });
            Action nullGetNInProduceRows = new Action ( () =>
                { 
                    DataProducer dp = new DataProducer(new ValueStore()); 
                    dp.ProduceRows(new List<TableEntity>(), null).Count();
                });

            Assert.That(ExpectedExceptionHappened<ArgumentNullException>(nullValuesInConstructor, "nullValuesInConstructor"));
            Assert.That(ExpectedExceptionHappened<ArgumentNullException>(nullValuesInProduceRow, "nullValuesInProduceRow"));
            Assert.That(ExpectedExceptionHappened<ArgumentNullException>(nullTablesInProduceRows, "nullTablesInProduceRows"));
            Assert.That(ExpectedExceptionHappened<ArgumentNullException>(nullGetNInProduceRows, "nullGetNInProduceRows"));
           
        }

        private bool ExpectedExceptionHappened<T>(Action action, string testName) where T : Exception
        {
            bool exceptionHappened = false;
            try
            {
                action();
            }
            catch (T ex)
            {
                Console.WriteLine(ex.ToString());
                exceptionHappened = true;
            }
            if (exceptionHappened != true)
            {
                Console.WriteLine("Exception did not happend on this:");
                Console.WriteLine(testName);
            }

            return exceptionHappened;
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldOverWriteExistingValueIfSameValueIsPutForSameKeyAgain()
        {
            ValueStore valueStore = new ValueStore();
            Guid g = Guid.NewGuid();
            valueStore.Put(g, "Stored Value");
            valueStore.Put(g, "Modified Value");

            var retreived = valueStore.GetByKey(g);
            Assert.That(retreived, Is.EqualTo("Modified Value"));
            Assert.That("Modified Value", Is.EqualTo(retreived));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGetNullFromValueStoreIfKeyDoesNotExist()
        {
            ValueStore valueStore = new ValueStore();
            var value = valueStore.GetByKey(Guid.NewGuid());
            Assert.That(value, Is.Null);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldProduceOneDataRow()
        {
            ValueStore valuestore = new ValueStore();
            var dp = new DataProducer(valuestore);
            
            DataRowEntity row = dp.ProduceRow(customerTable, 1);
            
            Assert.That(row, Is.Not.Null);
            Assert.That(row.Fields, Is.Not.Null);
            Assert.That(row.Fields.Count, Is.EqualTo(4));

            Assert.That(row.Fields[0].FieldName, Is.EqualTo(customerTable.Columns[0].ColumnName));
            Assert.That(row.Fields[0].ProducesValue, Is.True);
            Assert.That(row.Fields[0].KeyValue, Is.Not.Null);

            AssertFieldsInRowHaveSomeValues(valuestore, row, 2);
        }

        private static void AssertFieldsInRowHaveSomeValues(ValueStore valuestore, DataRowEntity row, int nullColumns)
        {
            var nullableFieldCounter = 0;

            foreach (var field in row.Fields)
            {
                var retreivedValue = valuestore.GetByKey(field.KeyValue);

                Console.WriteLine("{0}: {1}", field.KeyValue, retreivedValue);

                Assert.That(field.KeyValue, Is.Not.Null);
                Assert.That(retreivedValue, Is.Not.Null);

                if (field.DataType.IsNullable)
                {
                    nullableFieldCounter++;
                    Assert.That(retreivedValue, Is.EqualTo(DBNull.Value));
                }
            }

            Assert.That(nullableFieldCounter, Is.EqualTo(nullColumns));
        }

        static long i = 0;
        Func<long> getN = new Func<long>(() =>
        {
            return i++;
        });

        [Test]
        [MSTest.TestMethod]
        public void ShouldProduceDataForAllTables()
        {
            ValueStore valuestore = new ValueStore();
            var dp = new DataProducer(valuestore);

            List<TableEntity> tables = new List<TableEntity>();
            tables.Add(customerTable);
            tables.Add(orderTable);

            List<DataRowEntity> rows = new List<DataRowEntity>();

            rows.AddRange( dp.ProduceRows(tables, getN));
            Assert.That(rows.Count, Is.EqualTo(2));

            Assert.That(rows[0].Fields.Count, Is.EqualTo(4));
            AssertFieldsInRowHaveSomeValues(valuestore, rows[0], 2);

            Assert.That(rows[1].Fields.Count, Is.EqualTo(3));
            AssertFieldsInRowHaveSomeValues(valuestore, rows[1], 0);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldNotProduceValueForFieldsThatGeneratesValueOnInsert()
        {
            Assert.Fail("requirement");
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldUseSameValueWhenUsingIdentityFromOtherColumn()
        {
            ValueStore valuestore = new ValueStore();
            var dp = new DataProducer(valuestore);

            List<TableEntity> tables = new List<TableEntity>();
            tables.Add(customerTable);
            tables.Add(orderTable);

            List<DataRowEntity> rows = new List<DataRowEntity>();

            rows.AddRange(dp.ProduceRows(tables, getN));
            Assert.That(rows.Count, Is.EqualTo(2));
            var customerRow = rows[0];
            var orderRow = rows[1];

            Assert.That(customerRow.Fields.Count, Is.EqualTo(4));
            Assert.That(orderRow.Fields.Count, Is.EqualTo(3));

            Assert.That(orderRow.Fields[1].FieldName, Is.EqualTo("CustomerId"));
            Assert.That(customerRow.Fields[0].FieldName, Is.EqualTo("CustomerId"));

            // Check key is the same as it is for the CustomerId column in CustomerRow
            Assert.That(orderRow.Fields[1].KeyValue, Is.EqualTo(customerRow.Fields[0].KeyValue));
        }

        /*
   * Node1 x50:
   * User
   * PlayerDetails
   * Account
   * Account
   *      Node2 x1:
   *      Deposit
   *          Node3 x100:
   *          Order
   *          Transaction
   *      Node3 x1
   *      Withdraw
         
   * 
   * Output:
   * [User, pd, acc, acc, [dep, [Gr, tran], [Gr, tran] ... [Gr, tran]], Withdraw]
   * 
   */

        //[Test]
        //[MSTest.TestMethod]
        //public void ShouldProduceDataForEachTableThatHaveForeignKeysToEachOther()
        //{
        //    // Scenario: Make 2 customers, for each customer make two accounts and do one deposit and one withdraw for each account
        //    string[] requestedOrder = { "Customer", "Account", "Deposit", "Withdraw", "Account", "Deposit", "Withdraw",
        //                                "Customer", "Account", "Deposit", "Withdraw", "Account", "Deposit", "Withdraw", };

        //    // 2 Customers
        //    ExecutionNode customer = ExecutionNode.CreateLevelOneNode(2, "Customer");
        //    customer.AddTable(new TableEntity("dbo", "Customer"));

        //    // Make 2 accounts
        //    var accounts = customer.AddChild(2, "Accounts");
        //    accounts.AddTable(new TableEntity("dbo", "Account"));

        //    // make one one Deposit and one WithDraw
        //    var accountTransactions = accounts.AddChild(1, "AccountTransactions");
        //    accountTransactions.AddTable(new TableEntity("dbo", "Deposit"));
        //    accountTransactions.AddTable(new TableEntity("dbo", "Withdraw"));


        //    NodeIterator it = new NodeIterator(customer);

        //    List<TableEntity> actual =
        //        new List<TableEntity>(it.GetTablesRecursive());

        //    for (int i = 0; i < requestedOrder.Length; i++)
        //    {
        //        Console.WriteLine(actual[i].TableName);
        //        Assert.That(requestedOrder[i], Is.EqualTo(actual[i].TableName));
        //    }
        //}


    }
}
