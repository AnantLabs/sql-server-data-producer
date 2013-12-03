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
using SQLDataProducer.Entities.Generators;

namespace SQLDataProducer.Tests.ValueGenerators
{
    [TestFixture]
    [MSTest.TestClass]
    public class GeneratorHelpTextManagerTest
    {

        [Test]
        [MSTest.TestMethod]
        public void ShouldFindGeneratorHelpTextFile()
        {
            System.IO.File.Exists(GeneratorHelpTextManager.HELPTEXT_FILENAME);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGetEmptyStringWhenGeneratorNameNotFound()
        {
            string text = GeneratorHelpTextManager.GetGeneratorHelpText("does not exist");
            Assert.That(text, Is.Empty);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldLoadHelpTextForEachGenerator()
        {
            foreach (string dataType in GeneratorFactoryTest.supportedDatatypes)
            {
                var gens = GeneratorFactory.GetAllGeneratorsForType(new ColumnDataTypeDefinition(dataType, true));
                Assert.That(gens.Count, Is.GreaterThan(0), dataType);

                foreach (var gen in gens)
                {
                    string text = GeneratorHelpTextManager.GetGeneratorHelpText(gen.GeneratorName);
                    Assert.That(text, Is.Not.Empty, gen.GeneratorName);
                    System.Console.WriteLine(text);
                }
            }
        }

      
    }
}
