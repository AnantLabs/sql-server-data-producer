// Copyright 2012 Peter Henell

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
using SQLDataProducer.RandomTests;
using SQLDataProducer.Entities;
using SQLDataProducer.DataAccess;
using SQLDataProducer.TaskExecuter;
using SQLDataProducer.Entities.OptionEntities;


namespace SQLDataProducer.RandomTests
{
    public class GeneratorTests : TestBase
    {
        public GeneratorTests()
            : base()
        {

        }

        [Test]
        public void ShouldBeAbleToInsertUsingSQLGetDate()
        {
            var tda = new TableEntityDataAccess(Connection());
            var table = tda.GetTableAndColumns("Person", "NewPerson");
            var options = new ExecutionTaskOptions();
            var execItems = new ExecutionItemCollection();


            var ei = new ExecutionItem(table);
            ei.RepeatCount = 10;
            execItems.Add(ei);

            var col = table.Columns.Where(x => x.ColumnDataType.Raw == "datetime").FirstOrDefault();
            Assert.IsNotNull(col);

            col.Generator = col.PossibleGenerators.Where(g => g.GeneratorName == Generators.Generator.GENERATOR_SQLGetDate).FirstOrDefault();
            Assert.IsNotNull(col.Generator);
            Assert.AreEqual(Generators.Generator.GENERATOR_SQLGetDate, col.Generator.GeneratorName);

            for (int i = 0; i < 1000; i++)
            {
                col.GenerateValue(i);
                Assert.IsNotNull(col.PreviouslyGeneratedValue);
            }

            var wfm = new WorkflowManager();
            var res = wfm.RunWorkFlow(options, Connection(), execItems);
            foreach (var er in res.ErrorList)
            {
                Console.WriteLine(er);
            }
            Assert.AreEqual(0, res.ErrorList.Count);

        }
        [Test]
        public void ShouldBeAbleToInsertUsingSQLQuery()
        {
            var tda = new TableEntityDataAccess(Connection());
            var table = tda.GetTableAndColumns("Person", "NewPerson");
            var options = new ExecutionTaskOptions();
            var execItems = new ExecutionItemCollection();


            var ei = new ExecutionItem(table);
            ei.RepeatCount = 10;
            execItems.Add(ei);

            var col = table.Columns.Where(x => x.ColumnDataType.Raw == "datetime").FirstOrDefault();
            Assert.IsNotNull(col);

            col.Generator = col.PossibleGenerators.Where(g => g.GeneratorName == Generators.Generator.GENERATOR_CustomSQLQuery).FirstOrDefault();
            Assert.IsNotNull(col.Generator);
            Assert.AreEqual(Generators.Generator.GENERATOR_CustomSQLQuery, col.Generator.GeneratorName);

            var queryParam = col.Generator.GeneratorParameters[0];
            queryParam.Value = "Select getdate() -5";

            for (int i = 0; i < 1000; i++)
            {
                col.GenerateValue(i);
                Assert.IsNotNull(col.PreviouslyGeneratedValue);
            }

            var wfm = new WorkflowManager();
            var res = wfm.RunWorkFlow(options, Connection(), execItems);
            foreach (var er in res.ErrorList)
            {
                Console.WriteLine(er);
            }
            Assert.AreEqual(0, res.ErrorList.Count);

        }

        [Test]
        public void IdentityColumnsShouldHaveTheIdentityGeneratorSelected()
        {
            var tda = new TableEntityDataAccess(Connection());
            var table = tda.GetTableAndColumns("Person", "NewPerson");
            var options = new ExecutionTaskOptions();
            var execItems = new ExecutionItemCollection();


            var ei = new ExecutionItem(table);
            ei.RepeatCount = 10;
            execItems.Add(ei);

            var col = table.Columns.Where(x => x.IsIdentity).FirstOrDefault();
            Assert.IsNotNull(col);
            Assert.IsNotNull(col.Generator);
            Assert.AreEqual(Generators.Generator.GENERATOR_IdentityFromSqlServerGenerator, col.Generator.GeneratorName);


            var wfm = new WorkflowManager();
            var res = wfm.RunWorkFlow(options, Connection(), execItems);
            foreach (var er in res.ErrorList)
            {
                Console.WriteLine(er);
            }
            Assert.AreEqual(0, res.ErrorList.Count);

        }




        [Test]
        public void ShouldGenerate_DateTime_CurrentDateAllways()
        {
            var genName = Generators.Generator.GENERATOR_CurrentDate;
            ColumnEntity col = CreateColumnOfDatatype("datetime", genName);

            List<DateTime> dates = Create10000FromColumn<DateTime>(col);
            for (int i = 0; i < 10000; i++)
            {
                if (i > 0 && i < 10000)
                {
                    // Accepting that the get dates can be different, but not much
                    Assert.Less(dates[i] - dates[i - 1], new TimeSpan(0, 0, 0, 0, 5));
                }
            }
        }

