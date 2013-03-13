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
    public class TablaEntityDataAccessTests : TestBase
    {
        public TablaEntityDataAccessTests()
            : base ()
        {

        }


        [Test]
        public void ShouldRun_GetAllTables()
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());
            var tables = tda.GetAllTablesAndColumns();
        }

        [Test]
        public void ShouldRun_GetTableAndColumns()
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());
            var tables = tda.GetTableAndColumns("Person", "Person");
        }

        [Test]
        public void ShouldRun_CloneTable()
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());
            var table = tda.GetTableAndColumns("Person", "Person");

            var clone = tda.CloneTable(table);
        }

        [Test]
        public void ShouldRun_CloneTables()
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());
            var tables = tda.GetAllTablesAndColumns();

            var clone = tda.CloneTables(tables);
        }

        [Test]
        public void ShouldRun_GetPrimaryKeysForColumnInTable()
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());
            var table = tda.GetTableAndColumns("Person", "Person");

            var pks = tda.GetPrimaryKeysForColumnInTable(table, table.Columns.First().ColumnName);
        }

        [Test]
        public void ShouldRun_TruncateTable()
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());
            var table = tda.GetTableAndColumns("Person", "Person");

            tda.TruncateTable(table);
        }

        [Test]
        public void ShouldRun_GetLeaf()
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());
            var tables = tda.GetAllTablesAndColumns();

            tda.GetTreeStructureWithTableAsLeaf(new TableEntity("Person", "Person"), tables);
        }

        [Test]
        public void ShouldRun_GetRoot()
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());
            var tables = tda.GetAllTablesAndColumns();

            tda.GetTreeStructureFromRoot(new TableEntity("Person", "Person"), tables);
        }
    }
}
