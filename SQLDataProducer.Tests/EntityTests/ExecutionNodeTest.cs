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
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DatabaseEntities.Factories;
using SQLDataProducer.Entities.ExecutionEntities;
using Generators = SQLDataProducer.Entities.Generators;
using SQLDataProducer.Entities;


namespace SQLDataProducer.Tests.EntitiesTests
{
    [TestFixture]
    [MSTest.TestClass]
    public class ExecutionNodeTest : TestBase
    {
        public ExecutionNodeTest()
        {
        }

        [Test]
        [MSTest.TestMethod]
        public void shouldCreateExecutionNodeBySettingRepeatCount()
        {
            int numberOfRepeats = 10;
            ExecutionNode node = ExecutionNode.CreateLevelOneNode(numberOfRepeats);
            Assert.NotNull(node);
            Assert.That(node.RepeatCount, Is.EqualTo(numberOfRepeats));
        }

        [Test]
        [MSTest.TestMethod]
        public void shouldIncreaseTheLevelOfEachChild()
        {
            ExecutionNode node = ExecutionNode.CreateLevelOneNode(1);
            var node2 = node.AddChild(100);
            var node3 = node2.AddChild(24);

            var node4 = node.AddChild(1);

            Assert.That(node2.Level, Is.GreaterThan(node.Level));
            Assert.That(node2.Level, Is.EqualTo(node.Level + 1));

            Assert.That(node3.Level, Is.GreaterThan(node2.Level));
            Assert.That(node3.Level, Is.EqualTo(node2.Level + 1));

            Assert.That(node4.Level, Is.GreaterThan(node.Level));
            Assert.That(node4.Level, Is.EqualTo(node.Level + 1));
        }

        [Test]
        [MSTest.TestMethod]
        public void shouldAddChildNodesToNode()
        {
            ExecutionNode node = ExecutionNode.CreateLevelOneNode(1);
            node.AddChild(100);
            Assert.AreEqual(1, node.Children.Count, "Number of children");

            node.AddChild(1);
            node.AddChild(1);
            Assert.AreEqual(3, node.Children.Count, "Number of children");
            foreach (var item in node.Children)
            {
                Assert.That(item.Level, Is.GreaterThan(1));
                Console.WriteLine(item.Level);

                for (int i = 0; i < 3; i++)
                    item.AddChild(1);

                foreach (var child in item.Children)
                {
                    Assert.That(item.Level, Is.LessThan(child.Level));
                }
            }
        }

        //[Test]
        //[MSTest.TestMethod]
        //public void shouldIterateOverAllNodesInTree()
        //{
        //    ExecutionNode node = ExecutionNode.CreateLevelOneNode(1);
        //    node.AddChild(1).AddChild(1).AddChild(1).AddChild(1).AddChild(1).AddChild(1);

        //    int counter = 0;
        //    NodeIterator it = new NodeIterator(node);
        //    Assert.That(node.Children.Count, Is.GreaterThan(0));

        //    foreach (var item in it.GetNodesRecursive())
        //    {
        //        counter++;
        //        Console.WriteLine(item.NodeId);
        //    }

        //    Assert.That(counter, Is.EqualTo(7));

        //}

        [Test]
        [MSTest.TestMethod]
        public void shouldProduceUniqueIdentitiesForEachNodeAdded()
        {
            ExecutionNode node = ExecutionNode.CreateLevelOneNode(1);
            var node2 = node.AddChild(1);
            var node3 = node2.AddChild(1);
            var node4 = node2.AddChild(1);
            var node5 = node.AddChild(1);

            Assert.That(node, Is.Not.EqualTo(node2));
            Assert.That(node3, Is.Not.EqualTo(node2));
            Assert.That(node4, Is.Not.EqualTo(node2));
            Assert.That(node5, Is.Not.EqualTo(node));

            Assert.That(node, Is.EqualTo(node));
            Assert.That(node2, Is.EqualTo(node2));
        }

        //[Test]
        //[MSTest.TestMethod]
        //public void shouldGetEachExecutionNodeOnlyOnceFromTheIterator()
        //{
        //    ExecutionNode node = ExecutionNode.CreateLevelOneNode(1);
        //    node.AddChild(1).AddChild(1).AddChild(1).AddChild(1).AddChild(1).AddChild(1);

        //    int counter = 0;
        //    NodeIterator it = new NodeIterator(node);
        //    Assert.That(node.Children.Count, Is.GreaterThan(0));

        //    HashSet<ExecutionNode> nodes = new HashSet<ExecutionNode>();

