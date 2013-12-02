
using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.Generators.IntGenerators;

namespace SQLDataProducer.Tests.ValueGenerators
{
    [TestFixture]
    [MSTest.TestClass]
    public class RandomForeignKeyIntGeneratorTest
    {
        public RandomForeignKeyIntGeneratorTest()
        {
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGenerateValue()
        {
            var gen = new RandomForeignKeyIntGenerator(new ColumnDataTypeDefinition("TinyInt", false));
            var firstValue = gen.GenerateValue(1);
            Assert.That(firstValue, Is.Not.Null);
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