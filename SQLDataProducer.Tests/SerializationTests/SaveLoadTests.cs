// Copyright 2012 Peter Henell

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
using SQLDataProducer.RandomTests.Helpers;
using SQLDataProducer.Entities.ExecutionEntities;
using System.Xml;

namespace SQLDataProducer.RandomTests.SerializationTests
{
    public class SaveLoadTests : TestBase
    {

        public SaveLoadTests()
            :base()
        {
        }

        [Test]
        public void ShouldBeAbleToSerializeAndDeserializeExecutionItemCollection()
        {
            var execItems = ExecutionItemHelper.GetExecutionItemCollection();
            System.Xml.Serialization.XmlSerializer seri = new System.Xml.Serialization.XmlSerializer(typeof(ExecutionItemCollection));
            using (var writer = XmlWriter.Create(@"c:\temp\repeater\saved.xml"))
            {
                seri.Serialize(writer, execItems);
            }
            using (var reader = XmlReader.Create(@"c:\temp\repeater\saved.xml"))
            {
                ExecutionItemCollection loadedList =
                    seri.Deserialize(reader) as ExecutionItemCollection;

                Assert.IsNotNull(loadedList);
                Assert.AreEqual(execItems.Count, loadedList.Count);
                Assert.AreEqual(execItems.IsContainingData, loadedList.IsContainingData);

                foreach (var item in execItems)
                {
                    var loadedItem = loadedList.Where(x => x.TargetTable.ToString() == item.TargetTable.ToString() && x.Order == item.Order).First(); // TODO replace with table == table when equals is implemented or executionItem == executionItem is implemented.
                    Assert.IsNotNull(loadedItem, string.Format("expected {0} to be loaded but was not found in loaded collection", item.TargetTable.ToString()));
                    Assert.IsTrue(item == loadedItem);
                }
            }
        }

        
        [Test]
        public void ShouldBeAbleToSaveAndLoadExecutionItemCollectionUsingManager()
        {
            Assert.AreEqual("implemented", "not implemented");
        }
    }
}
