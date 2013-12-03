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
using SQLDataProducer.Entities;
using SQLDataProducer.Entities.DataEntities;
using SQLDataProducer.Entities.Generators;
using System.Collections.ObjectModel;
using SQLDataProducer.Entities.Generators.DecimalGenerators;

namespace SQLDataProducer.Tests
{
    [TestFixture]
    [MSTest.TestClass]
    public class GeneratorFactoryTest : TestBase
    {
        public static List<string> supportedDatatypes = new List<string>();

        static GeneratorFactoryTest()
        {
            supportedDatatypes.Add("BigInt");
            supportedDatatypes.Add("Binary");
            supportedDatatypes.Add("Bit");
            supportedDatatypes.Add("Char");
            supportedDatatypes.Add("DateTime");
            supportedDatatypes.Add("Decimal");
            supportedDatatypes.Add("Float");
            supportedDatatypes.Add("Image");
            supportedDatatypes.Add("Int");
            supportedDatatypes.Add("Money");
            supportedDatatypes.Add("NChar");
            supportedDatatypes.Add("NText");
            supportedDatatypes.Add("NVarChar");
            supportedDatatypes.Add("Real");
            supportedDatatypes.Add("UniqueIdentifier");
            supportedDatatypes.Add("SmallDateTime");
            supportedDatatypes.Add("SmallInt");
            supportedDatatypes.Add("SmallMoney");
            supportedDatatypes.Add("Text");
            supportedDatatypes.Add("Timestamp");
            supportedDatatypes.Add("TinyInt");
            supportedDatatypes.Add("VarBinary");
            supportedDatatypes.Add("VarChar");
            supportedDatatypes.Add("Variant");
            supportedDatatypes.Add("Xml");
            // not supported
            //dataTypes.Add("Udt");
            //dataTypes.Add("Structured");
            supportedDatatypes.Add("Date");
            supportedDatatypes.Add("Time");
            supportedDatatypes.Add("DateTime2");
            supportedDatatypes.Add("DateTimeOffset");

            supportedDatatypes.Add("NChar(123)");
            supportedDatatypes.Add("NVarChar(123)");
            supportedDatatypes.Add("VarBinary(123)");
            supportedDatatypes.Add("VarChar(123)");
            supportedDatatypes.Add("Binary(123)");

            supportedDatatypes.Add("Decimal(12,10)");
            supportedDatatypes.Add("Decimal(12,1)");
            supportedDatatypes.Add("Decimal(12)");

            supportedDatatypes.Add("DateTime2(2)");
        }

        public GeneratorFactoryTest()
            : base()
        {
            
        }
        
