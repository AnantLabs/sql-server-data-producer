// Copyright 2012-2014 Peter Henell

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
    public class GeneratorParameterTests : TestBase
    {

        GeneratorParameter p1 = new GeneratorParameter("peter", 1, GeneratorParameterParser.IntegerParser);
        GeneratorParameter p2 = new GeneratorParameter("peter", 1, GeneratorParameterParser.IntegerParser);
        GeneratorParameter p3 = new GeneratorParameter("peter", 1.0, GeneratorParameterParser.IntegerParser);
        GeneratorParameter nullGenParam = null;

        [Test]
        [MSTest.TestMethod]
        public void ShouldBeEqualityWhenEqual()
        {
            Assert.That(p1, Is.EqualTo(p1));
            Assert.That(p1, Is.EqualTo(p2));

            AssertEqualsDefaultBehaviour(p1, p2, p3);

            Assert.That(p2.GetHashCode(), Is.EqualTo(p3.GetHashCode()));
            Assert.That(p1.GetHashCode(), Is.EqualTo(p2.GetHashCode()));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGetTheCorrectDatatypeFromParameter()
        {
            var p = new GeneratorParameter("test", new decimal(1.0), GeneratorParameterParser.DecimalParser);
            Assert.That(p.Value, Is.Not.Null);
           // make sure that value is castable to decimal
            decimal val = (decimal)p.Value;
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldBeEqualIfSameName()
        {
            GeneratorParameter withOtherValue = new GeneratorParameter("peter", 10, GeneratorParameterParser.IntegerParser);
            GeneratorParameter withOtherName = new GeneratorParameter("Generator", 1, GeneratorParameterParser.IntegerParser);
            GeneratorParameter withOtherParser = new GeneratorParameter("peter", 1, GeneratorParameterParser.DecimalParser);
            GeneratorParameter withAllPropertiesChanged = new GeneratorParameter("generator", "streng", GeneratorParameterParser.StringParser);

            Assert.That(p1, Is.Not.EqualTo(nullGenParam));

            Assert.That(p1, Is.EqualTo(withOtherValue));
            Assert.That(p1, Is.Not.EqualTo(withOtherName));
            Assert.That(p1, Is.EqualTo(withOtherParser));
            Assert.That(p1, Is.Not.EqualTo(withAllPropertiesChanged));

        }
    }
}