        [Test]
        public void ShouldGenerate_DateTime_StaticDate()
        {
            var genName = Generators.Generator.GENERATOR_StaticDate;
            ColumnEntity col = CreateColumnOfDatatype("datetime", genName);

            List<DateTime> dates = Create10000FromColumn<DateTime>(col);

            var aDate = (DateTime)dates.FirstOrDefault();
            Assert.IsNotNull(aDate);
            Assert.IsTrue(dates.All(x => x == aDate));
        }
        [Test]
        public void ShouldGenerate_DateTime_MinuteSeries()
        {
            var genName = Generators.Generator.GENERATOR_MinutesSeries;
            ColumnEntity col = CreateColumnOfDatatype("datetime", genName);

            List<DateTime> dates = Create10000FromColumn<DateTime>(col);
            for (int i = 0; i < 10000; i++)
            {
                if (i > 0 && i < 10000)
                {
                    // Accepting that the get dates can be different, but not much
                    Assert.LessOrEqual(dates[i] - dates[i - 1], new TimeSpan(0, 0, 1, 0, 2));
                    Assert.GreaterOrEqual(dates[i] - dates[i - 1], new TimeSpan(0, 0, 1, 0, 0));
                }
            }
        }
        [Test]
        public void ShouldGenerate_DateTime_HourSeries()
        {
            var genName = Generators.Generator.GENERATOR_HoursSeries;
            ColumnEntity col = CreateColumnOfDatatype("datetime", genName);

            List<DateTime> dates = Create10000FromColumn<DateTime>(col);
            for (int i = 0; i < 10000; i++)
            {
                if (i > 0 && i < 10000)
                {
                    // Accepting that the get dates can be different, but not much
                    Assert.LessOrEqual(dates[i] - dates[i - 1], new TimeSpan(0, 1, 0, 0, 2));
                    Assert.GreaterOrEqual(dates[i] - dates[i - 1], new TimeSpan(0, 1, 0, 0, 0));
                }
            }
        }
        [Test]
        public void ShouldGenerate_DateTime_SecondSeries()
        {
            var genName = Generators.Generator.GENERATOR_SecondsSeries;
            ColumnEntity col = CreateColumnOfDatatype("datetime", genName);

            List<DateTime> dates = Create10000FromColumn<DateTime>(col);
            for (int i = 0; i < 10000; i++)
            {
                if (i > 0 && i < 10000)
                {
                    // Accepting that the get dates can be different, but not much
                    Assert.LessOrEqual(dates[i] - dates[i - 1], new TimeSpan(0, 0, 0, 1, 2));
                    Assert.GreaterOrEqual(dates[i] - dates[i - 1], new TimeSpan(0, 0, 0, 1, 0));
                }
            }
        }
        [Test]
        public void ShouldGenerate_DateTime_DaySeries()
        {
            var genName = Generators.Generator.GENERATOR_DaysSeries;
            ColumnEntity col = CreateColumnOfDatatype("datetime", genName);

            List<DateTime> dates = Create10000FromColumn<DateTime>(col);
            for (int i = 0; i < 10000; i++)
            {
                if (i > 0 && i < 10000)
                {
                    // Accepting that the get dates can be different, but not much
                    Assert.LessOrEqual(dates[i] - dates[i - 1], new TimeSpan(1, 0, 0, 0, 2));
                    Assert.GreaterOrEqual(dates[i] - dates[i - 1], new TimeSpan(1, 0, 0, 0, 0));
                }
            }
        }
        [Test]
        public void ShouldGenerate_Long_StaticNumber()
        {
            var genName = Generators.Generator.GENERATOR_StaticNumber;
            ColumnEntity col = CreateColumnOfDatatype("int", genName);

            List<long> longs = Create10000FromColumn<long>(col);
            
            Assert.IsTrue(longs.All(x => 0 == x));
            
        }
                
        [Test]
        public void ShouldGenerate_Long_CountingUp()
        {
            var genName = Generators.Generator.GENERATOR_CountingUpInteger;
            ColumnEntity col = CreateColumnOfDatatype("int", genName);

            List<long> longs = Create10000FromColumn<long>(col);

            for (int i = 0; i < 10000; i++)
            {
                if (i > 0 && i < 10000)
                {
                    Assert.Greater(longs[i], longs[i - 1]);
                    Assert.AreEqual(longs[i] - longs[i - 1], 1);
                }
            }
        }

