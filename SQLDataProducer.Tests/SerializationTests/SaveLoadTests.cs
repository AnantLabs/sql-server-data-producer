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
using SQLDataProducer.DataAccess.Factories;

namespace SQLDataProducer.RandomTests.SerializationTests
{
    public class SaveLoadTests : TestBase
    {

        public SaveLoadTests()
            :base()
        {
        }

        [Test]
        public void ShouldBeAbleToSaveAndLoadExecutionItemCollectionUsingManager()
        {
            var execItems = ExecutionItemHelper.GetRealExecutionItemCollection(Connection());
            var fileName = @"c:\temp\repeater\saved.xml";

            // Sanity check
            foreach (var item in execItems)
            {
                Assert.IsTrue(item.Equals(item));
            }

            var tda = new DataAccess.TableEntityDataAccess(Connection());
            ExecutionItemManager.Save(execItems, fileName);

            var loadedList = ExecutionItemManager.Load(fileName, tda);

            

            Assert.IsNotNull(loadedList);
            Assert.AreEqual(execItems.Count, loadedList.Count);
            Assert.AreEqual(execItems.IsContainingData, loadedList.IsContainingData);

            foreach (var item in execItems)
            {
                ExecutionItem loadedItem = loadedList.Where(x => x.TargetTable.Equals(item.TargetTable) && x.Order == item.Order).First();
                Assert.IsNotNull(loadedItem, string.Format("expected {0} to be loaded but was not found in loaded collection", item.TargetTable.ToString()));
                

                Assert.IsTrue(item.Description == loadedItem.Description);
                Assert.IsTrue(item.ExecutionCondition == loadedItem.ExecutionCondition);
                Assert.IsTrue(item.ExecutionConditionValue == loadedItem.ExecutionConditionValue);
                Assert.IsTrue(item.HasWarning == loadedItem.HasWarning);
                Assert.IsTrue(item.Order == loadedItem.Order);
                Assert.IsTrue(item.RepeatCount == loadedItem.RepeatCount);
                
                Assert.IsTrue(item.TruncateBeforeExecution == loadedItem.TruncateBeforeExecution);
                Assert.IsTrue(item.UseIdentityInsert == loadedItem.UseIdentityInsert);
                Assert.IsTrue(item.WarningText == loadedItem.WarningText);


                Assert.IsTrue(object.Equals(item.TargetTable, loadedItem.TargetTable));
                Assert.IsTrue(item.Equals(loadedItem));
            }
        }
    }
}
