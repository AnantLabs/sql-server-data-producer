
using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.Generators.StringGenerators;

namespace SQLDataProducer.Tests.ValueGenerators
{
    [TestFixture]
    [MSTest.TestClass]
    public class UserNameStringGeneratorTest
    {
        public UserNameStringGeneratorTest()
        {
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGenerateValue()
        {
            var gen = new UserNameStringGenerator(new ColumnDataTypeDefinition("VarChar(123)", false));
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