using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.DataConsumers.DataToMSSSQLScriptConsumer
{
    [ConsumerMetaData("Create Insert Scripts", "Output Folder")]
    public class DataToMSSSQLScriptConsumer : IDataConsumer
    {
        Dictionary<string, string> _options;

      
        public string OutputFolder { 
            get { return _options["Output Folder"]; } 
            set { _options["Output Folder"] = value; } 
        }

      

        public bool Init(string connectionString, Dictionary<string, string> options)
        {
            _options = options;
            return true;
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
