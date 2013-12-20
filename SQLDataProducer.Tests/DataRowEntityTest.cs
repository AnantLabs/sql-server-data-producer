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
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.Generators.DecimalGenerators;
using SQLDataProducer.Entities.DataEntities;
using System;

namespace SQLDataProducer.Tests
{
    [TestFixture]
    [MSTest.TestClass]
    public class DataRowEntityTest
    {
        public DataRowEntityTest()
        {
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldInstanciate()
        {
            var table = new TableEntity("dbo", "Customer");
            var row = new DataRowEntity(table);
            Assert.That(row.Table, Is.EqualTo(table));
            Assert.That(row.Fields, Is.Not.Null);
            Assert.That(row.Fields.Count, Is.EqualTo(0));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldEqualIfSameInstance()
        {
            var table = new TableEntity("dbo", "Customer");
            var row2 = new DataRowEntity(table);
            var row = new DataRowEntity(table);

            Assert.That(row, Is.Not.EqualTo(row2));
            Assert.That(row, Is.EqualTo(row));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldAddFieldsToRow()
        {
            var table = new TableEntity("dbo", "Customer");
            var row = new DataRowEntity(table);
            row.AddField("customerid", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), true);

            Assert.That(row.Fields.Count, Is.EqualTo(1));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldBeAbleToIterateThroughTheFields()
        {
            var table = new TableEntity("dbo", "Customer");
            var row = new DataRowEntity(table);
            row.AddField("customerid", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), true);
            row.AddField("name", Guid.NewGuid(), new ColumnDataTypeDefinition("varchar(100)", false), false);
            row.AddField("vip", Guid.NewGuid(), new ColumnDataTypeDefinition("bit", false), false);
            row.AddField("lastseen", Guid.NewGuid(), new ColumnDataTypeDefinition("datetime", false), false);

            Assert.That(row.Fields.Count, Is.EqualTo(4));
            foreach (var f in row.Fields)
            {
                Assert.That(f, Is.Not.Null);
            }
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldBeAbleToAddSameFieldMultipleTimes_ForNow()
        {
            var table = new TableEntity("dbo", "Customer");
            var row = new DataRowEntity(table);
            row.AddField("customerid", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), true);
            row.AddField("customerid", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), true);
            row.AddField("customerid", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), true);
            row.AddField("customerid", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), true);

            Assert.That(row.Fields.Count, Is.EqualTo(4));
            foreach (var f in row.Fields)
            {
                Assert.That(f, Is.Not.Null);
            }
        }
    }
}
