using SQLDataProducer.Entities.DataConsumers;
using SQLDataProducer.Entities.DataEntities;
using SQLDataProducer.Entities.ExecutionEntities;
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
        public MockDataConsumer()
        {
            ValueValidator = new Action<object>(value => { return; });
        }

        public bool Init(string connectionString, Dictionary<string, string> options = null)
        {
            rowCounter = 0;
            return initialized = true;
        }

        public ExecutionResult Consume(IEnumerable<DataRowEntity> rows, ValueStore valueStore)
        {
            ValidateInitialized();
            foreach (var row in rows)
            {
                rowCounter++;
                //Console.WriteLine(row);
                foreach (var field in row.Fields)
                {
                    ValueValidator(valueStore.GetByKey(field.KeyValue));
                    Console.WriteLine(valueStore.GetByKey(field.KeyValue));
                }
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

        public Action<object> ValueValidator { get; set; }

        public static Action<object> NonNullValueValidator { get { return new Action<object>(value => { if (value == null) throw new ArgumentNullException(); }); } }
    }
}
