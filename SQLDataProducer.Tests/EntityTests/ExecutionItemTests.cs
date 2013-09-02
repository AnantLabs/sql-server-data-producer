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
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Data;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.Entities.DatabaseEntities.Factories;
using SQLDataProducer.Entities.DatabaseEntities;



namespace SQLDataProducer.Tests.EntitiesTests
{
    [TestFixture]
    [MSTest.TestClass]
    public class ExecutionItemTests : TestBase
    {
        [Test]
        [MSTest.TestMethod]
        public void ShouldBeAbletoCloneExecutionItem()
        {
            var t = new TableEntity("dbo", "peter");
            var c1 = DatabaseEntityFactory.CreateColumnEntity("id", new ColumnDataTypeDefinition("int", false), true, 1, false, string.Empty, null);
            var c2 = DatabaseEntityFactory.CreateColumnEntity("name", new ColumnDataTypeDefinition("varchar(500)", false), false, 2, false, string.Empty, null);
            var c3 = DatabaseEntityFactory.CreateColumnEntity("created", new ColumnDataTypeDefinition("datetime", false), false, 3, false, string.Empty, null);
            var c4 = DatabaseEntityFactory.CreateColumnEntity("enabled", new ColumnDataTypeDefinition("bit", false), false, 4, false, string.Empty, null);

            Assert.AreEqual("id", c1.ColumnName);
            Assert.AreEqual("name", c2.ColumnName);
            Assert.AreEqual("created", c3.ColumnName);
            Assert.AreEqual("enabled", c4.ColumnName);

            t.Columns.Add(c1);
            t.Columns.Add(c2);
            t.Columns.Add(c3);
            t.Columns.Add(c4);

            Assert.AreEqual(4, t.Columns.Count);

            var ei = new ExecutionItem(t, "orginal");
            var clonedEI = ei.Clone();

            Assert.That(ei, Is.EqualTo(clonedEI));

            Assert.IsTrue(ei.Equals(clonedEI));
            Assert.IsTrue(ei.TargetTable.Equals(clonedEI.TargetTable));

            Assert.AreEqual(ei.Description, clonedEI.Description);
            Assert.AreEqual(ei.RepeatCount, clonedEI.RepeatCount);
            Assert.AreEqual(ei.Order, clonedEI.Order);
            Assert.AreEqual(ei.ExecutionCondition, clonedEI.ExecutionCondition);
            Assert.AreEqual(ei.ExecutionConditionValue, clonedEI.ExecutionConditionValue);
            Assert.AreEqual(ei.HasWarning, clonedEI.HasWarning);
            Assert.AreEqual(ei.TargetTable, clonedEI.TargetTable);
            Assert.AreEqual(ei.TruncateBeforeExecution, clonedEI.TruncateBeforeExecution);
            Assert.AreEqual(ei.WarningText, clonedEI.WarningText);

            var ct = clonedEI.TargetTable;


            Assert.AreEqual("peter", ct.TableName);
            Assert.AreEqual("dbo", ct.TableSchema);
            Assert.AreEqual(4, ct.Columns.Count);
            var cc1 = ct.Columns[0];
            var cc2 = ct.Columns[1];
            var cc3 = ct.Columns[2];
            var cc4 = ct.Columns[3];

            AssertColumn(cc1, c1);
            AssertColumn(cc2, c2);
            AssertColumn(cc3, c3);
            AssertColumn(cc4, c4);

        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldBeAbleToCompareExecutionItems()
        {
            //Assert.AreEqual("implemented", "not implemented");

            {
                // Equal
                ExecutionItem x = new ExecutionItem(new TableEntity("dbo", "peter"));
                ExecutionItem y = new ExecutionItem(new TableEntity("dbo", "peter"));
                ExecutionItem z = new ExecutionItem(new TableEntity("dbo", "peter"));
                AssertEqualsDefaultBehaviour(x, y, z);

                Assert.IsTrue(x.Equals(y));
                Assert.IsTrue(x.Equals(z));
                Assert.IsTrue(y.Equals(x));
                Assert.IsTrue(y.Equals(z));
                Assert.IsTrue(z.Equals(x));
                Assert.IsTrue(z.Equals(y));
            }

            {
                // Not equal
                ExecutionItem x = new ExecutionItem(new TableEntity("dbo", "peter"));
                ExecutionItem y = new ExecutionItem(new TableEntity("dbo", "henell"));
                ExecutionItem z = new ExecutionItem(new TableEntity("dbo", "data producer"));
                AssertEqualsDefaultBehaviour(x, y, z);

                Assert.IsFalse(x.Equals(y));
                Assert.IsFalse(x.Equals(z));
                Assert.IsFalse(y.Equals(x));
                Assert.IsFalse(y.Equals(z));
                Assert.IsFalse(z.Equals(x));
                Assert.IsFalse(z.Equals(y));
            }
            {
                // Not equal even if same table
                ExecutionItem x = new ExecutionItem(new TableEntity("dbo", "peter"));
                ExecutionItem y = new ExecutionItem(new TableEntity("dbo", "peter"));
                ExecutionItem z = new ExecutionItem(new TableEntity("dbo", "peter"));
                x.Order = 1;
                y.Order = 2;
                z.Order = 3;

                AssertEqualsDefaultBehaviour(x, y, z);

                Assert.IsFalse(x.Equals(y));
                Assert.IsFalse(x.Equals(z));
                Assert.IsFalse(y.Equals(x));
                Assert.IsFalse(y.Equals(z));
                Assert.IsFalse(z.Equals(x));
                Assert.IsFalse(z.Equals(y));
            }
            {
                // Not equal even if same table
                ExecutionItem x = new ExecutionItem(new TableEntity("dbo", "peter"));
                ExecutionItem y = new ExecutionItem(new TableEntity("dbo", "peter"));
                ExecutionItem z = new ExecutionItem(new TableEntity("dbo", "peter"));
                // When items are added to the list they should get the Order property set, and then be unique
                ExecutionItemCollection col = new ExecutionItemCollection();
                col.Add(x);
                col.Add(y);
                col.Add(z);
                var tested = 0;
                for (int i = 0; i < col.Count - 1; i++)
                {
                    for (int j = i + 1; j < col.Count; j++)
                    {
                        Assert.IsFalse(col[i].Equals(col[j]));
                        tested++;
                    }
                }
                Assert.Greater(tested, 0);

            }
        }

        [Test]
        [MSTest.TestMethod]
        public void AnExecutionItemWithATableWithAColumnReferencingAnotherTableShouldHaveAWarningIfItDoesNotHaveAnyForeignKeys()
        {

            {
                var fk = new ForeignKeyEntity();
                fk.ReferencingTable = new TableEntity("dbo", "Company");
                fk.ReferencingColumn = "CompanyID";

                var warnedColumn = DatabaseEntityFactory.CreateColumnEntity(
                                  "CompanyID",
                                  new ColumnDataTypeDefinition("int", false),
                                  true,
                                  1,
                                  true, // yes, it references another table
                                  string.Empty,
                                  fk);


                Assert.IsTrue(warnedColumn.HasWarning, "Column has no warning");
                var table = new TableEntity("dbo", "Employee");
                table.Columns.Add(warnedColumn);
                Assert.IsTrue(table.HasWarning, "Table has no warning");

                ExecutionItem ie = new ExecutionItem(table);
                Assert.IsTrue(ie.HasWarning, "Execution Item has no warning");
            }
            {
                // Item with with no foreign key should not have warning
                var fk = new ForeignKeyEntity();
                var notWarnedColumn = DatabaseEntityFactory.CreateColumnEntity(
                                  "CompanyID",
                                  new ColumnDataTypeDefinition("int", false),
                                  true,
                                  1,
                                  false, // not foreign key
                                  string.Empty,
                                  fk);


                Assert.IsTrue(notWarnedColumn.HasWarning == false, "Column has warning but should not");
                var table = new TableEntity("dbo", "Employee");
                table.Columns.Add(notWarnedColumn);
                Assert.IsTrue(table.HasWarning == false, "Table has warning but should not");

                ExecutionItem ie = new ExecutionItem(table);
                Assert.IsTrue(ie.HasWarning == false, "Execution Item has warning but should not");
            }


        }
    }
}
