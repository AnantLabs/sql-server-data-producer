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


using System.Threading;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.ViewModels;
using SQLDataProducer.Entities;




namespace SQLDataProducer.Tests.ViewModelTests
{
    [TestFixture]
    [MSTest.TestClass]
    public class ExecutionOrderViewModelTests : TestBase
    {
        public ExecutionOrderViewModelTests()
            : base()
        {
            //model = new Model.ApplicationModel();
            var options = new SQLDataProducer.Entities.OptionEntities.ExecutionTaskOptions();

            options.ExecutionType = ExecutionTypes.ExecutionCountBased;
            options.FixedExecutions = 1;

            mainVM = new MainWindowViewModel(options);

            model = mainVM.Model;
            model.ConnectionString = Connection();

            orderVM = new ExecutionOrderViewModel(model);

        }

        ExecutionOrderViewModel orderVM;
        MainWindowViewModel mainVM;
        Model.ApplicationModel model;


        [Test]
        [MSTest.TestMethod]
        public void ShouldRun_AddItemToRightCommand()
        {
            LoadTables();

            var table = AddFirstTableAsExecutionItem();

            Assert.That(mainVM.Model.ExecutionItems.Count, Is.EqualTo(1));
            Assert.That(mainVM.Model.ExecutionItems.First().TargetTable, Is.EqualTo(table));
        }

        private void MainViewModelDoEventsUntilQueryIsDone()
        {
            while (mainVM.Model.IsQueryRunning)
            {
                Thread.Sleep(10);
                SQLDataProducer.Helpers.DispatcherSupplier.DispatcherUtil.DoEvents();

            }
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldRun_MoveAllItemsLeftCommand()
        {
            LoadTables();
            var table = mainVM.Model.Tables.FirstOrDefault();
            Assert.That(table, Is.Not.Null);

            orderVM.MoveItemRightCommand.Execute(table);
            Assert.That(mainVM.Model.ExecutionItems.Count, Is.EqualTo(1));
            Assert.That(mainVM.Model.ExecutionItems.First().TargetTable, Is.EqualTo(table));

            orderVM.MoveAllItemsLeftCommand.Execute();
            Assert.That(mainVM.Model.ExecutionItems.Count, Is.EqualTo(0));

        }
        [Test]
        [MSTest.TestMethod]
        public void ShouldRun_MoveAllItemsRightCommand()
        {
            LoadTables();

            Assert.That(mainVM.Model.Tables.Count, Is.GreaterThan(0));

            orderVM.MoveAllItemsRightCommand.Execute();

            MainViewModelDoEventsUntilQueryIsDone();

            Assert.That(mainVM.Model.ExecutionItems.Count, Is.GreaterThan(0));
            Assert.That(mainVM.Model.Tables.Count, Is.EqualTo(mainVM.Model.ExecutionItems.Count));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldRun_MoveDownWithTheSelectorCommand()
        {
            LoadTables();
            orderVM.MoveDownWithTheSelectorCommand.Execute();

        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldRun_MoveItemDownCommand()
        {
            LoadTables();
            AddFirstTableAsExecutionItem();
            AddFirstTableAsExecutionItem();

            Assert.That(mainVM.Model.ExecutionItems.Count, Is.EqualTo(2));

            mainVM.Model.SelectedExecutionItem = mainVM.Model.ExecutionItems.First();

            orderVM.MoveItemDownCommand.Execute();
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldRun_MoveItemLeftCommand()
        {
            LoadTables();
            AddFirstTableAsExecutionItem();
            AddFirstTableAsExecutionItem();
            Assert.That(mainVM.Model.ExecutionItems.Count, Is.EqualTo(2));

            mainVM.Model.SelectedExecutionItem = mainVM.Model.ExecutionItems.Skip(1).First();

            orderVM.MoveItemLeftCommand.Execute();

            Assert.That(mainVM.Model.ExecutionItems.Count, Is.EqualTo(1));
        }


        [Test]
        [MSTest.TestMethod]
        public void ShouldRun_MoveItemUpCommand()
        {
            LoadTables();
            AddFirstTableAsExecutionItem();
            AddFirstTableAsExecutionItem();

            mainVM.Model.SelectedExecutionItem = mainVM.Model.ExecutionItems.Skip(1).First();

            orderVM.MoveItemUpCommand.Execute();
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldRun_MoveUpWithTheSelectorCommand()
        {
            LoadTables();
            orderVM.MoveUpWithTheSelectorCommand.Execute();
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldRun_CreateTreeWithTableTableAsRootCommand()
        {
            LoadTables();
            AddFirstTableAsExecutionItem();
            var exec = mainVM.Model.ExecutionItems.First();
            Assert.That(exec, Is.Not.Null);

            orderVM.CreateTreeWithTableTableAsRootCommand.Execute(exec.TargetTable);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldRun_CreateTreeWithTableAsLeafCommand()
        {
            LoadTables();
            AddFirstTableAsExecutionItem();
            var exec = mainVM.Model.ExecutionItems.First();
            Assert.That(exec, Is.Not.Null);

            orderVM.CreateTreeWithTableAsLeafCommand.Execute(exec.TargetTable);
        }

        //[Test] [MSTest.TestMethod]
        //public void ShouldRun_CloneExecutionItemCommand()
        //{
        //    LoadTables();
        //    AddFirstTableAsExecutionItem();
        //    var exec = mainVM.Model.ExecutionItems.First();
        //    Assert.That(exec, Is.Not.Null);

        //    orderVM.CloneExecutionItemCommand.Execute(exec);

        //}

        private TableEntity AddFirstTableAsExecutionItem()
        {
            var table = mainVM.Model.Tables.FirstOrDefault();
            Assert.That(table, Is.Not.Null);

            orderVM.MoveItemRightCommand.Execute(table);

            MainViewModelDoEventsUntilQueryIsDone();

            return table;
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldRun_ClearSearchCriteraCommand()
        {
            LoadTables();
            orderVM.ClearSearchCriteraCommand.Execute();
            Assert.That(mainVM.Model.SearchCriteria, Is.EqualTo(string.Empty));
        }


        private void LoadTables()
        {
            mainVM.LoadTablesCommand.Execute();
            MainViewModelDoEventsUntilQueryIsDone();

            Assert.That(model.Tables.Count, Is.GreaterThan(0));
        }
    }
}
