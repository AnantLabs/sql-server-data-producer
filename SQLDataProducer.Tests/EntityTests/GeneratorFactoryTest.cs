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
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DatabaseEntities.Factories;
using Generators = SQLDataProducer.Entities.Generators;
using SQLDataProducer.Entities;
using SQLDataProducer.Entities.DataEntities;
using SQLDataProducer.Entities.Generators;
using System.Collections.ObjectModel;

namespace SQLDataProducer.Tests
{
    [TestFixture]
    [MSTest.TestClass]
    public class GeneratorFactoryTest : TestBase
    {
        public GeneratorFactoryTest()
            : base()
        {
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGetAllIntGenerators()
        {
            ObservableCollection<Generator> gens = GeneratorFactory.GetAllGeneratorsForType(System.Data.SqlDbType.BigInt);
            Assert.That(gens.Count, Is.GreaterThan(0));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGetGeneratorsForDatatype()
        {
            ExpectMoreNullableThanRegular("BigInt", 9);
            ExpectMoreNullableThanRegular("Bit", 0);
            ExpectMoreNullableThanRegular("Char", 8);
            ExpectMoreNullableThanRegular("Date", 9);
            ExpectMoreNullableThanRegular("DateTime", 9);
            ExpectMoreNullableThanRegular("DateTime2", 9);
            ExpectMoreNullableThanRegular("SmallDateTime", 9);
            ExpectMoreNullableThanRegular("Time", 9);
            ExpectMoreNullableThanRegular("DateTimeOffset", 9);
            ExpectMoreNullableThanRegular("Decimal", 2);
            ExpectMoreNullableThanRegular("Float", 2);
            ExpectMoreNullableThanRegular("Real", 2);
            ExpectMoreNullableThanRegular("Money", 2);
            ExpectMoreNullableThanRegular("SmallMoney", 2);
            ExpectMoreNullableThanRegular("Binary", 0);
            ExpectMoreNullableThanRegular("Image", 0);
            ExpectMoreNullableThanRegular("Timestamp", 0);
            ExpectMoreNullableThanRegular("VarBinary(10)", 0);
            ExpectMoreNullableThanRegular("int", 9);
            ExpectMoreNullableThanRegular("NChar(10)", 8);
            ExpectMoreNullableThanRegular("NText", 8);
            ExpectMoreNullableThanRegular("NVarChar(10)", 8);
            ExpectMoreNullableThanRegular("Text", 8);
            ExpectMoreNullableThanRegular("VarChar(100)", 8);
            ExpectMoreNullableThanRegular("VarChar(max)", 8);
            ExpectMoreNullableThanRegular("SmallInt", 9);
            ExpectMoreNullableThanRegular("TinyInt", 9);
            ExpectMoreNullableThanRegular("UniqueIdentifier", 0);
            ExpectMoreNullableThanRegular("Variant", 9);
            ExpectMoreNullableThanRegular("Xml", 0);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGetExceptionWhenTryingToGetGeneratorForNonNullableDatatypeThatIsNotImplemented()
        {
            ExpectEmptyListForNonNullableType("udt");
            ExpectEmptyListForNonNullableType("Structured");
        }

        [Test]
        [MSTest.TestMethod]
        public void shouldGetGeneratorForNullableDatatypeEvenIfGeneratorIsNotImplemented()
        {
            var gens = GeneratorFactory.GetGeneratorsForDataType(new ColumnDataTypeDefinition("udt", true));
            var gens2 = GeneratorFactory.GetGeneratorsForDataType(new ColumnDataTypeDefinition("Structured", true));

            Assert.That(gens.Count, Is.EqualTo(1));
            Assert.That(gens2.Count, Is.EqualTo(1));
        }

        private void ExpectEmptyListForNonNullableType(string datatype)
        {
          
          var gens = GeneratorFactory.GetGeneratorsForDataType(new ColumnDataTypeDefinition(datatype, false));
          Assert.That(gens.Count, Is.EqualTo(0), "should not find generators for datatype, but did: " + datatype);
        }

        private void ExpectMoreNullableThanRegular(string datatype, int expectedGens)
        {
            var gens = GeneratorFactory.GetGeneratorsForDataType(new ColumnDataTypeDefinition(datatype, false));
            Assert.That(gens.Count, Is.GreaterThan(expectedGens));
            var nullable = GeneratorFactory.GetGeneratorsForDataType(new ColumnDataTypeDefinition(datatype, true));
            Assert.That(nullable.Count, Is.GreaterThan(gens.Count));
        }

        [Test]
        [MSTest.TestMethod]
        public void shouldHaveLimitedMaxAndMinValuesOfDecimalGenerators()
        {
            AssertMaxValue("decimal(19, 6)", 100000000000000000);
            AssertMaxValue("float", 100000000000000000);
            AssertMaxValue("Real", 100000000000000000);
            AssertMaxValue("Money", 922337203685470);
            AssertMaxValue("SmallMoney", 214740);

         
        }
        
        [Test]
        [MSTest.TestMethod]
        public void shouldHaveLimitedMaxAndMinValuesOfIntegerGenerators()
        {
            AssertMaxValue("int", int.MaxValue);
            AssertMaxValue("BigInt", long.MaxValue);
            AssertMaxValue("Bit", 1);
            AssertMaxValue("SmallInt", short.MaxValue);
            AssertMaxValue("TinyInt", byte.MaxValue);
        }
          
        private static void AssertMaxValue(string datatype, long maxValue)
        {
            var gens = GeneratorFactory.GetGeneratorsForDataType(new ColumnDataTypeDefinition(datatype, false));
            Assert.That(gens.Count, Is.GreaterThan(0));

            foreach (var g in gens)
            {
                Console.WriteLine(g.GeneratorName);
                var maxParam = g.GeneratorParameters.GetParameterByName("MaxValue");
                Assert.That(maxParam, Is.Not.Null);
                Assert.That(maxParam, Is.EqualTo(maxValue), g.GeneratorName);
            }
        }
    }
}
