using NUnit.Framework;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DataEntities.Collections;
using SQLDataProducer.RandomTests;
using SQLDataProducer.RandomTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer.ConsumerTests
{
    [TestFixture]
    public class InsertConsumerTests : TestBase
    {
        public InsertConsumerTests()
            : base ()
        {

        }
        
        [Test]
        public void ShouldConsumeDataAndInsertTheRowsToDB()
        {
            var listOfExecutionItems = ExecutionItemHelper.GetRealExecutionItemCollection(Connection());

            long i = 0;
            Func<long> getN = new Func<long>( () =>
                { return i++;});

            using (IDataConsumer consumer = new SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer.InsertComsumer())
            {
                consumer.Init(Connection());

                foreach (var item in listOfExecutionItems)
                {
                    var data = item.CreateData(getN, new Entities.SetCounter());
                    if (data.Count == 0)
                        continue;
                    
                    DefaultDataConsumer.Consume(data);

                    consumer.Consume(data);
                }
            }
        }
    }
}
