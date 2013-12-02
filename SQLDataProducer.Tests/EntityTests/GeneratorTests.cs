//// Copyright 2012-2013 Peter Henell

////   Licensed under the Apache License, Version 2.0 (the "License");
////   you may not use this file except in compliance with the License.
////   You may obtain a copy of the License at

////       http://www.apache.org/licenses/LICENSE-2.0

////   Unless required by applicable law or agreed to in writing, software
////   distributed under the License is distributed on an "AS IS" BASIS,
////   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
////   See the License for the specific language governing permissions and
////   limitations under the License.

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NUnit.Framework;
//using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
//using SQLDataProducer.Entities.DatabaseEntities;
//using SQLDataProducer.Entities.DatabaseEntities.Factories;
//using SQLDataProducer.Entities.ExecutionEntities;


//using SQLDataProducer.Entities;
//using SQLDataProducer.DataAccess;
//using SQLDataProducer.TaskExecuter;
//using SQLDataProducer.Entities.OptionEntities;
//using System.Diagnostics;
//using System.Collections.ObjectModel;
//using SQLDataProducer.Entities.DatabaseEntities.Collections;
//using SQLDataProducer.Entities.Generators;

//namespace SQLDataProducer.Tests.EntitiesTests
//{
//    [MSTest.TestClass]
//    public class GeneratorTests : TestBase
//    {
//        public GeneratorTests()
//            : base()
//        {

//        }

//        Generator intGenerator1 = Generator.CreateIntUpCounter(1, 50000);
//        Generator intGenerator2 = Generator.CreateIntUpCounter(1, 50000);
//        Generator intGenerator3 = Generator.CreateIntUpCounter(1, 50000);

//        Generator intGenerator4 = Generator.CreateRandomIntGenerator(1, 50000);

//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldBeEqualWhenEqual()
//        {
//            Assert.IsTrue(intGenerator1.Equals(intGenerator2));
//            Assert.IsTrue(intGenerator2.Equals(intGenerator1));
//            Assert.AreEqual(intGenerator1, intGenerator2);
//            Assert.AreEqual(intGenerator2, intGenerator1);

//            AssertEqualsDefaultBehaviour(intGenerator1, intGenerator2, intGenerator3);
//            AssertEqualsDefaultBehaviour(intGenerator2, intGenerator1, intGenerator3);
//            AssertEqualsDefaultBehaviour(intGenerator2, intGenerator3, intGenerator1);

//        }

//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldNotBeEqualWhenNotEqual()
//        {
//            Generator modifiedGenerator = Generator.CreateRandomIntGenerator(1, 50000);

//            Assert.That(intGenerator4, Is.EqualTo(modifiedGenerator));
//            Assert.That(intGenerator1, Is.Not.EqualTo(intGenerator4));
//        }

//        [Test]
//        [MSTest.TestMethod]
//        public void GeneratorsShouldHaveHelpTexts()
//        {
//            Assert.IsTrue(gens.Count() > 0, "No generators found");
//            foreach (var g in gens)
//            {
//                Assert.IsNotNullOrEmpty(g.GeneratorHelpText, string.Format("{0} does not have helptext", g.GeneratorName));
//            }
//        }

//        [Test]
//        [MSTest.TestMethod]
//        public void EveryGeneratorShouldBeRunnableWithDefaultConfiguration()
//        {
//            Assert.IsTrue(gens.Count() > 0, "No generators found");

//            // Ignore some generators that we know cannot work
//            List<Generator> runThese = new List<Generator>(gens.Where(x => x.GeneratorName != Generator.GENERATOR_IdentityFromPreviousItem));

//            foreach (var g in runThese)
//            {
//                for (long i = 0; i < 50000; i++)
//                {
//                    object o = g.GenerateValue(i);
//                    Assert.IsNotNull(o, string.Format("{0} generator could not generate value with default parameters", g.GeneratorName));
//                    //Console.WriteLine(o);
//                }
//            }
//        }

//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldBeAbleToInsertUsingSQLGetDate()
//        {
//            var tda = new TableEntityDataAccess(Connection());
//            var table = tda.GetTableAndColumns("Person", "NewPerson");
//            var options = new ExecutionTaskOptions();
//            var execItems = new ExecutionItemCollection();


//            var ei = new ExecutionNode(table);
//            ei.RepeatCount = 10;
//            execItems.Add(ei);

//            var col = table.Columns.Where(x => x.ColumnDataType.Raw == "datetime").FirstOrDefault();
//            Assert.IsNotNull(col);

