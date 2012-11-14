using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DatabaseEntities.Factories;
using SQLDataProducer.Entities.ExecutionEntities;
using Generators = SQLDataProducer.Entities.Generators;


namespace SQLDataProducer.Tests
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

        //[Test]
        //public void EveryGeneratorShouldBeRunnableWithDefaultConfiguration()
        //{
        //    Assert.IsTrue(gens.Count() > 0, "No generators found");
        //    for (long i = 0; i < 500; i++)
        //    {
        //        foreach (var g in gens)
        //        {
        //            object o = g.GenerateValue(i);
        //            Assert.IsNotNull(o, string.Format("{0} generator could not generate value with default parameters", g.GeneratorName));
        //        }
        //    }
        //}

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
    }
}
