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


using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.Generators.IntGenerators;
using System;
using System.Collections.Generic;
using SQLDataProducer.Entities.Generators;

namespace SQLDataProducer.Tests.ValueGenerators
{
    [TestFixture]
    [MSTest.TestClass]
    public class CountingUpIntGeneratorTest
    {
        public CountingUpIntGeneratorTest()
        {
            dataTypesToTest.Add(new ColumnDataTypeDefinition("tinyint", false));
            dataTypesToTest.Add(new ColumnDataTypeDefinition("bit", false));
            dataTypesToTest.Add(new ColumnDataTypeDefinition("bigint", false));
            dataTypesToTest.Add(new ColumnDataTypeDefinition("int", false));
            dataTypesToTest.Add(new ColumnDataTypeDefinition("smallint", false));
        }

        private List<ColumnDataTypeDefinition> dataTypesToTest = new List<ColumnDataTypeDefinition>();

        private IEnumerable<IntegerGeneratorBase> PossibleGenerators
        {
            get
            {
                foreach (var dt in dataTypesToTest)
                {
                    Console.WriteLine("Now testing " + dt.Raw);
                    yield return new CountingUpIntGenerator(dt);
                }
            }
        }

        private class MockIntegerGenerator : IntegerGeneratorBase
        {
            public MockIntegerGenerator(string name, ColumnDataTypeDefinition dataType) 
                : base(name,dataType, false)
            {

            }

            protected override object InternalGenerateValue(long n, SQLDataProducer.Entities.Generators.Collections.GeneratorParameterCollection paramas)
            {
                return null;    
            }
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldEqualIfSameName()
        {
            foreach (var generatorUnderTest in PossibleGenerators)
            {
                var mockGenerator = new MockIntegerGenerator(generatorUnderTest.GeneratorName, new ColumnDataTypeDefinition("varchar", false));
                Assert.That(generatorUnderTest, Is.EqualTo(mockGenerator));
                Assert.That(generatorUnderTest.Equals(mockGenerator));

                mockGenerator = new MockIntegerGenerator("Mock", new ColumnDataTypeDefinition("varchar", false));
                Assert.That(generatorUnderTest, Is.Not.EqualTo(mockGenerator));
                Assert.That(!generatorUnderTest.Equals(mockGenerator));
            }
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldIncrementForEachN()
        {
            foreach (var generator in PossibleGenerators)
            {
                for (int n = 1; n < 10; n++)
                {
                     long maxValue = generator.GeneratorParameters.GetValueOf<long>("MaxValue");
                     if (n < maxValue)
                     {
                         var value = generator.GenerateValue(n);
                         Assert.That(value, Is.EqualTo(n));
                     }
                }
            }
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldHaveOneAsStartValue()
        {
            foreach (var generator in PossibleGenerators)
            {
                Assert.That(generator.GenerateValue(), Is.EqualTo(1));
            }
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldIncrementCounterWhenNoNValueIsSupplied()
        {
            foreach (var generator in PossibleGenerators)
            {
                Assert.That(generator.GenerateValue(), Is.EqualTo(1));
                long maxValue = generator.GeneratorParameters.GetValueOf<long>("MaxValue");
                if (maxValue != 1)
                {
                    Assert.That(generator.GenerateValue(), Is.EqualTo(2));
                }
                Assert.That(generator.GenerateValue(1), Is.EqualTo(1), "Should use the supplied for generation");
            }
        }


        [Test]
        [MSTest.TestMethod]
        public void ShouldHaveOnlyTestedParameters()
        {
            foreach (var generator in PossibleGenerators)
            {
                Assert.That(generator.GeneratorParameters.Count, Is.EqualTo(4));
            }
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldStartAtTheConfiguredStartValue()
        {
            foreach (var generator in PossibleGenerators)
            {
                long maxValue = generator.GeneratorParameters.GetValueOf<long>("MaxValue");
                if (maxValue != 1)
                {
                    generator.GeneratorParameters["StartValue"].Value = 10;
                    Assert.That(generator.GenerateValue(1), Is.EqualTo(10));
                    Assert.That(generator.GenerateValue(2), Is.EqualTo(11));
                }
            }
        }


        [Test]
        [MSTest.TestMethod]
        public void ShouldStepTheConfiguredAmount()
        {
            foreach (var generator in PossibleGenerators)
            {
                 long maxValue = generator.GeneratorParameters.GetValueOf<long>("MaxValue");
                 if (maxValue != 1)
                 {
                     generator.GeneratorParameters["Step"].Value = 10;
                     Assert.That(generator.GenerateValue(1), Is.EqualTo(1));
                     Assert.That(generator.GenerateValue(2), Is.EqualTo(11));
                 }
            }
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldCountBackwardsOnNegativeStep()
        {
            foreach (var generator in PossibleGenerators)
            {
                long maxValue = generator.GeneratorParameters.GetValueOf<long>("MaxValue");
                if (maxValue != 1)
                {
                    generator.GeneratorParameters["Step"].Value = -1;
                    Assert.That(generator.GenerateValue(1), Is.EqualTo(1));
                    Assert.That(generator.GenerateValue(2), Is.EqualTo(0));
                }
            }
        }


        [Test]
        [MSTest.TestMethod]
        public void ShouldOverFlowToTheStartValue()
        {
            foreach (var generator in PossibleGenerators)
            {
                long maxValue = generator.GeneratorParameters.GetValueOf<long>("MaxValue");
                if (maxValue != 1 && maxValue != long.MaxValue)
                {
                    Assert.That(generator.GenerateValue(maxValue), Is.EqualTo(maxValue));
                    Assert.That(generator.GenerateValue(maxValue + 1), Is.EqualTo(1));
                    Assert.That(generator.GenerateValue(maxValue + 2), Is.EqualTo(2));

                    generator.GeneratorParameters["MaxValue"].Value = 10;
                    Assert.That(generator.GenerateValue(11), Is.EqualTo(1));
                    Assert.That(generator.GenerateValue(12), Is.EqualTo(2));
                }
            }
        }


        [MSTest.TestMethod]
        public void ShouldTestBit()
        {
            CountingUpIntGenerator generator = new CountingUpIntGenerator(new ColumnDataTypeDefinition("bit", false));
            generator.GenerateValue();
        }
    }
}