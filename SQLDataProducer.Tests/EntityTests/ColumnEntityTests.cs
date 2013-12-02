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
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DatabaseEntities.Factories;
using Generators = SQLDataProducer.Entities.Generators;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;
using SQLDataProducer.Entities.Generators;
using SQLDataProducer.Entities.Generators.IntGenerators;



namespace SQLDataProducer.Tests.EntitiesTests
{
    [TestFixture]
    [MSTest.TestClass]
    public class ColumnEntityTests : TestBase
    {
        ColumnEntity identityCol = DatabaseEntityFactory.CreateColumnEntity("identity", new ColumnDataTypeDefinition("int", false), true, 1, false, string.Empty, null);
        ColumnEntity dateCol = DatabaseEntityFactory.CreateColumnEntity("created", new ColumnDataTypeDefinition("datetime", false), false, 2, false, string.Empty, null);

        [Test]
        [MSTest.TestMethod]
        public void ShouldAssertDefaultPropertiesForTestColumn()
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


            Assert.AreEqual(SQLDataProducer.Entities.Generators.IntGenerators.IdentityIntGenerator.GENERATOR_NAME, identityCol.Generator.GeneratorName);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGetGeneratedValue()
        {
            var i = identityCol.GenerateValue(100);
            Assert.IsNotNull(i);
            var j = dateCol.GenerateValue(1);
            Assert.That(j, Is.Not.Null);
        }

        [Test]
        [MSTest.TestMethod]
        public void shouldProduceSameColumnIdentityEveryTime()
        {
            var colId = identityCol.ColumnIdentity;
            var colId2 = identityCol.ColumnIdentity;
            Assert.That(colId, Is.EqualTo(colId2));
            for (int i = 0; i < 100; i++)
            {
                Assert.That(colId, Is.EqualTo(identityCol.ColumnIdentity));
            }

        }

        [Test]
        [MSTest.TestMethod]
        public void shouldProduceDifferentColumnIdentitiesFromEachThread()
        {
            // Make both the same
            Guid original = Guid.NewGuid();
            Guid a1 = original;
            Guid a2 = original;

            Action action1 = new Action(() =>
            {
                a1 = identityCol.ColumnIdentity;
            });
            Action action2 = new Action(() =>
            {
                a2 = identityCol.ColumnIdentity;
            });

            Thread t1 = new Thread(new ThreadStart(action1));
            Thread t2 = new Thread(new ThreadStart(action2));

            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();

            Assert.That(a1, Is.Not.EqualTo(a2));
            Assert.That(a1, Is.Not.EqualTo(original));
            Assert.That(a1, Is.Not.EqualTo(original));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldPickForeignKeyTableAsReferenceWhenItExistInTheListOfExecutionItems()
        {
            Assert.Fail("Adding table that is referencing another table's identity value should automatically set the generator to value from other column and point it to the other identity column");
        }
      
    }
}