        //    foreach (var item in it.GetNodesRecursive())
        //    {
        //        nodes.Add(item);
        //        counter++;
        //        Console.WriteLine(item.NodeId);
        //    }

        //    Assert.That(counter, Is.EqualTo(7));
        //    Assert.That(nodes.Count, Is.EqualTo(7));
        //}

        //[Test]
        //[MSTest.TestMethod]
        //public void shouldHaveWantedOrderInTheRecursiveness()
        //{
        //    string[] requestedOrder = { "User", "Deposit", "Order", "Transactions", "Tracking", "Withdraw" };

        //    ExecutionNode User = ExecutionNode.CreateLevelOneNode(50, "User");

        //    var deposit = User.AddChild(1, "Deposit");
        //    var Order = User.AddChild(100, "Order");
        //    var withdraw = User.AddChild(1, "Withdraw");
        //    var transactions = Order.AddChild(2, "Transactions");
        //    var tracking = Order.AddChild(1, "Tracking");

        //    NodeIterator it = new NodeIterator(User);

        //    List<ExecutionNode> actual = new List<ExecutionNode>(it.GetNodesRecursive());
            
        //    Assert.That(actual.Count, Is.EqualTo(6));

        //    for (int i = 0; i < requestedOrder.Length; i++)
        //    {
        //        Assert.That(requestedOrder[i], Is.EqualTo(actual[i].NodeName));
        //    }
        //}

        [Test]
        [MSTest.TestMethod]
        public void shouldBeEqual()
        {
            ExecutionNode node1 = ExecutionNode.CreateLevelOneNode(1);
            ExecutionNode node2 = ExecutionNode.CreateLevelOneNode(1);

            ExecutionNode node3 = node1;

            Assert.That(node1, Is.EqualTo(node1));
            Assert.That(node1, Is.EqualTo(node3));
            Assert.That(node3, Is.EqualTo(node1));

            Assert.That(node2, Is.EqualTo(node2));
        }

        [Test]
        [MSTest.TestMethod]
        public void shouldNotBeEqual()
        {
            ExecutionNode node1 = ExecutionNode.CreateLevelOneNode(1);
            ExecutionNode node2 = ExecutionNode.CreateLevelOneNode(1);

            ExecutionNode node3 = node1;
            ExecutionNode node4 = null;
            ExecutionNode node5 = node2;

            Assert.That(node1, Is.Not.EqualTo(node2));
            Assert.That(node2, Is.Not.EqualTo(node1));
            Assert.That(node4, Is.Not.EqualTo(node1));

            Assert.That(node4, Is.Null);
            Assert.That(node1, Is.Not.EqualTo(node4));

            Assert.That(node1, Is.Not.EqualTo(new List<int>()));

            Console.WriteLine(node2);
        }

        [Test]
        [MSTest.TestMethod]
        public void shouldReturnOnlyTheNodeIfOnlyOne()
        {
            ExecutionNode node = ExecutionNode.CreateLevelOneNode(1);
            node.AddTable(new TableEntity());

            int counter = 0;
            NodeIterator it = new NodeIterator(node);
            Assert.That(node.Children.Count, Is.EqualTo(0));

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
        public void ShouldHaveNoNullValuesAfterInitialization()
        {
            ExecutionNode node = ExecutionNode.CreateLevelOneNode(1);

            Assert.That(node, Is.Not.Null);
            Assert.That(node.NodeName, Is.Not.Null);
            Assert.That(node.Level, Is.Not.Null);
            Assert.That(node.NodeId, Is.Not.Null);
            Assert.That(node.Tables, Is.Not.Null);
            Assert.That(node.Children, Is.Not.Null);
            Assert.That(node.RepeatCount, Is.Not.Null);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldHaveParentIfItIsChild()
        {
            ExecutionNode node = ExecutionNode.CreateLevelOneNode(1);
            Assert.That(node.HasChildren, Is.False);

            var child = node.AddChild(1);
            Assert.That(node.HasChildren, Is.True);
            Assert.That(child.Parent, Is.Not.Null);
            Assert.That(child.Parent, Is.EqualTo(node));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGetAllExecutionItems()
        {
            ExecutionNode node = ExecutionNode.CreateLevelOneNode(1);
            node.Tables.Add(new TableEntity());
            node.Tables.Add(new TableEntity());

            var child = node.AddChild(1);
            child.Tables.Add(new TableEntity());
            child.Tables.Add(new TableEntity());
            child.Tables.Add(new TableEntity());

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
    }
}