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
using SQLDataProducer.Entities.DatabaseEntities.Factories;


namespace SQLDataProducer.RandomTests
{
    [TestFixture]
    public class ColumnEntityTests : TestBase
    {
        ColumnEntity identityCol = DatabaseEntityFactory.CreateColumnEntity("identity", new ColumnDataTypeDefinition("int", false), true, 1, false, string.Empty, null);
        ColumnEntity dateCol = DatabaseEntityFactory.CreateColumnEntity("created", new ColumnDataTypeDefinition("datetime", false), false, 2, false, string.Empty, null);

        [Test]
        public void DefaultTest()
        {
            Assert.IsTrue(identityCol.IsIdentity);
            Assert.AreEqual(false, identityCol.IsNotIdentity);
            Assert.AreEqual(1, identityCol.OrdinalPosition);
            Assert.AreEqual(false, identityCol.ColumnDataType.IsNullable);
            Assert.AreEqual("", identityCol.Constraints);
            Assert.AreEqual(false, identityCol.HasConstraints);
            Assert.AreEqual(false, identityCol.IsForeignKey);
            Assert.AreEqual(null, identityCol.ForeignKey);
            Assert.AreEqual(11, identityCol.PossibleGenerators.Count);

            
            Assert.AreEqual(SQLDataProducer.Entities.Generators.Generator.GENERATOR_IdentityFromSqlServerGenerator, identityCol.Generator.GeneratorName);
        }

        [Test]
        public void ShouldGetGeneratedValue()
        {
            var i = identityCol.GenerateValue(100);
            Assert.IsNotNull(i);
            var j = dateCol.GenerateValue(1);
            Assert.That(j, Is.Not.Null);
        }

        [Test]
        public void ShouldStorePreviouslyGeneratedValue()
        {
            identityCol.GenerateValue(100);
            Assert.IsNotNull(identityCol.PreviouslyGeneratedValue);
            dateCol.GenerateValue(100);
            Assert.IsNotNull(dateCol.PreviouslyGeneratedValue);
        }
    }
}