        [Test]
        [MSTest.TestMethod]
        public void GeneratorsShouldHaveHelpTexts()
        {
            foreach (string dataType in supportedDatatypes)
            {
                var gens = GeneratorFactory.GetAllGeneratorsForType(new ColumnDataTypeDefinition(dataType, true));
                Assert.That(gens.Count, Is.GreaterThan(0), dataType);

                foreach (var gen in gens)
                {
                    //Console.WriteLine(gen.GeneratorHelpText);
                    Assert.IsNotNullOrEmpty(gen.GeneratorHelpText, string.Format("{0} does not have helptext", gen.GeneratorName));
                }
            }
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGetAllGenerators()
        {
            foreach (string dataType in supportedDatatypes)
            {
                var gens = GeneratorFactory.GetAllGeneratorsForType(new ColumnDataTypeDefinition(dataType, true));
                Assert.That(gens.Count, Is.GreaterThan(0), dataType);
               
                foreach (var gen in gens)
                {
                    try
                    {
                        Assert.That(gen.GeneratorName, Is.Not.Empty, dataType);
                        Assert.That(gen.GeneratorName, Is.Not.Null, dataType);
                        Assert.That(gen.GenerateValue(1), Is.Not.Null, dataType);
                    }
                    catch (Exception e)
                    {
                        Assert.Fail(string.Format("ERROR Generating value for [{0}], generator: [{1}], message : {2}", dataType, gen.GeneratorName ,e.ToString()));
                    }
                }
            }
        }

//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldOutputTestsForEachType()
//        {
//            string testClassTemplate = @"
//using NUnit.Framework;
//using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
//using SQLDataProducer.Entities.DatabaseEntities;
//using {0};
//
//namespace SQLDataProducer.Tests.ValueGenerators
//{{
//    [TestFixture]
//    [MSTest.TestClass]
//    public class {1}Test
//    {{
//        public {1}Test()
//        {{
//        }}
//
//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldGenerateValue()
//        {{
//            var gen = new {1}(new ColumnDataTypeDefinition(""{2}"", false));
//            var firstValue = gen.GenerateValue(1);
//            Assert.That(firstValue, Is.Not.Null);
//        }}
//
//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldTestStep()
//        {{
//            Assert.Fail(""not implemented"");
//        }}
//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldTestStartValue()
//        {{
//            Assert.Fail(""not implemented"");
//        }}
//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldTestOverFlow()
//        {{
//            Assert.Fail(""not implemented"");
//        }}
//    }}
//}}";


//            foreach (string dataType in supportedDatatypes)
//            {
//                var gens = GeneratorFactory.GetAllGeneratorsForType(new ColumnDataTypeDefinition(dataType, true));
//                Console.WriteLine();
//                Console.WriteLine(dataType);
//                foreach (var g in gens)
//                {
//                    string testClass = string.Format(testClassTemplate, g.GetType().Namespace, g.GetType().Name, dataType);
//                    Console.WriteLine(testClass);
//                    System.IO.Directory.CreateDirectory("c:\\temp\\" + g.GetType().Namespace.Substring(g.GetType().Namespace.LastIndexOf('.') + 1, g.GetType().Namespace.Length - g.GetType().Namespace.LastIndexOf('.') - 1));
//                    System.IO.File.WriteAllText("c:\\temp\\" + g.GetType().Namespace.Substring(g.GetType().Namespace.LastIndexOf('.') + 1 , g.GetType().Namespace.Length - g.GetType().Namespace.LastIndexOf('.') - 1 ) 
//                        + "\\" + g.GetType().Name + "Test.cs", testClass);
//                }
                
//            }
//            Assert.Fail();
//        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGetGeneratorsForDatatype()
        {
            int expectedStringGenerators = 5;
            int expectedDateTimeGenerators = 9;
            int expectedDecimalGenerators = 2;

            ExpectMoreNullableThanRegular("BigInt", expectedDateTimeGenerators);
            ExpectMoreNullableThanRegular("Bit", 0);
            ExpectMoreNullableThanRegular("Char", expectedStringGenerators);
            ExpectMoreNullableThanRegular("Date", expectedDateTimeGenerators);
            ExpectMoreNullableThanRegular("DateTime", expectedDateTimeGenerators);
            ExpectMoreNullableThanRegular("DateTime2", expectedDateTimeGenerators);
            ExpectMoreNullableThanRegular("SmallDateTime", expectedDateTimeGenerators);
            ExpectMoreNullableThanRegular("Time", expectedDateTimeGenerators);
            ExpectMoreNullableThanRegular("DateTimeOffset", expectedDateTimeGenerators);
            ExpectMoreNullableThanRegular("Decimal", expectedDecimalGenerators);
            ExpectMoreNullableThanRegular("Float", expectedDecimalGenerators);
            ExpectMoreNullableThanRegular("Real", expectedDecimalGenerators);
            ExpectMoreNullableThanRegular("Money", expectedDecimalGenerators);
            ExpectMoreNullableThanRegular("SmallMoney", expectedDecimalGenerators);
            ExpectMoreNullableThanRegular("Binary", 0);
            ExpectMoreNullableThanRegular("Image", 0);
            ExpectMoreNullableThanRegular("Timestamp", 0);
            ExpectMoreNullableThanRegular("VarBinary(10)", 0);
            ExpectMoreNullableThanRegular("int", expectedDateTimeGenerators);
            ExpectMoreNullableThanRegular("NChar(10)", expectedStringGenerators);
            ExpectMoreNullableThanRegular("NText", expectedStringGenerators);
            ExpectMoreNullableThanRegular("NVarChar(10)", expectedStringGenerators);
            ExpectMoreNullableThanRegular("Text", expectedStringGenerators);
            ExpectMoreNullableThanRegular("VarChar(100)", expectedStringGenerators);
            ExpectMoreNullableThanRegular("VarChar(max)", expectedStringGenerators);
            ExpectMoreNullableThanRegular("SmallInt", expectedDateTimeGenerators);
            ExpectMoreNullableThanRegular("TinyInt", expectedDateTimeGenerators);
            ExpectMoreNullableThanRegular("UniqueIdentifier", 0);
            ExpectMoreNullableThanRegular("Variant", expectedStringGenerators);
            ExpectMoreNullableThanRegular("Xml", 0);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGetExceptionWhenTryingToGetGeneratorForNonNullableDatatypeThatIsNotImplemented()
        {
            ExeptionForUnsupportedDatatype("udt");
            ExeptionForUnsupportedDatatype("Structured");
        }

        private void ExeptionForUnsupportedDatatype(string datatype)
        {
            bool exeptionHappened = false;
            try
            {
                var gens = GeneratorFactory.GetAllGeneratorsForType(new ColumnDataTypeDefinition(datatype, false));
            }
            catch (Exception)
            {
                exeptionHappened = true;
            }
            Assert.That(exeptionHappened);
        }

        private void ExpectMoreNullableThanRegular(string datatype, int expectedGens)
        {
            var gens = GeneratorFactory.GetAllGeneratorsForType(new ColumnDataTypeDefinition(datatype, false));
            Assert.That(gens.Count, Is.GreaterThanOrEqualTo(expectedGens), datatype);
            var nullable = GeneratorFactory.GetAllGeneratorsForType(new ColumnDataTypeDefinition(datatype, true));
            Assert.That(nullable.Count, Is.GreaterThan(gens.Count), datatype);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldCreateInstanceOfDecimalUpCounter()
        {
            var dt = new ColumnDataTypeDefinition("decimal(19, 6)", false);
            var gen = GeneratorFactory.CreateInstance(typeof(CountingUpDecimalGenerator), dt);
            Assert.That(gen, Is.Not.Null);
            var value = gen.GenerateValue(1);
            Assert.That(value, Is.Not.Null);
        }
        
    }
}
