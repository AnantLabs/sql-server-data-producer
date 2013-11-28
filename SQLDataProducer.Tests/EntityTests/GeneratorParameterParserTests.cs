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
using SQLDataProducer.Entities.Generators;


namespace SQLDataProducer.Tests.Entities
{
    [TestFixture]
    [MSTest.TestClass]
    public class GeneratorParameterParserTests : TestBase
    {
        GeneratorParameterParser dateTimeParser = GeneratorParameterParser.DateTimeParser;
        GeneratorParameterParser decimalParser = GeneratorParameterParser.DecimalParser;
        GeneratorParameterParser stringParser = GeneratorParameterParser.StringParser;
        GeneratorParameterParser intParser = GeneratorParameterParser.IntegerParser;
        GeneratorParameterParser longParser = GeneratorParameterParser.LonglParser;
        GeneratorParameterParser objectParser = GeneratorParameterParser.ObjectParser;


        [Test]
        [MSTest.TestMethod]
        public void ShouldBeEqualityWhenEqual()
        {
            GeneratorParameterParser dateTimeParser1 = GeneratorParameterParser.DateTimeParser;
            GeneratorParameterParser dateTimeParser2 = GeneratorParameterParser.DateTimeParser;
            GeneratorParameterParser dateTimeParser3 = GeneratorParameterParser.DateTimeParser;

            Assert.That(dateTimeParser, Is.EqualTo(dateTimeParser));
            AssertEqualsDefaultBehaviour(dateTimeParser1, dateTimeParser2, dateTimeParser3);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldNotBeEqualWhenNotEqual()
        {
            Assert.That(dateTimeParser, Is.EqualTo(dateTimeParser));
            Assert.That(intParser, Is.EqualTo(intParser));

            Assert.That(dateTimeParser, Is.Not.EqualTo(intParser));
            Assert.That(dateTimeParser, Is.Not.EqualTo(decimalParser));
            Assert.That(dateTimeParser, Is.Not.EqualTo(stringParser));
            Assert.That(dateTimeParser, Is.Not.EqualTo(intParser));
            Assert.That(dateTimeParser, Is.Not.EqualTo(longParser));
            Assert.That(dateTimeParser, Is.Not.EqualTo(objectParser));

        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldNotChangeValueWhenTryingToSetValueOfWrongDataType()
        {
            DateTime d = DateTime.Now;
            GeneratorParameter p = new GeneratorParameter("date", d, dateTimeParser);
            Assert.That(d, Is.EqualTo(p.Value));

            // Try to Set value to wrong datatype
            p.Value = 10;

            Assert.That(d, Is.EqualTo(p.Value));

        }


        [Test]
        [MSTest.TestMethod]
        public void ShouldParseDateTime()
        {
            DateTime originalDate = DateTime.Now;
            string s = dateTimeParser.FormatToString(originalDate);
            DateTime parsedDate = (DateTime)dateTimeParser.ParseValue(s);

            Assert.That(originalDate, Is.EqualTo(parsedDate));


            string aDate = "2013-01-23T14:32:20.7326289";
            DateTime fromString = DateTime.Parse(aDate);
            string aString = dateTimeParser.FormatToString(fromString);
            DateTime pd2 = (DateTime)dateTimeParser.ParseValue(aString);
            Assert.That(aDate, Is.EqualTo(aString));
            Assert.That(fromString, Is.EqualTo(pd2));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldParseInt()
        {
            object boxed = 10;
            int parsed = (int)intParser.ParseValue(boxed);
            Assert.That(boxed, Is.EqualTo(parsed));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldParseLong()
        {
            object boxed = 100000000000;
            long parsed = (long)longParser.ParseValue(boxed);
            Assert.That(boxed, Is.EqualTo(parsed));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldParseString()
        {
            object boxed = "peter";
            string parsed = (string)stringParser.ParseValue(boxed);
            Assert.That(boxed, Is.EqualTo(parsed));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldParseObject()
        {
            object boxed = new System.Collections.ArrayList();
            System.Collections.ArrayList parsed = (System.Collections.ArrayList)objectParser.ParseValue(boxed);
            Assert.That(boxed, Is.EqualTo(parsed));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldParseDecimal()
        {
            object boxed = 1.1111;
            decimal parsed = (decimal)decimalParser.ParseValue(boxed);
            decimal parsedFromString = (decimal)decimalParser.ParseValue("1,1111");
            decimal parsedFromString2 = (decimal)decimalParser.ParseValue("1.1111");

            Assert.That(boxed, Is.EqualTo(parsed));
            Assert.That(boxed, Is.EqualTo(parsedFromString));
            Assert.That(boxed, Is.EqualTo(parsedFromString2));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGetGeneratorParameterParserFromName()
        {
            var parser = GeneratorParameterParser.FromName("Decimal Parser");
            Assert.That(parser, Is.Not.Null);
        }

    }
}
