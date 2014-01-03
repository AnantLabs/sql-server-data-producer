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
using System.Threading.Tasks;
using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.Entities.DataEntities;
using System.Threading;

namespace SQLDataProducer.Tests.EntityTests
{
    [TestFixture]
    [MSTest.TestClass]
    public class ExecutionResultBuilderTest
    {

        [Test]
        [MSTest.TestMethod]
        public void ShouldStartAndBuildBuilder()
        {
            ExecutionResultBuilder builder = new ExecutionResultBuilder();
            builder.Begin();
            builder.AddError(new Exception(), new DataRowEntity(null));
            Thread.Sleep(1); // to get different enddate than startdate
            builder.Increment();

            ExecutionResult result = builder.Build();
           
            Assert.That(result.InsertCount, Is.EqualTo(1));
            Assert.That(result.Errors.Count, Is.EqualTo(1));
            Assert.That(result.Duration, Is.Not.Null);
            Assert.That(result.StartTime, Is.Not.Null);
            Assert.That(result.EndTime, Is.Not.Null);

            Assert.That(result.EndTime, Is.GreaterThan(result.StartTime));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldNotBeAbleToPerformActionsOnBuilderBeforeStaringIt()
        {
            ExecutionResultBuilder builder = new ExecutionResultBuilder();
            ExpectedExceptionHappened<InvalidOperationException>(new Action(() => { builder.Increment(); }), "should throw exception if using un-initialized builder");
            ExpectedExceptionHappened<InvalidOperationException>(new Action(() => { builder.AddError(new ArgumentNullException(), null); }), "should throw exception if using un-initialized builder");
            ExpectedExceptionHappened<InvalidOperationException>(new Action(() => { builder.Build(); }), "should throw exception if using un-initialized builder");
        }

        private bool ExpectedExceptionHappened<T>(Action testInAction, string testName) where T : Exception
        {
            bool exceptionHappened = false;
            try
            {
                testInAction();
            }
            catch (T ex)
            {
                Console.WriteLine(ex.ToString());
                exceptionHappened = true;
            }
            Assert.That(exceptionHappened, "Exception did not happen during test: " + testName);

            return exceptionHappened;
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldResetAllValuesIfCallingBeginSeveralTimes()
        {
            ExecutionResultBuilder builder = new ExecutionResultBuilder();
            builder.Begin();
            builder.Increment();
            builder.Increment();
            builder.Increment();
            builder.AddError(new Exception(), null);
            var result = builder.Build();
            Assert.That(result.InsertCount, Is.EqualTo(3));
            Assert.That(result.Errors.Count, Is.EqualTo(1));

            ExpectedExceptionHappened<InvalidOperationException>(new Action(() => { builder.Build(); }), "should not be able to build again before calling begin");

            builder.Begin();
            builder.Increment();
            var result2 = builder.Build();
            Assert.That(result2.InsertCount, Is.EqualTo(1));
            Assert.That(result2.Errors.Count, Is.EqualTo(0));

        }
    }
}
