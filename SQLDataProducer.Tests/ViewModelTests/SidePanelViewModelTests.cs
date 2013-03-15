using NUnit.Framework;
using SQLDataProducer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataProducer.RandomTests.ViewModelTests
{
    [TestFixture]
    public class SidePanelViewModelTests : TestBase
    {
        public SidePanelViewModelTests()
        {
            //model = new Model.ApplicationModel();
            var options = new Entities.OptionEntities.ExecutionTaskOptions();

            options.ExecutionType = Entities.ExecutionTypes.ExecutionCountBased;
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
        public void ShouldStopExecution_OnlyIfItHaveStarted()
        {
            sidepanelVM.StopExecutionCommand.Execute();
            // Failing test added
        }

    }
}
