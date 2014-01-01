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
    }
}
