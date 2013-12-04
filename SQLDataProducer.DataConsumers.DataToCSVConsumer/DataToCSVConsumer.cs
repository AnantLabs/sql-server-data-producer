// Copyright 2012-2013 Peter Henell

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

using System.IO;
using SQLDataProducer.Entities.DataConsumers;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.Entities.DataEntities;

namespace SQLDataProducer.DataConsumers.DataToCSVConsumer
{
    [ConsumerMetaData("Output to CSV File", "Output Folder")]
    public class DataToCSVConsumer : IDataConsumer
    {

        Dictionary<string, string> _options;
        private int rowCounter = 0;

        public string OutputFolder { get { return _options["Output Folder"]; } }

        public void Dispose()
        {
        }

        public bool Init(string connectionString, Dictionary<string, string> options)
        {
            if (null == options)
                throw new ArgumentNullException("options");

            _options = options;

            rowCounter = 0;

            DirectoryInfo di = new DirectoryInfo(OutputFolder);
            if (!di.Exists)
                Directory.CreateDirectory(OutputFolder);

            return true;
        }

        public ExecutionResult Consume(IEnumerable<DataRowEntity> rows, ValueStore valueStore)
        {
            using (TextWriter writer = File.AppendText(OutputFolder + "\\output.csv")) 
            {
                foreach (var row in rows)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (var field in row.Fields)
                    {
                        var value = valueStore.GetByKey(field.KeyValue);
                        sb.Append(value);
                        Console.WriteLine(value);
                    }
                    writer.Write(sb.ToString());

                    rowCounter++;

                    writer.WriteLine();    
                }
                
                writer.Flush();
            }
            return null;
        }

        public int TotalRows
        {
            get { return rowCounter; }
        }
    }
}
