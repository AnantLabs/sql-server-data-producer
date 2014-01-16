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
using SQLDataProducer.Tests.ViewModelTests;


namespace SQLDataProducer.Tests.ViewModels
{
    [TestFixture]
    [MSTest.TestClass]
    public class ProjectViewModelTest
    {
        IDataService mockDataService = new MockDataService();
        IDataService designDataService = new DesignDataService();


        [Test]
        [MSTest.TestMethod]
        public void ShouldInstansiateViewModelWithDesignData()
        {
            ProjectModel model = null;
            ProjectViewModel viewModel = new ProjectViewModel(designDataService);

            model = viewModel.Model;

            Assert.That(viewModel.SelectedTable, Is.Not.Null);
            Assert.That(viewModel.SelectedExecutionNode, Is.Not.Null);
            Assert.That(viewModel.SelectedColumn, Is.Not.Null);
            Assert.That(viewModel.Model, Is.Not.Null);

            Assert.That(viewModel.SelectedAvailableTable, Is.Null, "Should not be selected");

            Assert.That(viewModel.SelectedExecutionNode, Is.EqualTo(model.RootNode.Children.First()), "Have default values");
            Assert.That(viewModel.SelectedTable, Is.EqualTo(model.RootNode.Children.First().Tables.First()), "Have default values");
            Assert.That(viewModel.SelectedColumn, Is.EqualTo(model.RootNode.Children.First().Tables.First().Columns.First()), "Have default values");
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldInstansiateViewModelWithDefaultRuntimeData()
        {
            ProjectViewModel viewModel = new ProjectViewModel(mockDataService);

            Assert.That(viewModel.Model, Is.Not.Null);
            
            Assert.That(viewModel.SelectedColumn, Is.Null);
            Assert.That(viewModel.SelectedExecutionNode, Is.Null);
            Assert.That(viewModel.SelectedTable, Is.Null);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldSetSelectedTableAndColumnWhenSelectingNode()
        {
            ProjectViewModel viewModel = new ProjectViewModel(designDataService);

            var selectedNode = viewModel.Model.RootNode.Children.First().Children.First();
            Assert.That(selectedNode, Is.Not.EqualTo(viewModel.SelectedExecutionNode), "sanity, Verify that the new selected node is not the same as the old selected node");

            viewModel.SelectedExecutionNode = selectedNode;

            Assert.That(viewModel.SelectedTable, Is.EqualTo(selectedNode.Tables.First()));
            Assert.That(viewModel.SelectedColumn, Is.EqualTo(selectedNode.Tables.First().Columns.First()));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldSetSelectedTableAndColumnToNullIfSelectedNodeHaveNoTables()
        {
            ProjectViewModel viewModel = new ProjectViewModel(designDataService);

            var selectedNode = viewModel.Model.RootNode.AddChild(1);

            viewModel.SelectedExecutionNode = selectedNode;

            Assert.That(viewModel.SelectedTable, Is.Null);
            Assert.That(viewModel.SelectedColumn, Is.Null);
        }


        [Test]
        [MSTest.TestMethod]
        public void ShouldNotCrashSettingSelectedNodeToNull()
        {
            ProjectViewModel viewModel = new ProjectViewModel(designDataService);
            viewModel.SelectedExecutionNode = null;
        }
       
    }
}
