using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SQLDataProducer.RandomTests.Helpers;
using SQLDataProducer.Entities.ExecutionEntities;


namespace SQLDataProducer.RandomTests.OtherTests
{
    public class DataTableGenerationTests : TestBase
    {
        public DataTableGenerationTests()
            : base()
        {
            
        }
        
        [Test]
        public void ShouldRunASmallTest()
        {
            var table = ExecutionItemHelper.CreateTableAnd5Columns("dbo", "Peter");
            long i = 0;
            //var dt = table.GenerateDataTable(10, new Func<long>(() => { return i++; }));
            var ei = new ExecutionItem(table);
            var dt = ei.GenerateDataTable(new Func<long>(() => { return i++; }));
        }
    }
}
