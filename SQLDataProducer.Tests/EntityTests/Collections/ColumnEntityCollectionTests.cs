// Copyright 2012-2014 Peter Henell

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
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using SQLDataProducer.Entities.DatabaseEntities.Collections;
using SQLDataProducer.Entities.DatabaseEntities.Factories;
using SQLDataProducer.Entities.DatabaseEntities;


namespace SQLDataProducer.Tests.Entities.Collections
{
    [TestFixture]
    [MSTest.TestClass]
    public class ColumnEntityCollectionTests : TestBase
    {
        [Test]
        [MSTest.TestMethod]
        public void ShouldAddColumnToCollection()
        {
            var columns = new ColumnEntityCollection();
            columns.Add(DatabaseEntityFactory.CreateColumnEntity("CustomerId", new ColumnDataTypeDefinition("int", false), true, 1, false, null, null));

            Assert.That(columns.Count(), Is.EqualTo(1));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGetColumnFromCollection()
        {
            var columns = new ColumnEntityCollection();
            var column = DatabaseEntityFactory.CreateColumnEntity("CustomerId", new ColumnDataTypeDefinition("int", false), true, 1, false, null, null);
            columns.Add(column);

            Assert.That(columns[0], Is.EqualTo(column));
            Assert.That(columns.Get("CustomerId"), Is.EqualTo(column));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGetExceptionWhenTryingToGetColumnThatDoesNotExist()
        {
            bool exceptionHappened = false;
            var columns = new ColumnEntityCollection();

            try
            {
                Assert.That(columns[0], Is.Null);
                Assert.That(columns.Get("CustomerId"), Is.Null);
            }
            catch (KeyNotFoundException)
            {
                exceptionHappened = true;
            }
            Assert.That(exceptionHappened);
        }
    }
}
