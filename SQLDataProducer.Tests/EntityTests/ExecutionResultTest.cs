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
using SQLDataProducer.Entities.ExecutionEntities;
using System;
using System.Linq;
using SQLDataProducer.Entities.DatabaseEntities.Collections;

namespace SQLDataProducer.Tests.EntityTests
{
    [TestFixture]
    [MSTest.TestClass]
    public class ExecutionResultTest
    {
        public ExecutionResultTest()
        {
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldInstanciateExecutionResult()
        {
            DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now;
            long insertCount = 789;

            ExecutionResult result = new ExecutionResult(startTime, endTime, insertCount, new ErrorList());
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StartTime, Is.EqualTo(startTime));
            Assert.That(result.EndTime, Is.EqualTo(endTime));
            Assert.That(result.Duration, Is.EqualTo(endTime - startTime));
            Assert.That(result.InsertCount, Is.EqualTo(insertCount));
            
            Assert.That(result.Errors, Is.Not.Null);
            Assert.That(result.Errors.Count, Is.EqualTo(0));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldAssertThatNoSettableFieldsAreAdded()
        {
            var allProperties = typeof(ExecutionResult).GetProperties(
                  System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Instance);

            Assert.That(allProperties.All(x => x.GetSetMethod() == null));

            Assert.That(allProperties.Count(), Is.EqualTo(5));
        }
    }
}
