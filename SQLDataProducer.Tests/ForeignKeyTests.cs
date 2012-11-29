// Copyright 2012 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SQLDataProducer.DataAccess;
using SQLDataProducer.TaskExecuter;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.Entities.OptionEntities;
using SQLDataProducer.Entities;
using SQLDataProducer.Entities.Generators;

namespace SQLDataProducer.RandomTests
{
    public class ForeignKeyTests : TestBase
    {
        public ForeignKeyTests()
            :base()
        {
        }

        [Test]
        public void ShouldHaveForeignKeysFor()
        {

        }

        [Test]
        public void ShouldGetIdentityFromPreviousItem()
        {
            var wfm = new WorkflowManager();
            var tda = new TableEntityDataAccess(Connection());
            var person = tda.GetTableAndColumns("Person", "NewPerson");
            var anotherTable = tda.GetTableAndColumns("Person", "AnotherTable");
            
            Assert.Greater(person.Columns.Count, 0);
            Assert.Greater(anotherTable.Columns.Count, 0);

            var i1 = new ExecutionItem(person);
            i1.RepeatCount = 10;
            var i2 = new ExecutionItem(anotherTable);
            i2.RepeatCount = 2;

            anotherTable.Columns[0].Generator = 
                anotherTable.Columns[0].PossibleGenerators.Where
                    ( 
                    g => g.GeneratorName == Generator.GENERATOR_IdentityFromPreviousItem
                    ).FirstOrDefault();

            Assert.IsNotNull(anotherTable.Columns[0].Generator);
            Assert.AreEqual(Generator.GENERATOR_IdentityFromPreviousItem, anotherTable.Columns[0].Generator.GeneratorName);

            var items = new ExecutionItemCollection();
            items.Add(i1);
            items.Add(i2);

            {
                var options = new ExecutionTaskOptions();
                options.ExecutionType = ExecutionTypes.ExecutionCountBased;
                options.FixedExecutions = 1;
                options.MaxThreads = 1;
                options.NumberGeneratorMethod = NumberGeneratorMethods.NewNForEachRow;
                
                // new N for each row
                var res = wfm.RunWorkFlow(options, Connection(), items);

                Console.WriteLine(res.ToString());
                Assert.AreEqual(12, res.InsertCount, "InsertCount should be 2");
                Assert.AreEqual(0, res.ErrorList.Count, "InsertCount should be 0");
                Assert.AreEqual(1, res.ExecutedItemCount, "ExecutedItemCount should be 1");
                Assert.Greater(res.Duration, TimeSpan.Zero, "Duration should > 0");
            }
        }


        //[Test]
        //public void ShouldGeValueFromPreviousItem()
        //{
        //    var wfm = new WorkflowManager();
        //    var tda = new TableEntityDataAccess(Connection());
        //    var person = tda.GetTableAndColumns("Person", "NewPerson");
        //    var anotherTable = tda.GetTableAndColumns("Person", "AnotherTable");
        //    var i1 = new ExecutionItem(person);
        //    var i2 = new ExecutionItem(anotherTable);

        //    anotherTable.Columns[0].Generator =
        //        anotherTable.Columns[0].PossibleGenerators.Where
        //            (
        //            g => g.GeneratorName == Generator.GENERATOR_ValueFromOtherColumn
        //            ).FirstOrDefault();
        //    anotherTable.Columns[0].Generator.GeneratorParameters[0].Value = 


        //    Assert.IsNotNull(anotherTable.Columns[0].Generator);
        //    Assert.AreEqual(Generator.GENERATOR_IdentityFromPreviousItem, anotherTable.Columns[0].Generator.GeneratorName);

        //    var items = new ExecutionItemCollection();
        //    items.Add(i1);
        //    items.Add(i2);

        //    {
        //        var options = new ExecutionTaskOptions();
        //        options.ExecutionType = ExecutionTypes.ExecutionCountBased;
        //        options.FixedExecutions = 1;
        //        options.MaxThreads = 1;
        //        options.NumberGeneratorMethod = NumberGeneratorMethods.NewNForEachRow;

        //        // new N for each row
        //        var res = wfm.RunWorkFlow(options, Connection(), items);

        //        Console.WriteLine(res.ToString());
        //        Assert.AreEqual(2, res.InsertCount, "InsertCount should be 2");
        //        Assert.AreEqual(0, res.ErrorList.Count, "InsertCount should be 0");
        //        Assert.AreEqual(1, res.ExecutedItemCount, "ExecutedItemCount should be 1");
        //        Assert.Greater(res.Duration, TimeSpan.Zero, "Duration should > 0");
        //    }
        //}
       
    }
}
