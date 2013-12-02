
using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.Generators.XMLGenerators;

namespace SQLDataProducer.Tests.ValueGenerators
{
    [TestFixture]
    [MSTest.TestClass]
    public class NullValueXmlGeneratorTest
    {
        public NullValueXmlGeneratorTest()
        {
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGenerateValue()
        {
            var gen = new NullValueXmlGenerator(new ColumnDataTypeDefinition("Xml", false));
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