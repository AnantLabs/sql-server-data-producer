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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Data;
using SQLDataProducer.Entities.DatabaseEntities;


namespace SQLDataProducer.RandomTests
{
    [TestFixture]
    public class ColumnEntityTests : TestBase
    {
        ColumnEntity col = new ColumnEntity("Identity",
                new ColumnDataTypeDefinition("int", false), true, 1, false, string.Empty, null);

        [Test]
        public void DefaultTest()
        {
            Assert.IsTrue(col.IsIdentity);
            Assert.AreEqual(false, col.IsNotIdentity);
            Assert.AreEqual(1, col.OrdinalPosition);
            Assert.AreEqual(false, col.ColumnDataType.IsNullable);
            Assert.AreEqual("", col.Constraints);
            Assert.AreEqual(false, col.HasConstraints);
            Assert.AreEqual(false, col.IsForeignKey);
            Assert.AreEqual(null, col.ForeignKey);
            Assert.AreEqual(5, col.PossibleGenerators.Count);

            
            Assert.AreEqual(SQLDataProducer.Entities.Generators.Generator.GENERATOR_CountingUpInteger, col.Generator.GeneratorName);
        }

        [Test]
        public void ShouldGetGeneratedValue()
        {
            var i = col.GenerateValue(100);
            Assert.IsNotNull(i);
        }

        [Test]
        public void ShouldStorePreviouslyGeneratedValue()
        {
            col.GenerateValue(100);
            Assert.IsNotNull(col.PreviouslyGeneratedValue);
        }
    }
}