//            col.Generator = col.PossibleGenerators.Where(g => g.GeneratorName == Generator.GENERATOR_SQLGetDate).FirstOrDefault();
//            Assert.IsNotNull(col.Generator);
//            Assert.AreEqual(Generator.GENERATOR_SQLGetDate, col.Generator.GeneratorName);

//            for (int i = 0; i < 1000; i++)
//            {
//                col.GenerateValue(i);
//                //Assert.IsNotNull(col.PreviouslyGeneratedValue);
//                throw new NotImplementedException("Valuestore");
//            }

//            var wfm = new WorkflowManager();
//            var res = wfm.RunWorkFlow(new TaskExecuter.TaskExecuter(options, Connection(), execItems, DefaultDataConsumer));
//            foreach (var er in res.ErrorList)
//            {
//                Console.WriteLine(er);
//            }
//            Assert.AreEqual(0, res.ErrorList.Count);

//        }
//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldBeAbleToInsertUsingSQLQuery()
//        {
//            var tda = new TableEntityDataAccess(Connection());
//            var table = tda.GetTableAndColumns("Person", "NewPerson");
//            var options = new ExecutionTaskOptions();
//            var execItems = new ExecutionItemCollection();


//            var ei = new ExecutionNode(table);
//            ei.RepeatCount = 10;
//            execItems.Add(ei);

//            var col = table.Columns.Where(x => x.ColumnDataType.Raw == "datetime").FirstOrDefault();
//            Assert.IsNotNull(col);

//            col.Generator = col.PossibleGenerators.Where(g => g.GeneratorName == Generator.GENERATOR_CustomSQLQuery).FirstOrDefault();
//            Assert.IsNotNull(col.Generator);
//            Assert.AreEqual(Generator.GENERATOR_CustomSQLQuery, col.Generator.GeneratorName);

//            var queryParam = col.Generator.GeneratorParameters["Custom SQL Query"];
//            queryParam.Value = "Select getdate() -5";

//            for (int i = 0; i < 1000; i++)
//            {
//                col.GenerateValue(i);
//                //Assert.IsNotNull(col.PreviouslyGeneratedValue);
//                throw new NotImplementedException("Valuestore");
//            }

//            var wfm = new WorkflowManager();
//            var res = wfm.RunWorkFlow(new TaskExecuter.TaskExecuter(options, Connection(), execItems, DefaultDataConsumer));
//            foreach (var er in res.ErrorList)
//            {
//                Console.WriteLine(er);
//            }
//            Assert.AreEqual(0, res.ErrorList.Count);

//        }

//        [Test]
//        [MSTest.TestMethod]
//        public void IdentityColumnsShouldHaveTheIdentityGeneratorSelected()
//        {
//            var tda = new TableEntityDataAccess(Connection());
//            var table = tda.GetTableAndColumns("Person", "NewPerson");
//            var options = new ExecutionTaskOptions();
//            var execItems = new ExecutionItemCollection();


//            var ei = new ExecutionNode(table);
//            ei.RepeatCount = 10;
//            execItems.Add(ei);

//            var col = table.Columns.Where(x => x.IsIdentity).FirstOrDefault();
//            Assert.IsNotNull(col);
//            Assert.IsNotNull(col.Generator);
//            Assert.AreEqual(Generator.GENERATOR_IdentityFromSqlServerGenerator, col.Generator.GeneratorName);


//            var wfm = new WorkflowManager();
//            var res = wfm.RunWorkFlow(new TaskExecuter.TaskExecuter(options, Connection(), execItems, DefaultDataConsumer));
//            foreach (var er in res.ErrorList)
//            {
//                Console.WriteLine(er);
//            }
//            Assert.AreEqual(0, res.ErrorList.Count);

//        }




//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldGenerate_DateTime_CurrentDateAllways()
//        {
//            var genName = Generator.GENERATOR_CurrentDate;
//            ColumnEntity col = CreateColumnOfDatatype("datetime", genName);

//            List<DateTime> dates = Create10000FromColumn<DateTime>(col);
//            for (int i = 0; i < 10000; i++)
//            {
//                if (i > 0 && i < 10000)
//                {
//                    // Accepting that the get dates can be different, but not much
//                    Assert.Less(dates[i] - dates[i - 1], new TimeSpan(0, 0, 0, 0, 5));
//                }
//            }
//        }

//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldGenerate_DateTime_StaticDate()
//        {
//            var genName = Generator.GENERATOR_StaticDate;
//            ColumnEntity col = CreateColumnOfDatatype("datetime", genName);

//            List<DateTime> dates = Create10000FromColumn<DateTime>(col);

