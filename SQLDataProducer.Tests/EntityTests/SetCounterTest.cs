
using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.Generators.IntGenerators;
using SQLDataProducer.Entities;

namespace SQLDataProducer.Tests.EntityTests
{

    [TestFixture]
    [MSTest.TestClass]
    public class SetCounterTest 
    {

        [MSTest.TestMethod]
        public void ShouldHaveZeroAsStartValue()
        {
            SetCounter counter = new SetCounter();
            long initialValue = counter.Peek();
            Assert.That(initialValue, Is.EqualTo(0));
        }

        [MSTest.TestMethod]
        public void ShouldHaveOneAsTheFirstValue()
        {
            SetCounter counter = new SetCounter();

            Assert.That(counter.GetNext(), Is.EqualTo(1));
        }


        [MSTest.TestMethod]
        public void ShouldResetToZero()
        {
            SetCounter counter = new SetCounter();

            for (int i = 0; i < 100; i++)
			{
                Assert.That(counter.GetNext(), Is.EqualTo(i + 1));
			}

            counter.Reset();
            Assert.That(counter.Peek(), Is.EqualTo(0));
        }
        
        [MSTest.TestMethod]
        public void ShouldIncrement()
        {
            SetCounter counter = new SetCounter();

            for (int j = 0; j < 100; j++)
            {
                for (int i = 0; i < 100; i++)
                {
                    counter.Increment();
                    Assert.That(counter.Peek(), Is.EqualTo(i + 1));
                }
                counter.Reset();
            }
        }


        [MSTest.TestMethod]
        public void ShouldCompareTwoNumbers()
        {
            SetCounter a = new SetCounter();
            SetCounter b = new SetCounter();

            a.Add(10);
            b.Add(5);
            b.Add(5);

            Assert.That(a.Peek(), Is.EqualTo(b.Peek()));
            Assert.That(a.IsEqual(b.Peek()), Is.True);
        }

        [MSTest.TestMethod]
        public void ShouldAddToCounter()
        {
            SetCounter counter = new SetCounter();

            counter.Add(10);
            Assert.That(counter.Peek(), Is.EqualTo(10));
        }
    }
}