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
using SQLDataProducer.Entities.Generators.Collections;
using SQLDataProducer.Entities.Generators;


namespace SQLDataProducer.Tests.Entities.Collections
{
    [TestFixture]
    [MSTest.TestClass]
    public class GeneratorParameterCollectionTests : TestBase
    {

        [Test]
        [MSTest.TestMethod]
        public void ShouldGetParameterFromCollection()
        {
            GeneratorParameterCollection coll = new GeneratorParameterCollection();
            var gp1 = new GeneratorParameter("Date", DateTime.Now, GeneratorParameterParser.DateTimeParser);
            var gp2 = new GeneratorParameter("Decimal", new Decimal(10), GeneratorParameterParser.DecimalParser);
            coll.Add(gp1);
            coll.Add(gp2);

            Decimal got = coll.GetValueOf<Decimal>("Decimal");

            DateTime got2 = coll.GetValueOf<DateTime>("Date");
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGetExceptionWhenTryingToGetWrongDataTypeFromParameterCollection()
        {
            GeneratorParameterCollection coll = new GeneratorParameterCollection();
            var gp1 = new GeneratorParameter("Date", DateTime.Now, GeneratorParameterParser.DateTimeParser);
            var gp2 = new GeneratorParameter("Decimal", "peter", GeneratorParameterParser.StringParser);
            coll.Add(gp1);
            coll.Add(gp2);

            bool gotException = false;
            try
            {
                Decimal got = coll.GetValueOf<Decimal>("Decimal");
            }
            catch (Exception)
            {
                gotException = true;
            }
            Assert.That(gotException, Is.True);

        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldParseTheParameterToCorrectTypeEvenIfInputIsNotOfSameType()
        {
            GeneratorParameterCollection coll = new GeneratorParameterCollection();
            var gp1 = new GeneratorParameter("Date", DateTime.Now.ToString(), GeneratorParameterParser.DateTimeParser);
            var gp2 = new GeneratorParameter("Decimal", 1000, GeneratorParameterParser.DecimalParser);
            coll.Add(gp1);
            coll.Add(gp2);

            Decimal dec = coll.GetValueOf<Decimal>("Decimal");
            DateTime date = coll.GetValueOf<DateTime>("Date");
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldBeAbleToCloneGeneratorParameterCollection()
        {
            GeneratorParameterCollection coll = new GeneratorParameterCollection();
            var gp1 = new GeneratorParameter("Date", DateTime.Now, GeneratorParameterParser.DateTimeParser);
            coll.Add(gp1);
            
            var coll2 = coll.Clone();

            //CollectionAssert.AreEqual(coll, coll2);
            
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGetExceptionIfParameterIsMissing()
        {
            GeneratorParameterCollection coll = new GeneratorParameterCollection();
            bool exceptionHappened = false;
            try
            {
                coll.GetValueOf<string>("unknown");
            }
            catch (Exception)
            {
                exceptionHappened = true;
            }
            Assert.That(exceptionHappened);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGetNullIfParameteHasNullValue()
        {
            bool exceptionHappened = false;
            try
            {
                GeneratorParameterCollection coll = new GeneratorParameterCollection();
                coll.Add(new GeneratorParameter("Date", null, GeneratorParameterParser.DateTimeParser));
                coll.GetValueOf<DateTime>("Date");
            }
            catch (ArgumentNullException)
            {
                exceptionHappened = true;
            }
            Assert.That(exceptionHappened);
        }
    }

}
