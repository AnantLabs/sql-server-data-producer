
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
        }


        private IntegerGeneratorBase NextGenerator
        {
            get
            {
                return new CountingUpIntGenerator(new ColumnDataTypeDefinition("TinyInt", false));
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
            var generatorUnderTest = NextGenerator;
            var mockGenerator = new MockIntegerGenerator(generatorUnderTest.GeneratorName, new ColumnDataTypeDefinition("varchar", false));
            Assert.That(generatorUnderTest, Is.EqualTo(mockGenerator));
            Assert.That(generatorUnderTest.Equals(mockGenerator));

            mockGenerator = new MockIntegerGenerator("Mock", new ColumnDataTypeDefinition("varchar", false));
            Assert.That(generatorUnderTest, Is.Not.EqualTo(mockGenerator));
            Assert.That(!generatorUnderTest.Equals(mockGenerator));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldIncrementForEachN()
        {
            var generator = NextGenerator;

            for (int n = 1; n < 10; n++)
            {
                var value = generator.GenerateValue(n);
                Assert.That(value, Is.EqualTo(n));
            }
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldHaveOnlyTestedParameters()
        {
            var generator = NextGenerator;
            Assert.That(generator.GeneratorParameters.Count, Is.EqualTo(4));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldStartAtTheConfiguredStartValue()
        {
            var generator = NextGenerator;
            generator.GeneratorParameters["StartValue"].Value = 10;
            Assert.That(generator.GenerateValue(1), Is.EqualTo(10));
            Assert.That(generator.GenerateValue(2), Is.EqualTo(11));
        }


        [Test]
        [MSTest.TestMethod]
        public void ShouldStepTheConfiguredAmount()
        {
            var generator = NextGenerator;
            generator.GeneratorParameters["Step"].Value = 10;
            Assert.That(generator.GenerateValue(1), Is.EqualTo(1));
            Assert.That(generator.GenerateValue(2), Is.EqualTo(11));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldCountBackwardsOnNegativeStep()
        {
            var generator = NextGenerator;
            generator.GeneratorParameters["Step"].Value = -1;
            Assert.That(generator.GenerateValue(1), Is.EqualTo(1));
            Assert.That(generator.GenerateValue(2), Is.EqualTo(0));
        }


        [Test]
        [MSTest.TestMethod]
        public void ShouldOverFlowToTheStartValue()
        {
            var generator = NextGenerator;
            long maxValue = generator.GeneratorParameters.GetValueOf<long>("MaxValue");
            Assert.That(generator.GenerateValue(maxValue + 1), Is.EqualTo(1));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldTestOverFlow()
        {
            
        }
    }
}