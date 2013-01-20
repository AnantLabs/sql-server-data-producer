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
using SQLDataProducer.Entities.DatabaseEntities.Factories;
using SQLDataProducer.Entities.DatabaseEntities;


namespace SQLDataProducer.RandomTests
{
    [TestFixture]
    public class DatabaseEntityFactoryTests : TestBase
    {
        [Test]
        public void ShouldBeAbleToCreateColumnEntityFromFactory()
        {
            var c1 = DatabaseEntityFactory.CreateColumnEntity("id", new ColumnDataTypeDefinition("int", false), true, 1, false, string.Empty, null);
            Assert.AreEqual("id", c1.ColumnName);
            Assert.AreEqual("int", c1.ColumnDataType.Raw);
            Assert.AreEqual(SqlDbType.Int, c1.ColumnDataType.DBType);
            Assert.AreEqual(true, c1.IsIdentity);
            Assert.AreEqual(false, c1.ColumnDataType.IsNullable);
            Assert.AreEqual(1, c1.OrdinalPosition);


            var c2 = DatabaseEntityFactory.CreateColumnEntity("name", new ColumnDataTypeDefinition("nvarchar(988)", true), false, 2, false, string.Empty, null);
            Assert.AreEqual("name", c2.ColumnName);
            Assert.AreEqual("nvarchar(988)", c2.ColumnDataType.Raw);
            Assert.AreEqual(SqlDbType.NVarChar, c2.ColumnDataType.DBType);
            Assert.AreEqual(false, c2.IsIdentity);
            Assert.AreEqual(true, c2.ColumnDataType.IsNullable);
            Assert.AreEqual(2, c2.OrdinalPosition);
        }
    }
}
