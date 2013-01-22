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
            var ei = new ExecutionItem(table);
            ei.RepeatCount = 100;
            var dataRows = ei.GenerateDataRows(new Func<long>(() => { return i++; }));
            foreach (var dr in dataRows)
            {
                foreach (var c in ei.TargetTable.Columns)
                {
                    Console.Write("{0}, ", dr[c.ColumnName]);
                }
                Console.WriteLine();
            }
        }

        [Test]
        public void DefaultTest()
        {
            //DataTableGenerator dtGen = new DataTableGenerator();
            Assert.Fail();
        }
    }
}