//            var aDate = (DateTime)dates.FirstOrDefault();
//            Assert.IsNotNull(aDate);
//            Assert.IsTrue(dates.All(x => x == aDate));
//        }
//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldGenerate_DateTime_MinuteSeries()
//        {
//            var genName = Generator.GENERATOR_MinutesSeries;
//            ColumnEntity col = CreateColumnOfDatatype("datetime", genName);

//            List<DateTime> dates = Create10000FromColumn<DateTime>(col);
//            for (int i = 0; i < 10000; i++)
//            {
//                if (i > 0 && i < 10000)
//                {
//                    // Accepting that the get dates can be different, but not much
//                    Assert.LessOrEqual(dates[i] - dates[i - 1], new TimeSpan(0, 0, 1, 0, 2));
//                    Assert.GreaterOrEqual(dates[i] - dates[i - 1], new TimeSpan(0, 0, 1, 0, 0));
//                }
//            }
//        }
//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldGenerate_DateTime_HourSeries()
//        {
//            var genName = Generator.GENERATOR_HoursSeries;
//            ColumnEntity col = CreateColumnOfDatatype("datetime", genName);

//            List<DateTime> dates = Create10000FromColumn<DateTime>(col);
//            for (int i = 0; i < 10000; i++)
//            {
//                if (i > 0 && i < 10000)
//                {
//                    // Accepting that the get dates can be different, but not much
//                    Assert.LessOrEqual(dates[i] - dates[i - 1], new TimeSpan(0, 1, 0, 0, 2));
//                    Assert.GreaterOrEqual(dates[i] - dates[i - 1], new TimeSpan(0, 1, 0, 0, 0));
//                }
//            }
//        }
//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldGenerate_DateTime_SecondSeries()
//        {
//            var genName = Generator.GENERATOR_SecondsSeries;
//            ColumnEntity col = CreateColumnOfDatatype("datetime", genName);

//            List<DateTime> dates = Create10000FromColumn<DateTime>(col);
//            for (int i = 0; i < 10000; i++)
//            {
//                if (i > 0 && i < 10000)
//                {
//                    // Accepting that the get dates can be different, but not much
//                    Assert.LessOrEqual(dates[i] - dates[i - 1], new TimeSpan(0, 0, 0, 1, 2));
//                    Assert.GreaterOrEqual(dates[i] - dates[i - 1], new TimeSpan(0, 0, 0, 1, 0));
//                }
//            }
//        }
//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldGenerate_DateTime_DaySeries()
//        {
//            var genName = Generator.GENERATOR_DaysSeries;
//            ColumnEntity col = CreateColumnOfDatatype("datetime", genName);

//            List<DateTime> dates = Create10000FromColumn<DateTime>(col);
//            for (int i = 0; i < 10000; i++)
//            {
//                if (i > 0 && i < 10000)
//                {
//                    // Accepting that the get dates can be different, but not much
//                    Assert.LessOrEqual(dates[i] - dates[i - 1], new TimeSpan(1, 0, 0, 0, 2));
//                    Assert.GreaterOrEqual(dates[i] - dates[i - 1], new TimeSpan(1, 0, 0, 0, 0));
//                }
//            }
//        }
//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldGenerate_Long_StaticNumber()
//        {
//            var genName = Generator.GENERATOR_StaticNumber;
//            ColumnEntity col = CreateColumnOfDatatype("int", genName);

//            List<long> longs = Create10000FromColumn<long>(col);

//            Assert.IsTrue(longs.All(x => 0 == x));

//        }

//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldGenerate_Long_CountingUp()
//        {
//            var genName = Generator.GENERATOR_CountingUpInteger;
//            ColumnEntity col = CreateColumnOfDatatype("int", genName);

//            List<long> longs = Create10000FromColumn<long>(col);

//            for (int i = 0; i < 10000; i++)
//            {
//                if (i > 0 && i < 10000)
//                {
//                    Assert.Greater(longs[i], longs[i - 1]);
//                    Assert.AreEqual(longs[i] - longs[i - 1], 1);
//                }
//            }
//        }

//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldGenerate_String_FemaleNames()
//        {
//            var genName = Generator.GENERATOR_FemaleNames;
//            ColumnEntity col = CreateColumnOfDatatype("nvarchar(900)", genName);

//            List<string> longs = Create10000FromColumn<string>(col);

//            for (int i = 0; i < 10000; i++)
//            {
//                Assert.Greater(longs[i].Length, 0);
//            }
//        }

//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldGenerate_Long_RandomInt()
//        {
//            var genName = Generator.GENERATOR_RandomInt;
//            ColumnEntity col = CreateColumnOfDatatype("bigint", genName);

//            List<long> longs = Create10000FromColumn<long>(col);

