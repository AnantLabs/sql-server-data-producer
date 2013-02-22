using NUnit.Framework;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DataEntities.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer.Tests
{
    [TestFixture]
    public class InsertConsumerTests
    {
        protected IDataConsumer DefaultDataConsumer = new SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer.InsertComsumer();

        [Test]
        public void ShouldConsumeData()
        {
            var rows = new DataRowSet();
            
            DefaultDataConsumer.Consume(rows, "");

        }
    }
}
