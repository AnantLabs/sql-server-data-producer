using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLDataProducer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLDataProducer.Entities;
using SQLDataProducer.Entities.OptionEntities;

namespace SQLDataProducer.Tests.ViewModelTests
{
    [TestFixture]
    [MSTest.TestClass]
    public class SidePanelViewModelTests : TestBase
    {
        public SidePanelViewModelTests()
        {
            //model = new Model.ApplicationModel();
            var options = new ExecutionTaskOptions();

            options.ExecutionType = ExecutionTypes.ExecutionCountBased;
            options.FixedExecutions = 1;

            mainVM = new MainWindowViewModel(options);

            model = mainVM.Model;
            model.ConnectionString = Connection();

            sidepanelVM = new SidePanelViewModel(model);
        }

        SidePanelViewModel sidepanelVM;
        MainWindowViewModel mainVM;
        Model.ApplicationModel model;

        [Test]
        [MSTest.TestMethod]
        public void ShouldStopExecution_OnlyIfItHaveStarted()
        {
            sidepanelVM.StopExecutionCommand.Execute();
            // Failing test added
        }

    }
}
