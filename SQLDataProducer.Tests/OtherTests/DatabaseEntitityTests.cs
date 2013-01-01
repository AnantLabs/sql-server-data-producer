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
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DatabaseEntities.Factories;
using SQLDataProducer.Entities.ExecutionEntities;
using Generators = SQLDataProducer.Entities.Generators;
using System.Data;


namespace SQLDataProducer.RandomTests
{
    [TestFixture]
    public class DatabaseEntitityTests
    {
        IEnumerable<Generators.Generator> gens = Generators.Generator.GetDateTimeGenerators()
                                            .Concat(Generators.Generator.GetDecimalGenerators(5000))
                                            .Concat(Generators.Generator.GetGeneratorsForBigInt())
                                            .Concat(Generators.Generator.GetGeneratorsForBit())
                                            .Concat(Generators.Generator.GetGeneratorsForInt())
                                            .Concat(Generators.Generator.GetGeneratorsForSmallInt())
                                            .Concat(Generators.Generator.GetGeneratorsForTinyInt())
                                            .Concat(Generators.Generator.GetGUIDGenerators())
                                            .Concat(Generators.Generator.GetStringGenerators(1))
                                            .Concat(Generators.Generator.GetBinaryGenerators(1))
                                            .Concat(Generators.Generator.GetXMLGenerators())
                                            .Concat(Generators.Generator.GetGeneratorsForIdentity())
                                            .Concat(new Generators.Generator[] { Generators.Generator.CreateNULLValueGenerator() });




        [Test]
        public void GeneratorsShouldHaveHelpTexts()
        {
            Assert.IsTrue(gens.Count() > 0, "No generators found");
            foreach (var g in gens)
            {
                Assert.IsNotNullOrEmpty(g.GeneratorHelpText, string.Format("{0} does not have helptext", g.GeneratorName));
            }
        }

        [Test]
        public void EveryGeneratorShouldBeRunnableWithDefaultConfiguration()
        {
            Assert.IsTrue(gens.Count() > 0, "No generators found");
            
            // Ignore some generators that we know cannot work
            List<Generators.Generator> runThese = new List<Generators.Generator>(gens.Where(x => x.GeneratorName != Generators.Generator.GENERATOR_IdentityFromPreviousItem));

            foreach (var g in runThese)
            {
                for (long i = 0; i < 50000; i++)
                {
                    object o = g.GenerateValue(i);
                    Assert.IsNotNull(o, string.Format("{0} generator could not generate value with default parameters", g.GeneratorName));
                    //Console.WriteLine(o);
                }
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

        [Test]
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

        [Test]
        public void ShouldBeAbleToCreateColumnEntityFromFactory()
        {
            var c1 = DatabaseEntityFactory.CreateColumnEntity("id", new ColumnDataTypeDefinition("int", false), true, 1, false, string.Empty, null);
            Assert.AreEqual("id", c1.ColumnName);
            Assert.AreEqual("int", c1.ColumnDataType.Raw);
            Assert.AreEqual(SqlDbType.Int, c1.ColumnDataType.DBType);
            Assert.AreEqual(true, c1.IsIdentity);
            Assert.AreEqual(false, c1.ColumnDataType.IsNullable);
            Assert.AreEqual(1, c1.OrdinalPosition);


            var c2 = DatabaseEntityFactory.CreateColumnEntity("name", new ColumnDataTypeDefinition("nvarchar(988)", true), false, 2, false, string.Empty, null);
            Assert.AreEqual("name", c2.ColumnName);
            Assert.AreEqual("nvarchar(988)", c2.ColumnDataType.Raw);
            Assert.AreEqual(SqlDbType.NVarChar, c2.ColumnDataType.DBType);
            Assert.AreEqual(false, c2.IsIdentity);
            Assert.AreEqual(true, c2.ColumnDataType.IsNullable);
            Assert.AreEqual(2, c2.OrdinalPosition);
        }

        [Test]
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

        /// <summary>
        /// Check that the provided objects comply with the default Equals behaviour specified by Microsoft.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="y"></param>
        /// <remarks>http://msdn.microsoft.com/en-us/library/ms173147(v=vs.80).aspx</remarks>
        private static void AssertEqualsDefaultBehaviour<T>(T x, T z, T y)  where T : IEquatable<T>
        {
            //http://msdn.microsoft.com/en-us/library/ms173147(v=vs.80).aspx

            //x.Equals(x) returns true.
            Assert.IsTrue(x.Equals(x));

            //x.Equals(y) returns the same value as y.Equals(x).
            var a = x.Equals(y);
            var b = y.Equals(x);
            Assert.AreEqual(a, b);

            // if (x.Equals(y) && y.Equals(z)) returns true, then x.Equals(z) returns true.
            if (x.Equals(y) && y.Equals(z))
                Assert.IsTrue(x.Equals(z));

            // Successive invocations of x.Equals(y) return the same value as long as the objects referenced by x and y are not modified.
            var before = x.Equals(y);
            for (int i = 0; i < 199; i++)
                Assert.AreEqual(before, x.Equals(y));

            // x.Equals(null) returns false.
            Assert.IsFalse(x.Equals(null));
            Assert.IsFalse(y.Equals(null));
            Assert.IsFalse(z.Equals(null));
        }



        [Test]
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
                for (int i = 0; i < col.Count-1; i++)
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

        private void AssertColumn(ColumnEntity expectedColumn, ColumnEntity newColumn)
        {
            Assert.AreEqual(expectedColumn.ColumnDataType.DBType, newColumn.ColumnDataType.DBType);
            Assert.AreEqual(expectedColumn.ColumnDataType.Raw, newColumn.ColumnDataType.Raw);
            Assert.AreEqual(expectedColumn.ColumnDataType.IsNullable, newColumn.ColumnDataType.IsNullable);
            Assert.AreEqual(expectedColumn.ColumnDataType.MaxLength, newColumn.ColumnDataType.MaxLength);

            Assert.AreEqual(expectedColumn.ColumnName, newColumn.ColumnName);
            Assert.AreEqual(expectedColumn.Generator.GeneratorName, newColumn.Generator.GeneratorName);
            Assert.AreEqual(expectedColumn.Generator.GeneratorParameters.Count, newColumn.Generator.GeneratorParameters.Count);
            Assert.AreEqual(expectedColumn.HasWarning, newColumn.HasWarning);
            Assert.AreEqual(expectedColumn.IsIdentity, newColumn.IsIdentity);
            Assert.AreEqual(expectedColumn.OrdinalPosition, newColumn.OrdinalPosition);
            Assert.AreEqual(expectedColumn.PossibleGenerators.Count, newColumn.PossibleGenerators.Count);
            Assert.AreEqual(expectedColumn.WarningText, newColumn.WarningText);

            Assert.IsTrue(expectedColumn.Equals(newColumn));
            Assert.IsTrue(expectedColumn.Equals(expectedColumn));
            Assert.IsTrue(newColumn.Equals(expectedColumn));
            Assert.IsTrue(newColumn.Equals(newColumn));

        }

        [Test]
        public void ShouldCompareParameterCollections()
        {
            
            //Assert.AreEqual(originCol.PossibleGenerators, loadedCol.PossibleGenerators);
        }
    }
}
