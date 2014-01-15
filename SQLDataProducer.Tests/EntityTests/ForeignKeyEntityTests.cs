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
using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using SQLDataProducer.Entities.DatabaseEntities;


namespace SQLDataProducer.Tests.Entities
{
    [TestFixture]
    [MSTest.TestClass]
    public class ForeignKeyEntityTests : TestBase
    {
        [Test]
        [MSTest.TestMethod]
        public void ShouldCloneForeignKeyEntity()
        {
            ForeignKeyEntity foreignKey = new ForeignKeyEntity();
            foreignKey.ReferencingColumn = "CustomerId";
            foreignKey.ReferencingTable = new TableEntity("dbo", "Customer");
            foreignKey.Keys.Add("Peter");
            foreignKey.Keys.Add("Henell");

            var cloned = foreignKey.Clone();

            Assert.That(cloned.ReferencingColumn, Is.EqualTo(foreignKey.ReferencingColumn));
            Assert.That(cloned.ReferencingTable, Is.EqualTo(foreignKey.ReferencingTable));
            CollectionAssert.AreEqual(cloned.Keys, foreignKey.Keys);
        }
    }
}
