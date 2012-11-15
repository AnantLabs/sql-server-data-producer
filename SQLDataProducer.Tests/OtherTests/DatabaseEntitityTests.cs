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
            var c1 = DatabaseEntityFactory.CreateColumnEntity("id", new ColumnDataTypeDefinition("int", false), true, 1, false, null);
            Assert.AreEqual("id", c1.ColumnName);
            Assert.AreEqual("int", c1.ColumnDataType.Raw);
            Assert.AreEqual(SqlDbType.Int, c1.ColumnDataType.DBType);
            Assert.AreEqual(true, c1.IsIdentity);
            Assert.AreEqual(false, c1.ColumnDataType.IsNullable);
            Assert.AreEqual(1, c1.OrdinalPosition);


            var c2 = DatabaseEntityFactory.CreateColumnEntity("name", new ColumnDataTypeDefinition("nvarchar(988)", true), false, 2, false, null);
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
            var c1 = DatabaseEntityFactory.CreateColumnEntity("id", new ColumnDataTypeDefinition("int", false), true, 1, false, null);
            var c2 = DatabaseEntityFactory.CreateColumnEntity("name", new ColumnDataTypeDefinition("varchar(500)", false), false, 2, false, null);
            var c3 = DatabaseEntityFactory.CreateColumnEntity("created", new ColumnDataTypeDefinition("datetime", false), false, 3, false, null);
            var c4 = DatabaseEntityFactory.CreateColumnEntity("enabled", new ColumnDataTypeDefinition("bit", false), false, 4, false, null);

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

            {
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
        }
    }
}
