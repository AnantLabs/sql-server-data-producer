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

using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.Generators.DecimalGenerators;

namespace SQLDataProducer.Tests.ValueGenerators
{
    [TestFixture]
    [MSTest.TestClass]
    public class CountingUpDecimalGeneratorTest
    {
        public CountingUpDecimalGeneratorTest()
        {
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGenerateValue()
        {
            var gen = new CountingUpDecimalGenerator(new ColumnDataTypeDefinition("decimal(19, 6)", false));
            var firstValue = gen.GenerateValue(1);
            Assert.That(firstValue, Is.EqualTo(0));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldTestStep()
        {
            Assert.Fail("not implemented");
        }
        [Test]
        [MSTest.TestMethod]
        public void ShouldTestStartValue()
        {
            Assert.Fail("not implemented");
        }
        [Test]
        [MSTest.TestMethod]
        public void ShouldTestOverFlow()
        {
            Assert.Fail("not implemented");
        }
    }
}
