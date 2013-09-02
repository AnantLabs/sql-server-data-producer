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
    public class TablaEntityDataAccessTests : TestBase
    {
        public TablaEntityDataAccessTests()
            : base()
        {

        }


        [Test]
        [MSTest.TestMethod]
        public void ShouldRun_GetAllTables()
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());
            var tables = tda.GetAllTablesAndColumns();
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldRun_GetTableAndColumns()
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());
            var tables = tda.GetTableAndColumns("Person", "Person");
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldRun_CloneTable()
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());
            var table = tda.GetTableAndColumns("Person", "Person");

            var clone = tda.CloneTable(table);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldRun_CloneTables()
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());
            var tables = tda.GetAllTablesAndColumns();

            var clone = tda.CloneTables(tables);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldRun_GetPrimaryKeysForColumnInTable()
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());
            var table = tda.GetTableAndColumns("Person", "Person");

            var pks = tda.GetPrimaryKeysForColumnInTable(table, table.Columns.First().ColumnName);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldRun_TruncateTable()
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());
            var table = tda.GetTableAndColumns("Person", "Person");

            tda.TruncateTable(table);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldRun_GetLeaf()
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());
            var tables = tda.GetAllTablesAndColumns();

            tda.GetTreeStructureWithTableAsLeaf(new TableEntity("Person", "Person"), tables);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldRun_GetRoot()
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());
            var tables = tda.GetAllTablesAndColumns();

            tda.GetTreeStructureFromRoot(new TableEntity("Person", "Person"), tables);
        }
    }
}