//            for (int i = 0; i < 10000; i++)
//            {
//                if (i > 0 && i < 10000)
//                {
//                    Assert.AreNotEqual(longs[i], longs[i - 1]);
//                }
//            }
//        }

//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldGenerate_Decimal_RandomDecimal()
//        {
//            var genName = Generator.GENERATOR_RandomDecimal;
//            ColumnEntity col = CreateColumnOfDatatype("decimal(19,6)", genName);

//            List<decimal> longs = Create10000FromColumn<decimal>(col);

//            for (int i = 0; i < 10000; i++)
//            {
//                if (i > 0 && i < 10000)
//                {
//                    Assert.AreNotEqual(longs[i], longs[i - 1]);
//                }
//            }
//        }
//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldGenerate_Decimal_UpCounter()
//        {
//            var genName = Generator.GENERATOR_CountingUpDecimal;
//            ColumnEntity col = CreateColumnOfDatatype("decimal(19,6)", genName);

//            List<decimal> longs = Create10000FromColumn<decimal>(col);

//            for (int i = 0; i < 10000; i++)
//            {
//                if (i > 0 && i < 10000)
//                {
//                    Assert.AreEqual(1.0, longs[i] - longs[i - 1]);
//                    Assert.AreNotEqual(longs[i], longs[i - 1]);
//                }
//            }
//        }

//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldGetGeneratorsForEverySQLDatatype()
//        {
//            var dataTypes = new List<string> {"bigint",
//"binary",
//"bit",
//"char",
//"date",
//"datetime",
//"datetime2",
//"datetimeoffset",
//"decimal",
//"float",
//"geography",
//"geometry",
//"hierarchyid",
//"image",
//"int",
//"money",
//"nchar",
//"ntext",
//"numeric",
//"nvarchar",
//"real",
//"smalldatetime",
//"smallint",
//"smallmoney",
//"sql_variant",
//"sysname",
//"text",
//"time",
//"timestamp",
//"tinyint",
//"uniqueidentifier",
//"varbinary",
//"varchar",
//"xml" };

//            List<ColumnEntity> cols = new List<ColumnEntity>();
//            foreach (var s in dataTypes)
//            {
//                ColumnEntity col = DatabaseEntityFactory.CreateColumnEntity(s, new ColumnDataTypeDefinition(s, false), false, 1, false, string.Empty, null);
//                cols.Add(col);
//                Assert.IsNotNull(col.Generator);

//                Assert.Greater(col.PossibleGenerators.Count, 0);

//                for (int i = 0; i < 10000; i++)
//                {
//                    col.GenerateValue(i);
//                    //Assert.IsNotNull(col.PreviouslyGeneratedValue);
//                    throw new NotImplementedException("Valuestore");
//                }
//            }
//        }

//        private static List<T> Create10000FromColumn<T>(ColumnEntity col)
//        {
//            List<T> longs = new List<T>();

//            for (int i = 0; i < 10000; i++)
//            {
//                col.GenerateValue(i);
//                //longs.Add((T)col.PreviouslyGeneratedValue);
//                throw new NotImplementedException("Valuestore");
//            }
//            return longs;
//        }


//        private static ColumnEntity CreateColumnOfDatatype(string dataType, string genName)
//        {
//            ColumnEntity col = DatabaseEntityFactory.CreateColumnEntity("someCol", new ColumnDataTypeDefinition(dataType, false), false, 1, false, string.Empty, null);

//            Assert.IsNotNull(col.PossibleGenerators);
//            Assert.IsNotNull(col.Generator);
//            Assert.Greater(col.PossibleGenerators.Count, 0);

//            col.Generator = col.PossibleGenerators.Where(x => x.GeneratorName == genName).FirstOrDefault();
//            Assert.IsNotNull(col.Generator);
//            Assert.AreEqual(genName, col.Generator.GeneratorName);

//            return col;
//        }

//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldGenerateNewValuesForEachRow()
//        {
//            var wfm = new WorkflowManager();
//            var tda = new TableEntityDataAccess(Connection());
//            var adressTable = tda.GetTableAndColumns("Person", "NewPerson");
//            var i1 = new ExecutionNode(adressTable);
//            i1.ExecutionCondition = ExecutionConditions.None;
//            i1.RepeatCount = 10;

//            {
//                var options = new ExecutionTaskOptions();
//                options.ExecutionType = ExecutionTypes.ExecutionCountBased;
//                options.FixedExecutions = 1;
//                options.NumberGeneratorMethod = NumberGeneratorMethods.NewNForEachRow;
//                ExecutionItemCollection items = new ExecutionItemCollection();
//                items.Add(i1);
//                // new N for each row
//                var res = wfm.RunWorkFlow(new TaskExecuter.TaskExecuter(options, Connection(), items, DefaultDataConsumer));

