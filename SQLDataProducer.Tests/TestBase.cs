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
using SQLDataProducer.DataAccess;
using SQLDataProducer.Entities.DatabaseEntities;
using Generators = SQLDataProducer.Entities.Generators;
using SQLDataProducer.DataConsumers;

namespace SQLDataProducer.RandomTests
{
    public class TestBase
    {
        protected IEnumerable<Generators.Generator> gens = Generators.Generator.GetDateTimeGenerators()
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


        protected IDataConsumer DefaultDataConsumer = new SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer.InsertComsumer();

        public TestBase()
        {
            DefaultDataConsumer.Init(Connection());

            string sql = @"

if exists (select 1 from INFORMATION_SCHEMA.tables where TABLE_NAME = 'AnotherTable' and TABLE_SCHEMA = 'Person')
	drop table Person.AnotherTable;

if exists (select 1 from INFORMATION_SCHEMA.tables where TABLE_NAME = 'NewPerson' and TABLE_SCHEMA = 'Person')
	drop table Person.NewPerson;
	
	
create table Person.NewPerson(
	NewPersonId int identity(1, 1) primary key,
	Name varchar(500) not null,
	BitColumn bit not null, 
	DecimalColumn decimal(10, 4) not null,
	BigintColumn bigint not null, 
	VarcharMaxColumn varchar(max)  not null,
	FloatColumn float not null,
	DateTime2Column datetime2 not null,
	DateTimeColumn datetime not null,
	NCharFiveColumn nchar(5) not null,
	DateColumn date not null, 
	TimeColumn time not null,
	SmallIntColumn smallint not null,
	SmallDateTimeColumn smalldatetime not null,
	SmallMoneyColumn smallmoney  not null
);

create table Person.AnotherTable(
	NewPersonId int foreign key references Person.NewPerson(NewPersonId),
	AnotherColumn char(1),
    ThirdColumn char(1) not null
);";
            AdhocDataAccess adhd = new AdhocDataAccess(Connection());
            adhd.ExecuteNonQuery(sql);

        }
        public static string Connection()
        {
            return "Data Source=localhost;Initial Catalog=AdventureWorks;Integrated Security=True";
        }



        protected void AssertColumn(ColumnEntity expectedColumn, ColumnEntity newColumn)
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

        /// <summary>
        /// Check that the provided objects comply with the default Equals behaviour specified by Microsoft.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="y"></param>
        /// <remarks>http://msdn.microsoft.com/en-us/library/ms173147(v=vs.80).aspx</remarks>
        protected static void AssertEqualsDefaultBehaviour<T>(T x, T z, T y) where T : IEquatable<T>
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

    }
}
