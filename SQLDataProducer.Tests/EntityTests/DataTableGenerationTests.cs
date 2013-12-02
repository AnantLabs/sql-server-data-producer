//// Copyright 2012-2013 Peter Henell

////   Licensed under the Apache License, Version 2.0 (the "License");
////   you may not use this file except in compliance with the License.
////   You may obtain a copy of the License at

////       http://www.apache.org/licenses/LICENSE-2.0

////   Unless required by applicable law or agreed to in writing, software
////   distributed under the License is distributed on an "AS IS" BASIS,
////   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
////   See the License for the specific language governing permissions and
////   limitations under the License.

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NUnit.Framework;
//using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;

//using SQLDataProducer.Entities.ExecutionEntities;
//using SQLDataProducer.Entities;
//using SQLDataProducer.Tests.Helpers;
//using SQLDataProducer.Entities.Generators;


//namespace SQLDataProducer.Tests.EntitiesTests
//{
//    [TestFixture]
//    [MSTest.TestClass]
//    public class DataTableGenerationTests : TestBase
//    {
//        public DataTableGenerationTests()
//            : base()
//        {

//        }

//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldCreateSomeData()
//        {
//            var table = ExecutionItemHelper.CreateTableWithIdenitityAnd5Columns("dbo", "Peter");
//            long i = 0;
//            var ei = new ExecutionNode(table);
//            ei.RepeatCount = 100;
//            var rows = ei.CreateData(new Func<long>(() => { return i++; }), new SetCounter());

//            Assert.That(rows.Count, Is.EqualTo(100));

//            // If the columns is Identity then it should not exist in the collection if it is the SQL Server Identity generator.
//            // In those cases the column should be ignored and be generated when inserted.
//            Assert.That(rows[0].Cells.Any(c => !(c.Column.IsIdentity && c.Column.Generator.GeneratorName == Generator.GENERATOR_IdentityFromSqlServerGenerator)), Is.True);

//            foreach (var datarow in rows)
//            {
//                for (int j = 0; j < datarow.Cells.Count; j++)
//                {
//                    Console.WriteLine(datarow.Cells[j].Value);
//                }

//                Console.WriteLine();
//            }
//        }

//        //[Test] [MSTest.TestMethod]
//        //public void DataToConsoleComsumerTest

       
//    }
//}
