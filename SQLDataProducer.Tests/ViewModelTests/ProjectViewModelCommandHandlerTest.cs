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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DatabaseEntities.Factories;
using SQLDataProducer.GUI.ViewModel;
using SQLDataProducer.GUI.Design;
using SQLDataProducer.GUI.Model;


namespace SQLDataProducer.Tests.ViewModels
{
    [TestFixture]
    [MSTest.TestClass]
    public class ProjectViewModelCommandHandlerTest
    {

        IDataService dataService = new DesignDataService();

        [Test]
        [MSTest.TestMethod]
        public void ShouldCheckSanityOfInitialData()
        {
            var viewModel = new ProjectViewModel(dataService);
            Assert.That(viewModel.Model.Tables.Count, Is.GreaterThanOrEqualTo(2));
            Assert.That(viewModel.Model.Tables.First().Columns.Count(), Is.GreaterThan(0));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldInstansiate()
        {
            var viewModel = new ProjectViewModel(dataService);
            ProjectViewModelCommandHandler commandHandler = new ProjectViewModelCommandHandler(viewModel);
            
            
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldDoNothingWhenTryingToAddTableWhenNoNodeIsSelected()
        {
            var viewModel = new ProjectViewModel(dataService);
            ProjectViewModelCommandHandler commandHandler = new ProjectViewModelCommandHandler(viewModel);

            var tableToAdd = viewModel.Model.Tables.First();
            commandHandler.AddTableToNode(tableToAdd);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldAddChildNodeAndMarkItAsSelected()
        {
            var viewModel = new ProjectViewModel(dataService);
            ProjectViewModelCommandHandler commandHandler = new ProjectViewModelCommandHandler(viewModel);

            int nodeCountBeforeAdd = viewModel.SelectedExecutionNode.Children.Count();

            var nodeToManipulate = viewModel.SelectedExecutionNode;

            commandHandler.AddChildNode(nodeToManipulate);

            Assert.That(nodeToManipulate.Children.Count(), Is.GreaterThan(nodeCountBeforeAdd));
            Assert.That(nodeToManipulate.Children.Last().RepeatCount, Is.EqualTo(1));
            Assert.That(nodeToManipulate.Children.Last().NodeName, Is.EqualTo(string.Empty));

            var addedNode = nodeToManipulate.Children.Last();
            Assert.That(viewModel.SelectedExecutionNode, Is.EqualTo(addedNode));

        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldMoveTableUpAndDownInTheListOfTables()
        {
            var viewModel = new ProjectViewModel(dataService);
            ProjectViewModelCommandHandler commandHandler = new ProjectViewModelCommandHandler(viewModel);

            Assert.That(viewModel.SelectedExecutionNode.Tables.Count(), Is.EqualTo(2));

            var tableToMove = viewModel.SelectedExecutionNode.Tables.Last();
            commandHandler.MoveTableUp(tableToMove);
            Assert.That(viewModel.SelectedExecutionNode.Tables.First(), Is.EqualTo(tableToMove));

            commandHandler.MoveTableDown(tableToMove);
            Assert.That(viewModel.SelectedExecutionNode.Tables.Last(), Is.EqualTo(tableToMove));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldDeleteTableFromExecutionNode()
        {
            var viewModel = new ProjectViewModel(dataService);
            ProjectViewModelCommandHandler commandHandler = new ProjectViewModelCommandHandler(viewModel);

            Assert.That(viewModel.SelectedExecutionNode.Tables.Count(), Is.EqualTo(2));

            var tableToDelete = viewModel.SelectedExecutionNode.Tables.First();
            commandHandler.RemoveTableFromSelectedNode(tableToDelete);
            
            Assert.That(viewModel.SelectedExecutionNode.Tables.Count(), Is.EqualTo(1));
            Assert.That(viewModel.SelectedExecutionNode.Tables.Contains(tableToDelete), Is.False);

            tableToDelete = viewModel.SelectedExecutionNode.Tables.First();
            commandHandler.RemoveTableFromSelectedNode(tableToDelete);
            Assert.That(viewModel.SelectedExecutionNode.Tables.Count(), Is.EqualTo(0));
            Assert.That(viewModel.SelectedExecutionNode.Tables.Contains(tableToDelete), Is.False);

            commandHandler.RemoveTableFromSelectedNode(tableToDelete);
            Assert.That(viewModel.SelectedExecutionNode.Tables.Count, Is.EqualTo(0), "Removing a table that does not exist in the list should not do anything");
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldAddTableToNode()
        {
            var viewModel = new ProjectViewModel(dataService);
            ProjectViewModelCommandHandler commandHandler = new ProjectViewModelCommandHandler(viewModel);
            var tableToAdd = new TableEntity("dbo", "Countries");
            
            commandHandler.AddTableToNode(tableToAdd);

            Assert.That(viewModel.SelectedExecutionNode.Tables.Where(x => x.FullName.Equals(tableToAdd.FullName)).Count(), Is.EqualTo(1), "Added table should be in collection");
            Assert.That(viewModel.SelectedExecutionNode.Tables.Contains(tableToAdd), Is.False, "Added table should not be the same table, it should be cloned and be a new entity");
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldAddParentNode()
        {
            var viewModel = new ProjectViewModel(dataService);
            ProjectViewModelCommandHandler commandHandler = new ProjectViewModelCommandHandler(viewModel);

            var oldSelectedNode = viewModel.SelectedExecutionNode;

            commandHandler.AddParentNode(viewModel.SelectedExecutionNode);
            Assert.That(viewModel.SelectedExecutionNode, Is.Not.EqualTo(oldSelectedNode));
        }
        
    }
}
