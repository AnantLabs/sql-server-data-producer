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
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DatabaseEntities.Factories;


namespace SQLDataProducer.Tests.Entities
{
    [TestFixture]
    [MSTest.TestClass]
    public class TableEntityTests : TestBase
    {
        TableEntity t1 = new TableEntity("dbo", "Person");
        TableEntity t2 = new TableEntity("dbo", "Person");
        TableEntity t3 = new TableEntity("dbo", "Person");

        TableEntity t4 = new TableEntity("dbo", "Address");

        [Test]
        [MSTest.TestMethod]
        public void ShouldOnlyBeEqualByReferential()
        {
            Assert.That(t1, Is.EqualTo(t1));
            var another = t1;
            Assert.That(t1, Is.EqualTo(another));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldNotBeEqualWhenNotEqual()
        {
            Assert.That(t1, Is.Not.EqualTo(t4));
            Assert.That(t1, Is.Not.EqualTo(t2));
            Assert.That(t1, Is.Not.EqualTo(t3));
        }

        [Test]
        [MSTest.TestMethod]
        public void GivenSomeTableConfigurationItShouldGenerateValuesForAllColumns()
        {
            TableEntity table = new TableEntity("dbo", "Customer");

            table.Columns.Add(DatabaseEntityFactory.CreateColumnEntity(
                                "CustomerName",
                                new ColumnDataTypeDefinition("varchar(500)", true),
                                false,
                                2,
                                false,
                                string.Empty,
                                null));

            ColumnEntity firstCol = table.Columns[0];
            Assert.IsTrue(firstCol.ColumnName == "CustomerName");
            Assert.IsTrue(firstCol.IsForeignKey == false);
            Assert.IsTrue(firstCol.IsIdentity == false);
            Assert.IsTrue(firstCol.OrdinalPosition == 2);
            Assert.IsTrue(firstCol.Generator != null);
            Assert.IsTrue(firstCol.PossibleGenerators.Count > 0);
            Assert.IsTrue(firstCol.HasWarning == false);

            table.Columns.Add(DatabaseEntityFactory.CreateColumnEntity(
                               "CustomerID",
                               new ColumnDataTypeDefinition("int", false),
                               true,
                               1,
                               false,
                               string.Empty,
                               null));

            ColumnEntity secondCol = table.Columns[1];
            Assert.IsTrue(secondCol.ColumnName == "CustomerID");
            Assert.IsTrue(secondCol.IsForeignKey == false);
            Assert.IsTrue(secondCol.IsIdentity == true);
            Assert.IsTrue(secondCol.OrdinalPosition == 1);
            Assert.IsTrue(secondCol.Generator != null);
            Assert.IsTrue(secondCol.PossibleGenerators.Count > 0);
            Assert.IsTrue(secondCol.HasWarning == false);

        }
    }
}
