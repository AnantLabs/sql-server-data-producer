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
            var rows = ei.CreateData(new Func<long>(() => { return i++; }));
            
            Assert.That(rows.Count, Is.EqualTo(100));

            foreach (var datarow in rows)
            {
                for (int j = 0; j < datarow.Cells.Count; j++)
                {
                    Console.WriteLine(datarow.Cells[j].Value);
                }
                
                Console.WriteLine();
            }
        }

        [Test]
        public void DefaultTest()
        {
            //DataTableGenerator dtGen = new DataTableGenerator();
            
        }
    }
}
