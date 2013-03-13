using NUnit.Framework;
using SQLDataProducer.DataAccess;
using SQLDataProducer.Entities.DatabaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.RandomTests.SQLDataProducer.DataAccess.Tests
{
    [TestFixture]
    public class ColumnEntityDataAccessTests : TestBase
    {
        public ColumnEntityDataAccessTests()
            : base()
        {

        }


        [Test]
        public void ShouldRun_GetAllColumnsForTable()
        {
            ColumnEntityDataAccess cde = new ColumnEntityDataAccess(Connection());
            var columns = cde.GetAllColumnsForTable(new TableEntity("Person", "Person"));
            //var tables = tda.GetAllTablesAndColumns();

            //var clone = tda.CloneTables(tables);
        }

    }
}