//                Console.WriteLine(res.ToString());
//                Assert.AreEqual(10, res.InsertCount, "InsertCount should be 10");
//                Assert.AreEqual(0, res.ErrorList.Count, "InsertCount should be 0");
//                Assert.AreEqual(1, res.ExecutedItemCount, "ExecutedItemCount should be 1");
//                Assert.Greater(res.Duration, TimeSpan.Zero, "Duration should > 0");
//            }
//        }

//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldUseIdentityInsertIfRegularGeneratorSelected()
//        {
//            var wfm = new WorkflowManager();
//            var tda = new TableEntityDataAccess(Connection());
//            var options = new ExecutionTaskOptions();
//            var newPerson = tda.GetTableAndColumns("Person", "NewPerson");
//            var another = tda.GetTableAndColumns("Person", "AnotherTable");
//            var i1 = new ExecutionNode(newPerson);
//            var i2 = new ExecutionNode(another);
//            ExecutionItemCollection items = new ExecutionItemCollection();
//            items.Add(i1);
//            items.Add(i2);

//            var idCol = newPerson.Columns.First();
//            Assert.IsTrue(idCol.IsIdentity);

//            idCol.Generator = idCol.PossibleGenerators.Where(gen => gen.GeneratorName == Generator.GENERATOR_RandomInt).FirstOrDefault();
//            Assert.IsNotNull(idCol.Generator);

//            wfm.RunWorkFlow(new TaskExecuter.TaskExecuter(options, Connection(), items, DefaultDataConsumer));

//        }

//        [Test]
//        [MSTest.TestMethod]
//        public void GeneratorsShouldBeAsFastAsOtherGenerators()
//        {
//            Stopwatch sw = new Stopwatch();
//            var currDateGeneratorCol = CreateColumnOfDatatype("datetime", Generator.GENERATOR_CurrentDate);
//            var msDateGeneratorCol = CreateColumnOfDatatype("datetime", Generator.GENERATOR_MilisecondsSeries);


//            // warm up
//            for (int i = 0; i < 1000; i++)
//            {
//                currDateGeneratorCol.GenerateValue(i);
//                msDateGeneratorCol.GenerateValue(i);
//            }

//            for (int j = 0; j < 20; j++)
//            {

//                sw.Start();
//                for (int i = 0; i < 100000; i++)
//                {
//                    currDateGeneratorCol.GenerateValue(i);
//                }
//                sw.Stop();
//                var currDateGeneratorElapsedTime = sw.Elapsed;



//                sw.Reset();



//                sw.Start();
//                for (int i = 0; i < 100000; i++)
//                {
//                    msDateGeneratorCol.GenerateValue(i);
//                }

//                sw.Stop();
//                var msDateGeneratorElapsedTime = sw.Elapsed;
//                sw.Reset();
//                Console.WriteLine("Current Date generation elapsed time: {0}", currDateGeneratorElapsedTime);
//                Console.WriteLine("Milliseconds generation elapsed time: {0}", msDateGeneratorElapsedTime);
//                Console.WriteLine("Diff {0}", msDateGeneratorElapsedTime - currDateGeneratorElapsedTime);
//                Console.WriteLine();
//                //Current Date generation elapsed time: 00:00:00.2576633
//                //Milliseconds generation elapsed time: 00:00:00.1483403
//                //Diff -00:00:00.1093230
//            }


//        }


//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldCloneGeneratorCollection()
//        {
//            var col1 = DatabaseEntityFactory.CreateColumnEntity("id", new ColumnDataTypeDefinition("int", false), true, 1, false, "", null);
//            var col2 = DatabaseEntityFactory.CreateColumnEntity("date", new ColumnDataTypeDefinition("datetime", false), false, 2, false, "", null);

//            ObservableCollection<Generator> intGens = GeneratorFactory.GetGeneratorsForColumn(col1);
//            ObservableCollection<Generator> dateGens = GeneratorFactory.GetGeneratorsForColumn(col2);

//            var clonedIntGens = intGens.Clone();
//            Assert.That(intGens, Is.EqualTo(intGens));
//            Assert.That(intGens, Is.EqualTo(clonedIntGens));
//        }



//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldProduceNullValue()
//        {
//            Generator nullGenerator = Generator.CreateNULLValueGenerator();
//            var value = nullGenerator.GenerateValue(1);
//            Assert.That(value, Is.EqualTo(DBNull.Value));
//        }


//    }
//}

