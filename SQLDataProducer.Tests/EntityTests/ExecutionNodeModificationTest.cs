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
    public class ExecutionNodeModificationTest
    {
        private TableEntity _CustomerTable = new TableEntity("dbo", "Customer");
        private TableEntity _AccountTable = new TableEntity("dbo", "Accounts");
       
        /* To be supported:
         * RemoveTable
         * AddTable
         * Move Table ToParent node
         * Move Table to new child node
         * Move table from Node A to node B (drag and drop)
         * Move tables (several) to new Node
         * Move tables to Node B (drag n drop)
         * Delete several tables.
         * 
         * 
         * Delete Node
         * Add node
         * Merge Node with Parent Node. (move tables from child to parent node)
         * Move Node to other parent (drag n drop)
         * Move Node at same level up and down (Node 1 have 2 child nodes(3, 4), 
         *                    move one of them so that the order becomes (4, 3))
         */


        [Test]
        [MSTest.TestMethod]
        public void ShouldBeAbleToRemoveOneTableFromExecutionNode()
        {
            // 2 Customers
            ExecutionNode customer = ExecutionNode.CreateLevelOneNode(2, "Customer");
            customer.AddTable(_CustomerTable);

            // Make 2 accounts per customer
            ExecutionNode accounts = customer.AddChild(2, "Accounts");
            accounts.AddTable(_AccountTable);

            NodeIterator it = new NodeIterator(customer);
            Assert.That(it.GetExpectedInsertCount(), Is.EqualTo(6));
            AssertOrder(it.GetTablesRecursive(), "Customer", "Accounts", "Accounts", "Customer", "Accounts", "Accounts");

            accounts.RemoveTable(_AccountTable);

            Assert.That(it.GetExpectedInsertCount(), Is.EqualTo(2));
            AssertOrder(it.GetTablesRecursive(), "Customer", "Customer");

        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldBeAbleToMoveOneTableToParentNode()
        {
            // 2 Customers
            ExecutionNode customer = ExecutionNode.CreateLevelOneNode(2, "Customer");
            customer.AddTable(_CustomerTable);

            // Make 2 accounts per customer
            ExecutionNode accounts = customer.AddChild(2, "Accounts");
            accounts.AddTable(_AccountTable);

            NodeIterator it = new NodeIterator(customer);
            Assert.That(it.GetExpectedInsertCount(), Is.EqualTo(6));
            AssertOrder(it.GetTablesRecursive(), "Customer", "Accounts", "Accounts", "Customer", "Accounts", "Accounts");

            accounts.MoveTableToParentNode(_AccountTable);

            Assert.That(it.GetExpectedInsertCount(), Is.EqualTo(4));
            AssertOrder(it.GetTablesRecursive(), "Customer", "Accounts", "Customer", "Accounts");
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldBeAbleToMergeNodeWithParent()
        {
            // 2 Customers
            ExecutionNode customer = ExecutionNode.CreateLevelOneNode(2, "Customer");
            customer.AddTable(_CustomerTable);

            // Make 2 accounts per customer
            ExecutionNode accounts = customer.AddChild(2, "Accounts");
            accounts.AddTable(_AccountTable);

            NodeIterator it = new NodeIterator(customer);
            Assert.That(it.GetExpectedInsertCount(), Is.EqualTo(6));
            AssertOrder(it.GetTablesRecursive(), "Customer", "Accounts", "Accounts", "Customer", "Accounts", "Accounts");

            accounts.MoveTableToParentNode(_AccountTable);

            Assert.That(it.GetExpectedInsertCount(), Is.EqualTo(4));
            AssertOrder(it.GetTablesRecursive(), "Customer", "Accounts", "Customer", "Accounts");

        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldBeAbleToMoveTableToOtherNode()
        {
            // 2 Customers
            ExecutionNode customer = ExecutionNode.CreateLevelOneNode(2, "Customer");
            customer.AddTable(_CustomerTable);

            // Make 2 accounts per customer
            ExecutionNode accounts = customer.AddChild(2, "Accounts");
            accounts.AddTable(_AccountTable);

            NodeIterator it = new NodeIterator(customer);
            Assert.That(it.GetExpectedInsertCount(), Is.EqualTo(6));
            AssertOrder(it.GetTablesRecursive(), "Customer", "Accounts", "Accounts", "Customer", "Accounts", "Accounts");



            accounts.MoveTableToNode(_AccountTable, customer);

            Assert.That(it.GetExpectedInsertCount(), Is.EqualTo(4));
            AssertOrder(it.GetTablesRecursive(), "Customer", "Accounts", "Customer", "Accounts");

        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldBeAbleToMoveTableToNewChildNode()
        {
            // 2 Customers
            ExecutionNode customer = ExecutionNode.CreateLevelOneNode(2, "Customer");
            customer.AddTable(_CustomerTable);

            // Make 2 accounts per customer
            ExecutionNode accounts = customer.AddChild(2, "Accounts");
            accounts.AddTable(_AccountTable);

            NodeIterator it = new NodeIterator(customer);
            Assert.That(it.GetExpectedInsertCount(), Is.EqualTo(6));
            AssertOrder(it.GetTablesRecursive(), "Customer", "Accounts", "Accounts", "Customer", "Accounts", "Accounts");

            // total 6 accounts per customer, 2 customers 12 + 2

            accounts.MoveToNewChildNode(_AccountTable, 3 ,"AccountChildNode");
           
            Assert.That(it.GetExpectedInsertCount(), Is.EqualTo(14));
            AssertOrder(it.GetTablesRecursive(), "Customer", "Accounts", "Accounts", "Accounts", "Accounts", "Accounts", "Accounts", "Customer", "Accounts", "Accounts", "Accounts", "Accounts", "Accounts", "Accounts");

        }

        private void AssertOrder(IEnumerable<TableEntity> actualOrder, params string[] expectedOrder)
        {
            int i = 0;
            foreach (var actualTable in actualOrder)
            {
                Assert.That(actualTable.TableName, Is.EqualTo(expectedOrder[i++]));
            }
        }
    }
}
