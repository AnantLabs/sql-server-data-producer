using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLDataProducer.DataAccess;
using SQLDataProducer.Entities.DatabaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Tests.DataAccessTests
{
    [TestFixture]
    [MSTest.TestClass]
    public class ColumnEntityDataAccessTests : TestBase
    {
        public ColumnEntityDataAccessTests()
            : base()
        {

        }


        [Test]
        [MSTest.TestMethod]
        public void ShouldRun_GetAllColumnsForTable()
        {
            ColumnEntityDataAccess cde = new ColumnEntityDataAccess(Connection());
            var columns = cde.GetAllColumnsForTable(new TableEntity("Person", "Person"));
            //var tables = tda.GetAllTablesAndColumns();

            //var clone = tda.CloneTables(tables);
        }

    }
}
