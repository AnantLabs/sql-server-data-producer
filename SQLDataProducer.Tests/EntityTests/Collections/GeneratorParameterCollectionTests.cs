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
        public void ShouldBeAbleToCloneGeneratorParameterCollection()
        {
            GeneratorParameterCollection coll = new GeneratorParameterCollection();
            var gp1 = new GeneratorParameter("Date", DateTime.Now, GeneratorParameterParser.DateTimeParser);
            coll.Add(gp1);

            Assert.Fail("Did i not used to have a clone method?");
            var coll2 = coll;//.Clone();

            Assert.That(coll2, Is.EqualTo(coll));

            // Changing the value should cause the collections to be unequal, value should only be changed in one of the collections.
            gp1.Value = DateTime.Now.AddDays(1);
            Assert.That(coll2, Is.Not.EqualTo(coll));

        }
    }
}
