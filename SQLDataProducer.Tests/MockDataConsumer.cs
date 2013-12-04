using SQLDataProducer.Entities.DataConsumers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Tests
{
    class MockDataConsumer : IDataConsumer
    {

        private int rowCounter = 0;
        private bool initialized = false;

        public bool Init(string connectionString, Dictionary<string, string> options = null)
        {
            rowCounter = 0;
            return initialized = true;
        }

        public SQLDataProducer.Entities.ExecutionEntities.ExecutionResult Consume(IEnumerable<SQLDataProducer.Entities.DataEntities.DataRowEntity> rows)
        {
            ValidateInitialized();
            foreach (var row in rows)
            {
                rowCounter++;
                Console.WriteLine(row);
            }
            
            return null;
        }

        private void ValidateInitialized()
        {
            if (!initialized)
            {
                throw new NotSupportedException("consumer is not initialized. Make sure to run Init(...).");
            }
        }

        public void Dispose()
        {

        }

        public int TotalRows
        {
            get
            {
                return rowCounter;
            }
        }
    }
}
