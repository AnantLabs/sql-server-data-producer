// Copyright 2012-2013 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLDataProducer.Entities.ExecutionEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Tests.ConsumerTests
{
    [MSTest.TestClass]
    public class DataToConsoleComsumerTest : TestBase
    {

        public DataToConsoleComsumerTest()
            : base()
        {

        }

        [Test]
        [MSTest.TestMethod]
        public void StandardTest()
        {
            //var table = ExecutionItemHelper.CreateTableAnd5Columns("dbo", "Peter");
            //long i = 0;
            //var ei = new ExecutionItem(table);
            //ei.RepeatCount = 100;
            //var rows = ei.CreateData(new Func<long>(() => { return i++; }));

            //Assert.That(rows.Count, Is.EqualTo(100));


            //DataToConsoleConsumer consumer = new DataToConsoleConsumer();
            //consumer.Consume(rows);

            Assert.Fail("Not implemented this test yet");

        }

    }
}
