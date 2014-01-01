// Copyright 2012-2014 Peter Henell

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
using SQLDataProducer.Entities.DatabaseEntities.Factories;
using SQLDataProducer.DataConsumers;
using SQLDataProducer.Entities.DataEntities;
using System.Collections.Generic;
using System;
using SQLDataProducer.Entities.DataConsumers;
using SQLDataProducer.Entities.ExecutionEntities;

namespace SQLDataProducer.Tests.ConsumerTests
{
    public class CommonDataConsumerTest
    {
        List<DataRowEntity> singleRowDataSet;
        private List<DataRowEntity> tenRowsDataSet;
        private List<DataRowEntity> mixedDataSet;
        private TableEntity customerTable;
        private TableEntity orderTable;

        ValueStore valueStore = new ValueStore();

        public Dictionary<string, string> options { get; set; }
        public Func<IDataConsumer> GetImplementedType { get; set; }

        public CommonDataConsumerTest(Dictionary<string, string> options, Func<IDataConsumer> getNewInstanceOfConsumer)
        {
            this.GetImplementedType = getNewInstanceOfConsumer;
            this.options = options;

            customerTable = new TableEntity("dbo", "Customer").AddColumn(DatabaseEntityFactory.CreateColumnEntity("CustomerId", new ColumnDataTypeDefinition("int", false), true, 1, false, null, null));

            orderTable = new TableEntity("dbo", "Order").AddColumn(DatabaseEntityFactory.CreateColumnEntity("OrderId", new ColumnDataTypeDefinition("int", false), true, 1, false, null, null));

            
            singleRowDataSet = new List<DataRowEntity>();
            singleRowDataSet.Add(new DataRowEntity(customerTable).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));

            tenRowsDataSet = new List<DataRowEntity>();
            tenRowsDataSet.Add(new DataRowEntity(customerTable).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));
            tenRowsDataSet.Add(new DataRowEntity(customerTable).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));
            tenRowsDataSet.Add(new DataRowEntity(customerTable).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));
            tenRowsDataSet.Add(new DataRowEntity(customerTable).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));
            tenRowsDataSet.Add(new DataRowEntity(customerTable).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));
            tenRowsDataSet.Add(new DataRowEntity(customerTable).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));
            tenRowsDataSet.Add(new DataRowEntity(customerTable).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));
            tenRowsDataSet.Add(new DataRowEntity(customerTable).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));
            tenRowsDataSet.Add(new DataRowEntity(customerTable).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));
            tenRowsDataSet.Add(new DataRowEntity(customerTable).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));

            mixedDataSet = new List<DataRowEntity>();
            mixedDataSet.Add(new DataRowEntity(customerTable).AddField("CustomerId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));
            mixedDataSet.Add(new DataRowEntity(orderTable).AddField("OrderId", Guid.NewGuid(), new ColumnDataTypeDefinition("int", false), false));
        }


     
        public void ShouldConsumeAllValues()
        {
            IDataConsumer consumer = GetImplementedType();

            if (consumer.Init("", options))
            {
                int rowCount = 0;
                consumer.ReportInsertion = new Action(() =>
                {
                    rowCount++;
                });
                consumer.Consume(tenRowsDataSet, valueStore);
                Assert.That(rowCount, Is.EqualTo(10), "Total rows does not match produced rows");
            }
        }

        public void ShouldProduceValuesForIdentityColumns()
        {
            ValueStore vs = new ValueStore();
            DataProducer producer = new DataProducer(vs);
            DataRowEntity row = producer.ProduceRow( customerTable, 1);
            
            // check that the value of the identity column have not been generated
            Assert.That(vs.GetByKey(row.Fields[0].KeyValue), Is.Null);

            IDataConsumer consumer = GetImplementedType();
            if (consumer.Init("", options))
            {
                int counter = 0;
                consumer.ReportInsertion = new Action(() =>
                {
                    counter++;
                });
                consumer.Consume(new List<DataRowEntity> { row }, vs);

                // now assert that the identity value have been generated by the consumer
                Assert.That(row.Fields[0].FieldName, Is.EqualTo("CustomerId"), "Column should be customerID");
                Assert.That(vs.GetByKey(row.Fields[0].KeyValue), Is.Not.Null, consumer.GetType().ToString());
            }
        }


        

        public void ShouldConsumeDataFromDifferentTables()
        {
             IDataConsumer consumer = GetImplementedType();

             if (consumer.Init("", options))
             {
                 int rowCount = 0;
                 consumer.ReportInsertion = new Action(() =>
                 {
                     rowCount++;
                 });
                 consumer.Consume(mixedDataSet, valueStore);
                 Assert.That(rowCount, Is.EqualTo(2), "Total rows does not match produced rows");
             }
        }

        public void ShouldThrowExceptionIfNotInitiatedBeforeRunning()
        {
            IDataConsumer consumer = GetImplementedType();
            bool exceptionHappened = false;
            try
            {
                consumer.Consume(new List<DataRowEntity>(), valueStore);
            }
            catch (System.Exception)
            {
                exceptionHappened = true;
            }
            Assert.That(exceptionHappened);
        }

        public void ShouldThrowExceptionIfConsumingNull()
        {
            IDataConsumer consumer = GetImplementedType();
            bool exceptionHappened = false;
            try
            {
                consumer.Init("", options);
                consumer.Consume(null, valueStore);
            }
            catch (System.Exception)
            {
                exceptionHappened = true;
            }
            Assert.That(exceptionHappened);
        }

       

        public void ShouldCountNumberOfRowsConsumed()
        {
            IDataConsumer consumer = GetImplementedType();
            if (consumer.Init("", options))
            {
                int rowCount = 0;
                consumer.ReportInsertion = new Action(() =>
                {
                    rowCount++;
                });
                consumer.Consume(singleRowDataSet, valueStore);
                Assert.That(rowCount, Is.EqualTo(1));
            }
        }

    }
}
