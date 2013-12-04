using SQLDataProducer.DataConsumers;
using SQLDataProducer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Tests
{
    public class GenerationManager
    {
        private IDataConsumer consumer;
        private NodeIterator nodeIterator;
        private DataProducer dataProducer;
        private Func<long> currentNumberFunction;

        public GenerationManager(IDataConsumer mockDataConsumer, NodeIterator nodeIterator, DataProducer dataProducer, Func<long> currentNumberFunction) 
        {
            this.consumer = mockDataConsumer;
            this.nodeIterator = nodeIterator;
            this.dataProducer = dataProducer;
            this.currentNumberFunction = currentNumberFunction;
        }

        internal void Run(string connectionString)
        {
            consumer.Init("");
            consumer.Consume(dataProducer.ProduceRows(nodeIterator.GetTablesRecursive(), currentNumberFunction));
        }
    }
}
