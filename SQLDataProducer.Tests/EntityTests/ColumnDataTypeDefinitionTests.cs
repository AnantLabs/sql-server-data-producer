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
using SQLDataProducer.Entities;


namespace SQLDataProducer.Tests.Entities
{
    [TestFixture]
    [MSTest.TestClass]
    public class ColumnDataTypeDefinitionTests : TestBase
    {
        [Test]
        [MSTest.TestMethod]
        public void ShouldGetColumnDataTypeDefinitionForType()
        {
            AssertNotNullDataType("BigInt");
            AssertNotNullDataType("Binary");
            AssertNotNullDataType("Bit");
            AssertNotNullDataType("Char");
            AssertNotNullDataType("DateTime");
            AssertNotNullDataType("Decimal");
            AssertNotNullDataType("Float");
            AssertNotNullDataType("Image");
            AssertNotNullDataType("Int");
            AssertNotNullDataType("Money");
            AssertNotNullDataType("NChar");
            AssertNotNullDataType("NText");
            AssertNotNullDataType("NVarChar");
            AssertNotNullDataType("Real");
            AssertNotNullDataType("UniqueIdentifier");
            AssertNotNullDataType("SmallDateTime");
            AssertNotNullDataType("SmallInt");
            AssertNotNullDataType("SmallMoney");
            AssertNotNullDataType("Text");
            AssertNotNullDataType("Timestamp");
            AssertNotNullDataType("TinyInt");
            AssertNotNullDataType("VarBinary");
            AssertNotNullDataType("VarChar");
            AssertNotNullDataType("Variant");
            AssertNotNullDataType("Xml");
            AssertNotNullDataType("Udt");
            AssertNotNullDataType("Structured");
            AssertNotNullDataType("Date");
            AssertNotNullDataType("Time");
            AssertNotNullDataType("DateTime2");
            AssertNotNullDataType("DateTimeOffset");
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldFallbackToVarcharWithDefaultLengthIfUnkownDatatype()
        {
            // TODO: Consider this requirement. Should we throw exception instead? Instead of having it fail when inserting values.
            AssertRawStringIsTurnedIntoSqlDataType("Boat", SqlDbType.VarChar);
            AssertLengthPropertyIsEqualTo("Boat", ColumnDataTypeDefinition.STRING_DEFAULT_LENGTH);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldSetLengthToDEFAULTIfUnableToParseLength()
        {
            // TODO: Consider this requirement. Should we throw exception instead? Instead of having it fail when inserting values.
            AssertRawStringIsTurnedIntoSqlDataType("varchar(9999999999999999)", SqlDbType.VarChar);
            AssertLengthPropertyIsEqualTo("varchar(9999999999999999)", ColumnDataTypeDefinition.STRING_DEFAULT_LENGTH);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldSetLengthToDefaultValueIfUnableToParseDatatype()
        {
            // TODO: Consider this requirement. Should we throw exception instead? Instead of having it fail when inserting values.
            AssertRawStringIsTurnedIntoSqlDataType("varchar(boat)", SqlDbType.VarChar);
            AssertLengthPropertyIsEqualTo("varchar(boat)", ColumnDataTypeDefinition.STRING_DEFAULT_LENGTH);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGetMinAndMaxFromDecimal()
        {
            var a = new ColumnDataTypeDefinition("Decimal(3, 1)", false);
            Assert.That(a.MaxValue, Is.EqualTo(99.9));
            AssertMinAndMaxValue("Decimal(3, 1)", -99.9m, 99.9m);
            AssertMinAndMaxValue("Decimal(1, 1)", -0.9m, 0.9m);
            AssertMinAndMaxValue("Decimal(4, 2)", -99.99m, 99.99m);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldUseDefaultValuesWhenInvalidInputsInDecimalDatatype()
        {
            var a = new ColumnDataTypeDefinition("Decimal(0, 1)", false);
            Assert.That(a.Scale, Is.EqualTo(ColumnDataTypeDefinition.DECIMAL_DEFAULT_SCALE));
            Assert.That(a.MaxLength, Is.EqualTo(ColumnDataTypeDefinition.DECIMAL_DEFAULT_LENGTH));
        }
        

        [Test]
        [MSTest.TestMethod]
        public void ShouldHaveMinAndMaxValuesForIntegerValues()
        {
            AssertMinAndMaxValue("int", int.MinValue, int.MaxValue);
            AssertMinAndMaxValue("smallint", short.MinValue, short.MaxValue);
            AssertMinAndMaxValue("bigint", long.MinValue, long.MaxValue);
            AssertMinAndMaxValue("tinyint", byte.MinValue, byte.MaxValue);
            AssertMinAndMaxValue("bit", 0, 1);
        }

        private void AssertMinAndMaxValue(string dataType, decimal min, decimal max)
        {
            var a = new ColumnDataTypeDefinition(dataType, false);
            Assert.That(a.MinValue, Is.EqualTo(min), dataType);
            Assert.That(a.MaxValue, Is.EqualTo(max), dataType);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldHaveMinAndMaxValuesForFloatingTypesAndOther()
        {
            AssertMinAndMaxValue("Float", decimal.MinValue, decimal.MaxValue);
            AssertMinAndMaxValue("Money", -9223372036854775808.999m, 9223372036854775807.999m);
            AssertMinAndMaxValue("Real", decimal.MinValue, decimal.MaxValue);
            AssertMinAndMaxValue("SmallMoney", -214748.3648m, 214748.3647m);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldSetCorrectSqlDataTypeForRawDataTypeString()
        {
            AssertRawStringIsTurnedIntoSqlDataType("BigInt", SqlDbType.BigInt);
            AssertRawStringIsTurnedIntoSqlDataType("Binary", SqlDbType.Binary);
            AssertRawStringIsTurnedIntoSqlDataType("Bit", SqlDbType.Bit);
            AssertRawStringIsTurnedIntoSqlDataType("Char", SqlDbType.Char);
            AssertRawStringIsTurnedIntoSqlDataType("DateTime", SqlDbType.DateTime);
            AssertRawStringIsTurnedIntoSqlDataType("Decimal", SqlDbType.Decimal);
            AssertRawStringIsTurnedIntoSqlDataType("Float", SqlDbType.Float);
            AssertRawStringIsTurnedIntoSqlDataType("Image", SqlDbType.Image);
            AssertRawStringIsTurnedIntoSqlDataType("Int", SqlDbType.Int);
            AssertRawStringIsTurnedIntoSqlDataType("Money", SqlDbType.Money);
            AssertRawStringIsTurnedIntoSqlDataType("NChar", SqlDbType.NChar);
            AssertRawStringIsTurnedIntoSqlDataType("NText", SqlDbType.NText);
            AssertRawStringIsTurnedIntoSqlDataType("NVarChar", SqlDbType.NVarChar);
            AssertRawStringIsTurnedIntoSqlDataType("Real", SqlDbType.Real);
            AssertRawStringIsTurnedIntoSqlDataType("UniqueIdentifier", SqlDbType.UniqueIdentifier);
            AssertRawStringIsTurnedIntoSqlDataType("SmallDateTime", SqlDbType.SmallDateTime);
            AssertRawStringIsTurnedIntoSqlDataType("SmallInt", SqlDbType.SmallInt);
            AssertRawStringIsTurnedIntoSqlDataType("SmallMoney", SqlDbType.SmallMoney);
            AssertRawStringIsTurnedIntoSqlDataType("Text", SqlDbType.Text);
            AssertRawStringIsTurnedIntoSqlDataType("Timestamp", SqlDbType.Timestamp);
            AssertRawStringIsTurnedIntoSqlDataType("TinyInt", SqlDbType.TinyInt);
            AssertRawStringIsTurnedIntoSqlDataType("VarBinary", SqlDbType.VarBinary);
            AssertRawStringIsTurnedIntoSqlDataType("VarChar", SqlDbType.VarChar);
            AssertRawStringIsTurnedIntoSqlDataType("Variant", SqlDbType.Variant);
            AssertRawStringIsTurnedIntoSqlDataType("Xml", SqlDbType.Xml);
            AssertRawStringIsTurnedIntoSqlDataType("Udt", SqlDbType.Udt);
            AssertRawStringIsTurnedIntoSqlDataType("Structured", SqlDbType.Structured);
            AssertRawStringIsTurnedIntoSqlDataType("Date", SqlDbType.Date);
            AssertRawStringIsTurnedIntoSqlDataType("Time", SqlDbType.Time);
            AssertRawStringIsTurnedIntoSqlDataType("DateTime2", SqlDbType.DateTime2);
            AssertRawStringIsTurnedIntoSqlDataType("DateTimeOffset", SqlDbType.DateTimeOffset);

            AssertRawStringIsTurnedIntoSqlDataType("NChar(123)", SqlDbType.NChar);
            AssertRawStringIsTurnedIntoSqlDataType("NVarChar(123)", SqlDbType.NVarChar);
            AssertRawStringIsTurnedIntoSqlDataType("VarBinary(123)", SqlDbType.VarBinary);
            AssertRawStringIsTurnedIntoSqlDataType("VarChar(123)", SqlDbType.VarChar);
            AssertRawStringIsTurnedIntoSqlDataType("Binary(123)", SqlDbType.Binary);

            AssertRawStringIsTurnedIntoSqlDataType("Decimal(12, 1)", SqlDbType.Decimal);
            AssertRawStringIsTurnedIntoSqlDataType("Decimal(12,1)", SqlDbType.Decimal);
            AssertRawStringIsTurnedIntoSqlDataType("Decimal(12)", SqlDbType.Decimal);

            AssertRawStringIsTurnedIntoSqlDataType("DateTime2(2)", SqlDbType.DateTime2);
        }

        private void AssertRawStringIsTurnedIntoSqlDataType(string dataType, SqlDbType sqlDbType)
        {
            {
                var a = new ColumnDataTypeDefinition(dataType, false);
                Assert.That(a, Is.Not.Null, dataType);
                Assert.That(a.DBType, Is.EqualTo(sqlDbType), dataType);
            }
            {
                // Also test nullable
                var a = new ColumnDataTypeDefinition(dataType, true);
                Assert.That(a, Is.Not.Null, dataType);
                Assert.That(a.DBType, Is.EqualTo(sqlDbType), dataType);
            }
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldSetLengthPropertyOfDataTypesWithLength()
        {
            // should set default length if not provided
            AssertLengthPropertyIsEqualTo("Char", ColumnDataTypeDefinition.STRING_DEFAULT_LENGTH);
            AssertLengthPropertyIsEqualTo("NChar", ColumnDataTypeDefinition.STRING_DEFAULT_LENGTH);
            AssertLengthPropertyIsEqualTo("NVarChar", ColumnDataTypeDefinition.STRING_DEFAULT_LENGTH);
            AssertLengthPropertyIsEqualTo("VarChar", ColumnDataTypeDefinition.STRING_DEFAULT_LENGTH);

            AssertLengthPropertyIsEqualTo("Char(155)", 155);
            AssertLengthPropertyIsEqualTo("NChar(155)", 155);
            AssertLengthPropertyIsEqualTo("NVarChar(155)", 155);
            AssertLengthPropertyIsEqualTo("VarChar(155)", 155);

            AssertLengthPropertyIsEqualTo("decimal(13)", 13);
            AssertLengthPropertyIsEqualTo("decimal(13, 1)", 13);
            AssertLengthPropertyIsEqualTo("decimal(13,1)", 13);
            AssertLengthPropertyIsEqualTo("decimal(13,11)", 13);

           
        }
        [Test]
        [MSTest.TestMethod]
        public void ShouldNotBeSensitiveToCase()
        {
            AssertLengthPropertyIsEqualTo("decimal(13)", 13);
            AssertLengthPropertyIsEqualTo("decimal(13, 1)", 13);
            AssertLengthPropertyIsEqualTo("decimal(13,1)", 13);
            AssertLengthPropertyIsEqualTo("decimal(13,11)", 13);

            AssertLengthPropertyIsEqualTo("Decimal(13)", 13);
            AssertLengthPropertyIsEqualTo("Decimal(13, 1)", 13);
            AssertLengthPropertyIsEqualTo("Decimal(13,1)", 13);
            AssertLengthPropertyIsEqualTo("Decimal(13,11)", 13);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldSetScalePropertyOfDataTypesWithScale()
        {
            AssertLengthPropertyIsEqualTo("decimal(20, 1)", 20);
            AssertScalePropertyIsEqualTo("decimal(20, 1)", 1);
            AssertScalePropertyIsEqualTo("decimal(20, 11)", 11);
            AssertScalePropertyIsEqualTo("decimal(20,11)", 11);
            AssertScalePropertyIsEqualTo("decimal(20)", ColumnDataTypeDefinition.DECIMAL_DEFAULT_SCALE);
            AssertScalePropertyIsEqualTo("decimal", ColumnDataTypeDefinition.DECIMAL_DEFAULT_SCALE);
        }

        private void AssertScalePropertyIsEqualTo(string dataType, int expectedScale)
        {
            var a = new ColumnDataTypeDefinition(dataType, false);
            Assert.That(a, Is.Not.Null, dataType);
            Assert.That(a.Scale, Is.EqualTo(expectedScale), dataType);
        }

        private void AssertLengthPropertyIsEqualTo(string dataType, int expectedLength)
        {
            var a = new ColumnDataTypeDefinition(dataType, false);
            Assert.That(a, Is.Not.Null, dataType);
            Assert.That(a.MaxLength, Is.EqualTo(expectedLength), dataType);
        }

        private static void AssertNotNullDataType(string dataType)
        {
            var a = new ColumnDataTypeDefinition(dataType, false);
            Assert.That(a, Is.Not.Null, dataType);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldCloneColumnDataTypeDefinition()
        {
            var a = new ColumnDataTypeDefinition("decimal(20)", false);
            ColumnDataTypeDefinition clone = a.Clone();
            Assert.That(a.DBType, Is.EqualTo(clone.DBType));
            Assert.That(a.IsNullable, Is.EqualTo(clone.IsNullable));
            Assert.That(a.MaxLength, Is.EqualTo(clone.MaxLength));
            Assert.That(a.MaxValue, Is.EqualTo(clone.MaxValue));
            Assert.That(a.MinValue, Is.EqualTo(clone.MinValue));
            Assert.That(a.Raw, Is.EqualTo(clone.Raw));
            Assert.That(a.Scale, Is.EqualTo(clone.Scale));
            Assert.That(a.StringFormatter, Is.EqualTo(clone.StringFormatter));
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldAssertThatNoSettableFieldsAreAdded()
        {
            var allProperties = typeof(ColumnDataTypeDefinition).GetProperties();

            Assert.That(allProperties.All(x => x.GetSetMethod() == null));

            Assert.That(allProperties.Count(), Is.EqualTo(8));
        }

        //[Test]
        //[MSTest.TestMethod]
        //public void ShouldCacheDataTypeDefinitionsBasedOnRaw()
        //{
        //    ColumnDataTypeDefinitionCache typeCache = new ColumnDataTypeDefinitionCache();
        //}
    }
}

