using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DatabaseEntities.Factories;
using SQLDataProducer.Entities.ExecutionEntities;
using Generators = SQLDataProducer.Entities.Generators;
using SQLDataProducer.RandomTests;
using SQLDataProducer.Entities;
using SQLDataProducer.DataAccess;
using SQLDataProducer.TaskExecuter;
using SQLDataProducer.Entities.OptionEntities;


namespace SQLDataProducer.OtherTests
{
    public class GeneratorTests : TestBase
    {

        IEnumerable<Generators.Generator> allGens = Generators.Generator.GetDateTimeGenerators()
                                            .Concat(Generators.Generator.GetDecimalGenerators())
                                            .Concat(Generators.Generator.GetGeneratorsForBigInt())
                                            .Concat(Generators.Generator.GetGeneratorsForBit())
                                            .Concat(Generators.Generator.GetGeneratorsForInt())
                                            .Concat(Generators.Generator.GetGeneratorsForSmallInt())
                                            .Concat(Generators.Generator.GetGeneratorsForTinyInt())
                                            .Concat(Generators.Generator.GetGUIDGenerators())
                                            .Concat(Generators.Generator.GetStringGenerators(1))
                                            .Concat(new Generators.Generator[] { Generators.Generator.CreateNULLValueGenerator() });

        public GeneratorTests()
            : base()
        {

        }

        //[Test]
        //public void TestDateTimeGenerators()
        //{
        //    var op = new SQLDataProducer.Entities.OptionEntities.ExecutionTaskOptions();
        //    op.DateTimeGenerationStartTime = new DateTime(2012, 1, 1);
        //    Generators.Generator.InitGeneratorStartValues(op);


        //    IEnumerable<Generators.Generator> dateGens = Generators.GeneratorFactory.GetGeneratorsForDataType(new ColumnDataTypeDefinition("datetime", false));

        //    var hs = dateGens.Where(x => x.GeneratorName == Generators.Generator.GENERATOR_HoursSeries).First();
        //    Assert.IsNotNull(hs, "Expected generator was not found");

        //    string paramKey = "Shift Minutes";
        //    var min = Generators.Generator.GetParameterByName(hs.GeneratorParameters, paramKey);
        //    Assert.IsNotNull(min, string.Format("The {0} parameters was not found in the generator", paramKey));

        //    paramKey = "Shift Seconds";
        //    var sec = Generators.Generator.GetParameterByName(hs.GeneratorParameters, paramKey);
        //    Assert.IsNotNull(min, string.Format("The {0} parameters was not found in the generator", paramKey));

        //    paramKey = "Shift Milliseconds";
        //    var ms = Generators.Generator.GetParameterByName(hs.GeneratorParameters, paramKey);
        //    Assert.IsNotNull(min, string.Format("The {0} parameters was not found in the generator", paramKey));

        //    DateTime firstGenerated = DateTime.Parse(hs.GenerateValue(1).ToString().Replace("'", ""));
        //    DateTime secondGenerated = DateTime.Parse(hs.GenerateValue(2).ToString().Replace("'", ""));

        //    Assert.IsTrue(secondGenerated > firstGenerated, "Second was not bigger than first");
        //    Assert.IsTrue(secondGenerated.Equals(firstGenerated.AddHours(1)), string.Format("Expected second being 1 hour bigger than first. First: {0} - Second: {1}:", firstGenerated, secondGenerated));


        //}

        [Test]
        public void ShouldGenerateCurrentDateAllways()
        {
            var genName = Generators.Generator.GENERATOR_CurrentDate;
            ColumnEntity col = CreateColumnOfDatatype("datetime", genName);
           
            List<DateTime> dates = new List<DateTime>();

            for (int i = 0; i < 10000; i++)
            {
                col.GenerateValue(i);
                dates.Add((DateTime)col.PreviouslyGeneratedValue);
                
            }
            for (int i = 0; i < 10000; i++)
            {
                if (i > 0 && i < 1000)
                {
                    // Accepting that the get dates can be different, but not much
                    Assert.Less(dates[i] - dates[i - 1], new TimeSpan(0, 0, 0, 0, 2));
                }
            }

        }

        [Test]
        public void ShouldGenerateStaticInt()
        {
            var genName = Generators.Generator.GENERATOR_StaticNumber;
            ColumnEntity col = CreateColumnOfDatatype("int", genName);

            List<int> dates = new List<int>();

            for (int i = 0; i < 10000; i++)
            {
                col.GenerateValue(i);
                dates.Add((int)col.PreviouslyGeneratedValue);

            }
            for (int i = 0; i < 10000; i++)
            {
                if (i > 0 && i < 1000)
                {
                    Assert.AreEqual(dates[i], dates[i - 1]);
                }
            }

        }

        

        private static ColumnEntity CreateColumnOfDatatype(string dataType, string genName)
        {
            ColumnEntity col = DatabaseEntityFactory.Instance.CreateColumnEntity("someCol", new ColumnDataTypeDefinition(dataType, false), false, 1, false, null);
            
            Assert.IsNotNull(col.PossibleGenerators);
            Assert.IsNotNull(col.Generator);
            Assert.Greater(col.PossibleGenerators.Count, 0);

            col.Generator = col.PossibleGenerators.Where(x => x.GeneratorName == genName).FirstOrDefault();
            Assert.IsNotNull(col.Generator);
            Assert.AreEqual(genName, col.Generator.GeneratorName);
            
            return col;
        }

        [Test]
        public void ShouldGenerateNewValuesForEachRow()
        {
            var wfm = new WorkflowManager();
            var tda = new TableEntityDataAccess(Connection());
            var adressTable = tda.GetTableAndColumns("Person", "NewPerson");
            var i1 = new ExecutionItem(adressTable);
            i1.ExecutionCondition = ExecutionConditions.None;
            i1.RepeatCount = 10;
            

            {
                var options = new ExecutionTaskOptions();
                options.ExecutionType = ExecutionTypes.ExecutionCountBased;
                options.FixedExecutions = 1;
                options.NumberGeneratorMethod = NumberGeneratorMethods.NewNForEachRow;
                ExecutionItemCollection items = new ExecutionItemCollection();
                items.Add(i1);
                // new N for each row
                var res = wfm.RunWorkFlow(options, Connection(), items);

                Console.WriteLine(res.ToString());
                Assert.AreEqual(10, res.InsertCount, "InsertCount should be 10");
                Assert.AreEqual(0, res.ErrorList.Count, "InsertCount should be 0");
                Assert.AreEqual(1, res.ExecutedItemCount, "ExecutedItemCount should be 1");
                Assert.Greater(res.Duration, TimeSpan.Zero, "Duration should > 0");
            }
        }
    }
}

