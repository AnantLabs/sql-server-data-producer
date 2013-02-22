using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.DataConsumers.DataToMSSSQLScriptConsumer
{
    public class DataToMSSSQLScriptConsumer : IDataConsumer
    {
        public bool Init(string target)
        {
            throw new NotImplementedException();
        }

        public Entities.ExecutionEntities.ExecutionResult Consume(Entities.DataEntities.Collections.DataRowSet rows)
        {
            throw new NotImplementedException();
        }

        public void CleanUp(List<string> datasetNames)
        {
            throw new NotImplementedException();
        }

        public void PreAction(string action)
        {
            throw new NotImplementedException();
        }

        public void PostAction(string action)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
