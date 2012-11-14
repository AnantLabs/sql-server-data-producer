﻿using System;
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
        public GeneratorTests()
            : base()
        {

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

            List<double> longs = Create10000FromColumn<double>(col);

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

            List<double> longs = Create10000FromColumn<double>(col);

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
                ColumnEntity col = DatabaseEntityFactory.CreateColumnEntity(s, new ColumnDataTypeDefinition(s, false), false, 1, false, null);
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
            ColumnEntity col = DatabaseEntityFactory.CreateColumnEntity("someCol", new ColumnDataTypeDefinition(dataType, false), false, 1, false, null);
            
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

