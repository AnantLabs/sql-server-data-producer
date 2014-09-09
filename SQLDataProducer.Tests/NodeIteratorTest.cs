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
using SQLDataProducer.Entities.ExecutionEntities;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;

namespace SQLDataProducer.Tests
{
    [TestFixture]
    [MSTest.TestClass]
    public class NodeIteratorTest
    {
        public NodeIteratorTest()
        {
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldStopWhenAskedToBeCancled()
        {
            ExecutionNode node = ExecutionNode.CreateLevelOneNode(1000000);
            node.AddTable(new TableEntity("", ""));

            List<TableEntity> tables = new List<TableEntity>();

            NodeIterator iterator = new NodeIterator(node);
            Action a = new Action(() =>
            {
                tables.AddRange(iterator.GetTablesRecursive());
            });

            a.BeginInvoke(null, null);
            Thread.Sleep(10);
            iterator.Cancel();
            Console.WriteLine(tables.Count);
            Assert.That(tables.Count, Is.LessThan(1000000));
            Assert.That(tables.Count, Is.GreaterThan(0));
        }

        [Test]
        [MSTest.TestMethod]
        public void shouldReturnOnlyTheNodeIfOnlyOne()
        {
            ExecutionNode node = ExecutionNode.CreateLevelOneNode(1);
            node.AddTable(new TableEntity("", ""));

            int counter = 0;
            NodeIterator it = new NodeIterator(node);
            Assert.That(node.Children.Count(), Is.EqualTo(0));

            HashSet<TableEntity> nodes = new HashSet<TableEntity>();

            foreach (var item in it.GetTablesRecursive())
            {
                nodes.Add(item);
                counter++;
            }

            Assert.That(counter, Is.EqualTo(1));
            Assert.That(nodes.Count, Is.EqualTo(1));
        }
        [Test]
        [MSTest.TestMethod]
        public void ShouldGetAllExecutionItems()
        {
            ExecutionNode node = ExecutionNode.CreateLevelOneNode(1);
            node.AddTable(new TableEntity("", ""));
            node.AddTable(new TableEntity("", ""));

            var child = node.AddChild(1);
            node.AddTable(new TableEntity("", ""));
            node.AddTable(new TableEntity("", ""));
            node.AddTable(new TableEntity("", ""));

            NodeIterator it = new NodeIterator(node);
            int counter = 0;
            foreach (var ei in it.GetTablesRecursive())
            {
                Console.WriteLine(ei);
                counter++;
            }

            Assert.That(counter, Is.EqualTo(5));
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

        [Test]
        [MSTest.TestMethod]
        public void ShouldReturnTablesInCorrectOrder()
        {
            // Scenario: Make 2 customers, for each customer make two accounts and do one deposit and one withdraw for each account
            string[] requestedOrder = { "Customer", "Account", "Deposit", "Withdraw", "Account", "Deposit", "Withdraw",
                                        "Customer", "Account", "Deposit", "Withdraw", "Account", "Deposit", "Withdraw", };

            // 2 Customers
            ExecutionNode customer = ExecutionNode.CreateLevelOneNode(2, "Customer");
            customer.AddTable(new TableEntity("dbo", "Customer"));

            // Make 2 accounts
            var accounts = customer.AddChild(2, "Accounts");
            accounts.AddTable(new TableEntity("dbo", "Account"));

            // make one one Deposit and one WithDraw
            var accountTransactions = accounts.AddChild(1, "AccountTransactions");
            accountTransactions.AddTable(new TableEntity("dbo", "Deposit"));
            accountTransactions.AddTable(new TableEntity("dbo", "Withdraw"));


            NodeIterator it = new NodeIterator(customer);

            List<TableEntity> actual =
                new List<TableEntity>(it.GetTablesRecursive());

            for (int i = 0; i < requestedOrder.Length; i++)
            {
                Console.WriteLine(actual[i].TableName);
                Assert.That(requestedOrder[i], Is.EqualTo(actual[i].TableName));
            }
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldNotAllowNullInTheParameterForNodeIterator()
        {
            bool thrown = false;
            ExecutionNode node = null;
            try
            {
                NodeIterator it = new NodeIterator(node);
            }
            catch (ArgumentNullException)
            {
                thrown = true;
            }

            Assert.That(thrown, Is.True);

        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldCountTheTotalExpectedInsertedRowsInOrderToPredictProgress()
        {
            // 2 Customers
            ExecutionNode customer = ExecutionNode.CreateLevelOneNode(2, "Customer");
            customer.AddTable(new TableEntity("dbo", "Customer"));

            // Make 2 accounts
            var accounts = customer.AddChild(2, "Accounts");
            accounts.AddTable(new TableEntity("dbo", "Account"));

            // make one one Deposit and one WithDraw
            var accountTransactions = accounts.AddChild(1, "AccountTransactions");
            accountTransactions.AddTable(new TableEntity("dbo", "Deposit"));
            accountTransactions.AddTable(new TableEntity("dbo", "Withdraw"));


            NodeIterator it = new NodeIterator(customer);
            long actualExpectedCount = it.GetExpectedInsertCount();
            Assert.That(actualExpectedCount, Is.EqualTo(14));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGetZeroTotalExpectedInsertedRowWhenThereIsNoTable()
        {
            ExecutionNode customer = ExecutionNode.CreateLevelOneNode(2, "No tables");
            NodeIterator it = new NodeIterator(customer);
            long actualExpectedCount = it.GetExpectedInsertCount();
            Assert.That(actualExpectedCount, Is.EqualTo(0));

        }
    }
}
