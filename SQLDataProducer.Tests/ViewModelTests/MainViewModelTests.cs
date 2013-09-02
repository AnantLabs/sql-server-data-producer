using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLDataProducer.Helpers;
using SQLDataProducer.ViewModels;
using System.Threading;
using SQLDataProducer.Entities;
using SQLDataProducer.Entities.OptionEntities;


namespace SQLDataProducer.Tests.ViewModelTests
{
    [TestFixture]
    [MSTest.TestClass]
    public class MainViewModelTests : TestBase
    {

        MainWindowViewModel mainVM;
        Model.ApplicationModel model;

        public MainViewModelTests()
            : base()
        {
            //model = new Model.ApplicationModel();
            var options = new ExecutionTaskOptions();

            options.ExecutionType = ExecutionTypes.ExecutionCountBased;
            options.FixedExecutions = 1;

            mainVM = new MainWindowViewModel(options);

            model = mainVM.Model;
            model.ConnectionString = Connection();
        }

        [Test]
        [MSTest.TestMethod]
        [Ignore]
        public void ShouldRun_LoadCommand()
        {
            // Cant test, is showing windows GUI
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldRun_LoadTablesCommand()
        {
            mainVM.LoadTablesCommand.Execute();
            MainViewModelDoEventsUntilQueryIsDone();
            Assert.That(mainVM.Model.Tables.Count, Is.GreaterThan(0));

        }

        private void MainViewModelDoEventsUntilQueryIsDone()
        {
            while (mainVM.Model.IsQueryRunning)
            {
                Thread.Sleep(10);
                DispatcherSupplier.DispatcherUtil.DoEvents();

            }
        }

        [Test]
        [MSTest.TestMethod]
        [Ignore]
        public void ShouldRun_RunCommand()
        {
            // cannot test, is showing windows
        }

        [Test]
        [MSTest.TestMethod]
        [Ignore]
        public void ShouldRun_SaveCommand()
        {
            // Cant test, is showing windows GUI
        }

    }
}