        [Test]
        public void ShouldGenerate_String_FemaleNames()
        {
            var genName = Generators.Generator.GENERATOR_FemaleNames;
            ColumnEntity col = CreateColumnOfDatatype("nvarchar(900)", genName);

            List<string> longs = Create10000FromColumn<string>(col);

            for (int i = 0; i < 10000; i++)
            {
                Assert.Greater(longs[i].Length, 0);
            }
        }

        [Test]
        public void ShouldGenerate_Long_RandomInt()
        {
            var genName = Generators.Generator.GENERATOR_RandomInt;
            ColumnEntity col = CreateColumnOfDatatype("bigint", genName);

            List<long> longs = Create10000FromColumn<long>(col);

            for (int i = 0; i < 10000; i++)
            {
                if (i > 0 && i < 10000)
                {
                    Assert.AreNotEqual(longs[i], longs[i - 1]);
                }
            }
        }

        [Test]
        public void ShouldGenerate_Decimal_RandomDecimal()
        {
            var genName = Generators.Generator.GENERATOR_RandomDecimal;
            ColumnEntity col = CreateColumnOfDatatype("decimal(19,6)", genName);

            List<decimal> longs = Create10000FromColumn<decimal>(col);

            for (int i = 0; i < 10000; i++)
            {
                if (i > 0 && i < 10000)
                {
                    Assert.AreNotEqual(longs[i], longs[i - 1]);
                }
            }
        }
        [Test]
        public void ShouldGenerate_Decimal_UpCounter()
        {
            var genName = Generators.Generator.GENERATOR_CountingUpDecimal;
            ColumnEntity col = CreateColumnOfDatatype("decimal(19,6)", genName);

            List<decimal> longs = Create10000FromColumn<decimal>(col);

            for (int i = 0; i < 10000; i++)
            {
                if (i > 0 && i < 10000)
                {
                    Assert.AreEqual(1.0, longs[i] - longs[i - 1]);
                    Assert.AreNotEqual(longs[i], longs[i - 1]);
                }
            }
        }

        [Test]
        public void ShouldGetGeneratorsForEverySQLDatatype()
        {
            var dataTypes = new List<string> {"bigint",
"binary",
"bit",
"char",
"date",
"datetime",
"datetime2",
"datetimeoffset",
"decimal",
"float",
"geography",
"geometry",
"hierarchyid",
"image",
"int",
"money",
"nchar",
"ntext",
"numeric",
"nvarchar",
"real",
"smalldatetime",
"smallint",
"smallmoney",
"sql_variant",
"sysname",
"text",
"time",
"timestamp",
"tinyint",
"uniqueidentifier",
"varbinary",
"varchar",
"xml" };

            List<ColumnEntity> cols = new List<ColumnEntity>();
            foreach (var s in dataTypes)
            {
                ColumnEntity col = DatabaseEntityFactory.CreateColumnEntity(s, new ColumnDataTypeDefinition(s, false), false, 1, false, string.Empty, null);
                cols.Add(col);
                Assert.IsNotNull(col.Generator);
                
                Assert.Greater(col.PossibleGenerators.Count, 0);

                for (int i = 0; i < 10000; i++)
                {
                    col.GenerateValue(i);
                    Assert.IsNotNull(col.PreviouslyGeneratedValue);
                }
            }
        }

        private static List<T> Create10000FromColumn<T>(ColumnEntity col)
        {
            List<T> longs = new List<T>();

            for (int i = 0; i < 10000; i++)
            {
                col.GenerateValue(i);
                longs.Add((T)col.PreviouslyGeneratedValue);
            }
            return longs;
        }
        

        private static ColumnEntity CreateColumnOfDatatype(string dataType, string genName)
        {
            ColumnEntity col = DatabaseEntityFactory.CreateColumnEntity("someCol", new ColumnDataTypeDefinition(dataType, false), false, 1, false, string.Empty, null);
            
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

        [Test]
        public void ShouldUseIdentityInsertIfRegularGeneratorSelected()
        {
            var wfm = new WorkflowManager();
            var tda = new TableEntityDataAccess(Connection());
            var options = new ExecutionTaskOptions();
            var newPerson = tda.GetTableAndColumns("Person", "NewPerson");
            var another = tda.GetTableAndColumns("Person", "AnotherTable");
            var i1 = new ExecutionItem(newPerson);
            var i2 = new ExecutionItem(another);
            ExecutionItemCollection items = new ExecutionItemCollection();
            items.Add(i1);
            items.Add(i2);

            var idCol = newPerson.Columns.First();
            Assert.IsTrue(idCol.IsIdentity);

            idCol.Generator = idCol.PossibleGenerators.Where(gen => gen.GeneratorName == Generators.Generator.GENERATOR_RandomInt).FirstOrDefault() ;
            Assert.IsNotNull(idCol.Generator);

            wfm.RunWorkFlow(options, Connection(), items);

        }
    }
}

