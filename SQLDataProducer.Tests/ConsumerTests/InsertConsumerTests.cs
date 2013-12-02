//using NUnit.Framework;
//using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
//using SQLDataProducer.Entities.DatabaseEntities;
//using SQLDataProducer.Entities.DataEntities.Collections;

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SQLDataProducer.Tests.Helpers;
//using SQLDataProducer.DataConsumers;
//using SQLDataProducer.Entities;

//namespace SQLDataProducer.Tests.ConsumerTests
//{
//    [TestFixture]
//    [MSTest.TestClass]
//    public class InsertConsumerTests : TestBase
//    {
//        public InsertConsumerTests()
//            : base()
//        {

//        }

//        [Test]
//        [MSTest.TestMethod]
//        public void ShouldConsumeDataAndInsertTheRowsToDB()
//        {
//            var listOfExecutionItems = ExecutionItemHelper.GetRealExecutionItemCollection(Connection());

//            long i = 0;
//            Func<long> getN = new Func<long>(() =>
//                { return i++; });

//            using (IDataConsumer consumer = new SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer.InsertComsumer())
//            {
//                consumer.Init(Connection());

//                foreach (var item in listOfExecutionItems)
//                {
//                    var data = item.CreateData(getN, new SetCounter());
//                    if (data.Count == 0)
//                        continue;

//                    DefaultDataConsumer.Consume(data);

//                    consumer.Consume(data);
//                }
//            }
//        }
//    }
//}
