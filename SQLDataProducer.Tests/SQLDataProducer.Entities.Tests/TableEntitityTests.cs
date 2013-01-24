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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Data;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DatabaseEntities.Factories;


namespace SQLDataProducer.RandomTests
{
    [TestFixture]
    public class TableEntityTests : TestBase
    {
        TableEntity t1 = new TableEntity("dbo", "Person");
        TableEntity t2 = new TableEntity("dbo", "Person");
        TableEntity t3 = new TableEntity("dbo", "Person");

        TableEntity t4 = new TableEntity("dbo", "Address");

        [Test]
        public void ShouldBeEqualWhenEqual()
        {
            Assert.That(t1, Is.EqualTo(t2));
            Assert.That(t1, Is.EqualTo(t3));
        }

        [Test]
        public void ShouldNotBeEqualWhenNotEqual()
        {
            Assert.That(t1, Is.Not.EqualTo(t4));
        }

        [Test]
        public void ShouldBeAbleToCompareTables()
        {

            {
                TableEntity x = new TableEntity("dbo", "Person");
                TableEntity z = new TableEntity("dbo", "Person");
                TableEntity y = new TableEntity("dbo", "Person");

                // All are the same
                AssertEqualsDefaultBehaviour(x, z, y);

                Assert.IsTrue(x.Equals(y) && x.Equals(z));
                Assert.IsTrue(z.Equals(x) && z.Equals(y));
                Assert.IsTrue(y.Equals(x) && y.Equals(z));

                Assert.IsTrue(object.Equals(x, y));
                Assert.IsTrue(object.Equals(x, z));

                Assert.IsTrue(object.Equals(y, x));
                Assert.IsTrue(object.Equals(y, z));

                Assert.IsTrue(object.Equals(z, y));
                Assert.IsTrue(object.Equals(z, x));

            }
            {
                TableEntity x = new TableEntity("Person", "Person");
                TableEntity z = new TableEntity("Person", "Contact");
                TableEntity y = new TableEntity("Person", "Customer");

                // All are different
                AssertEqualsDefaultBehaviour(x, z, y);

                Assert.IsFalse(x.Equals(y) && x.Equals(z));
                Assert.IsFalse(z.Equals(x) && z.Equals(y));
                Assert.IsFalse(y.Equals(x) && y.Equals(z));
            }
            {
                TableEntity x = new TableEntity("Person", "Person");
                TableEntity z = new TableEntity("Person", "Person");
                TableEntity y = new TableEntity("Person", "Person");

                x.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("id", new ColumnDataTypeDefinition("int", false), true, 1, false, "", null));

                z.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("id", new ColumnDataTypeDefinition("int", false), true, 1, true, "", new ForeignKeyEntity()));

                y.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("id", new ColumnDataTypeDefinition("int", false), false, 1, false, "", null));

                // All are different
                AssertEqualsDefaultBehaviour(x, z, y);

                Assert.IsFalse(x.Equals(y) && x.Equals(z));
                Assert.IsFalse(z.Equals(x) && z.Equals(y));
                Assert.IsFalse(y.Equals(x) && y.Equals(z));
            }


        }

        [Test]
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

            for (int i = 0; i < 500; i++)
            {
                foreach (var c in table.Columns)
                {
                    c.GenerateValue(i);
                    Assert.IsNotNull(c.PreviouslyGeneratedValue, string.Format("PreviouslyGeneratedValue of {0} does not exist", c.ColumnName));
                }
            }

        }
    }
}
