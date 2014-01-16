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
            Assert.AreEqual(1, node.Children.Count(), "Number of children");

            node.AddChild(1);
            node.AddChild(1);
            Assert.AreEqual(3, node.Children.Count(), "Number of children");
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

        [Test]
        [MSTest.TestMethod]
        public void ShouldNotBeAbleToAddSameTableTwice()
        {
            ExecutionNode node1 = ExecutionNode.CreateLevelOneNode(1);
            TableEntity customerTable = new TableEntity("dbo", "Customer");
            node1.AddTable(customerTable);
            node1.AddTable(customerTable);
            node1.AddTable(customerTable);
            Assert.That(node1.Tables.Count(), Is.EqualTo(1));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldMoveTableUpInTheListOfTables()
        {
            ExecutionNode node1 = ExecutionNode.CreateLevelOneNode(1);
            TableEntity customerTable = new TableEntity("dbo", "Customer");
            var orderTable = new TableEntity("dbo", "Order");
            node1.AddTable(orderTable);
            node1.AddTable(customerTable);

            Assert.That(node1.Tables.First(), Is.EqualTo(orderTable));

            node1.MoveTableUp(customerTable);
            
            Assert.That(node1.Tables.Count(), Is.EqualTo(2));
            Assert.That(node1.Tables.First(), Is.EqualTo(customerTable));

            node1.MoveTableUp(customerTable);
            Assert.That(node1.Tables.First(), Is.EqualTo(customerTable), "moving a table above the top should leave it where it is");
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldMoveTableDownInTheListOfTables()
        {
            ExecutionNode node1 = ExecutionNode.CreateLevelOneNode(1);
            TableEntity customerTable = new TableEntity("dbo", "Customer");
            var orderTable = new TableEntity("dbo", "Order");
            node1.AddTable(orderTable);
            node1.AddTable(customerTable);

            Assert.That(node1.Tables.Last(), Is.EqualTo(customerTable));

            node1.MoveTableDown(orderTable);

            Assert.That(node1.Tables.Count(), Is.EqualTo(2));
            Assert.That(node1.Tables.Last(), Is.EqualTo(orderTable));

            node1.MoveTableUp(customerTable);
            Assert.That(node1.Tables.Last(), Is.EqualTo(orderTable), "moving a table below the bottom should leave it where it is");
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldNotMoveATableThatIsNotPartOfTheNode()
        {
            ExecutionNode node1 = ExecutionNode.CreateLevelOneNode(1);
            var customerTable = new TableEntity("dbo", "Customer");
            var orderTable = new TableEntity("dbo", "Order");
            node1.AddTable(customerTable);

            node1.MoveTableUp(orderTable);
            node1.MoveTableUp(orderTable);

            node1.MoveTableDown(orderTable);
            node1.MoveTableDown(orderTable);

            Assert.That(node1.Tables.Count(), Is.EqualTo(1));
        }

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

            object boxed = node1;
            Assert.That(boxed, Is.EqualTo(node1));
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
            ExecutionNode root = ExecutionNode.CreateLevelOneNode(1);
            Assert.That(root.HasChildren, Is.False);

            var child = root.AddChild(1);
            Assert.That(root.HasChildren, Is.True);
            Assert.That(child.Parent, Is.EqualTo(root));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldAddParentNodeToNode()
        {
            ExecutionNode root = ExecutionNode.CreateLevelOneNode(1, "Root");
            var customer = root.AddChild(1, "Customer");
            var order = customer.AddChild(10, "Order");

            string expected =
@"-Customer
--Order";
            Assert.That(customer.getDebugString(), Is.EqualTo(expected));

            order.AddParent(1, "CustomerGroup");
            expected =
@"Root
-Customer
--CustomerGroup
---Order";
            Assert.That(root.getDebugString(), Is.EqualTo(expected));
            customer.AddParent(1, "Country");
            expected =
@"Root
-Country
--Customer
---CustomerGroup
----Order";
            Assert.That(root.getDebugString(), Is.EqualTo(expected));

        }


        [Test]
        [MSTest.TestMethod]
        public void ShouldNotBeAbleToAddParentToRoot()
        {
            ExecutionNode root = ExecutionNode.CreateLevelOneNode(1, "Root");
            var customer = root.AddChild(1, "Customer");
            var order = customer.AddChild(10, "Order");

            string beforeAddingParent = root.getDebugString();

            var returnNode = root.AddParent(1, "Nono");
            Assert.That(returnNode, Is.EqualTo(root));
            Assert.That(root.getDebugString(), Is.EqualTo(beforeAddingParent));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldCreateDebugString()
        {
            ExecutionNode root = ExecutionNode.CreateLevelOneNode(1, "Root");
            var customer = root.AddChild(1, "Customer");
            var order = customer.AddChild(10, "Order");
            root.AddChild(1, "Invoice");

            string expected =
@"Root
-Customer
--Order
-Invoice";
            string actual = root.getDebugString();
            Console.WriteLine("{" + actual + "}");
            Console.WriteLine("{" + expected + "}");

            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(order.getDebugString(), Is.EqualTo("--Order"));

            Assert.That(customer.getDebugString(), Is.EqualTo("-Customer" + Environment.NewLine + "--Order"));

        }

        

    }
}