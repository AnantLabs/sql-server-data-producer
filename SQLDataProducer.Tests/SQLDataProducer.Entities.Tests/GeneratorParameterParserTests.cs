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
using System.Data;
using SQLDataProducer.Entities.Generators;


namespace SQLDataProducer.RandomTests
{
    [TestFixture]
    public class GeneratorParameterParserTests : TestBase
    {
        GeneratorParameterParser dateTimeParser = GeneratorParameterParser.DateTimeParser;
        GeneratorParameterParser decimalParser = GeneratorParameterParser.DecimalParser;
        GeneratorParameterParser stringParser = GeneratorParameterParser.StringParser;
        GeneratorParameterParser intParser = GeneratorParameterParser.IntegerParser;
        GeneratorParameterParser longParser = GeneratorParameterParser.LonglParser;
        GeneratorParameterParser objectParser = GeneratorParameterParser.ObjectParser;


        [Test]
        public void ShouldBeEqualityWhenEqual()
        {
            Assert.That(dateTimeParser, Is.EqualTo(dateTimeParser));
        }

        [Test]
        public void ShouldNotBeEqualWhenNotEqual()
        {
            Assert.That(dateTimeParser, Is.EqualTo(intParser));
        }

        [Test]
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
        public void ShouldParseInt()
        {
            object boxed = 10;
            int parsed = (int)intParser.ParseValue(boxed);
            Assert.That(boxed, Is.EqualTo(parsed));
        }

        [Test]
        public void ShouldParseLong()
        {
            object boxed = 100000000000;
            long parsed = (long)longParser.ParseValue(boxed);
            Assert.That(boxed, Is.EqualTo(parsed));
        }

        [Test]
        public void ShouldParseString()
        {
            object boxed = "peter";
            string parsed = (string)stringParser.ParseValue(boxed);
            Assert.That(boxed, Is.EqualTo(parsed));
        }

        [Test]
        public void ShouldParseObject()
        {
            object boxed = new System.Collections.ArrayList();
            System.Collections.ArrayList parsed = (System.Collections.ArrayList)objectParser.ParseValue(boxed);
            Assert.That(boxed, Is.EqualTo(parsed));
        }

        [Test]
        public void ShouldParseDecimal()
        {
            object boxed = 1.1111;
            double parsed = (double)decimalParser.ParseValue(boxed);
            double parsedFromString = (double)decimalParser.ParseValue("1,1111");
            double parsedFromString2 = (double)decimalParser.ParseValue("1.1111");
            
            Assert.That(boxed, Is.EqualTo(parsed));
            Assert.That(boxed, Is.EqualTo(parsedFromString));
            Assert.That(boxed, Is.EqualTo(parsedFromString2));
            
        }

        [Test]
        public void ShouldGetGeneratorParameterParserFromName()
        {

        }

    }
}
