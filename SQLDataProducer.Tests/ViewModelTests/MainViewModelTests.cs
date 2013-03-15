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
    public class MainViewModelTests : TestBase
    {

        MainWindowViewModel mainVM;
        Model.ApplicationModel model;

        public MainViewModelTests()
            : base()
        {
            //model = new Model.ApplicationModel();
            var options = new Entities.OptionEntities.ExecutionTaskOptions();

            options.ExecutionType = Entities.ExecutionTypes.ExecutionCountBased;
            options.FixedExecutions = 1;

            mainVM = new MainWindowViewModel(options);

            model = mainVM.Model;
            model.ConnectionString = Connection();
        }

        [Test]
        public void ShouldRun_()
        {

        }

        [Test]
        public void ShouldRun_LoadCommand()
        {
        }

        [Test]
        public void ShouldRun_LoadTablesCommand()
        {
        }
        
        [Test]
        public void ShouldRun_RunCommand()
        {
        }

        [Test]
        public void ShouldRun_SaveCommand()
        {
        }

    }
}
