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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using SQLDataProducer.Entities.DataConsumers;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.Entities.DataEntities;
using SQLDataProducer.Entities.DatabaseEntities.Collections;

namespace SQLDataProducer.DataConsumers.DataToCSVConsumer
{
    [ConsumerMetaData("Output to CSV File", "Output Folder")]
    public class DataToCSVConsumer : IDataConsumer
    {

        public static readonly string OUTPUT_FOLDER_PARAMETER = "Output Folder";

        Dictionary<string, string> _options;
        private int rowCounter = 0;
        private int identityCounter = 0;
        private Action _reportInsertion;
        private Action<Exception, DataRowEntity> _reportError;

        public string OutputFolder {
            get { return _options[OUTPUT_FOLDER_PARAMETER]; }
            private set { _options[OUTPUT_FOLDER_PARAMETER] = value; }
        }

        public DataToCSVConsumer()
        {
            _reportError = new Action<Exception, DataRowEntity>((ex, row) =>
            {
                Console.WriteLine("Error was reported");
            });
            _reportInsertion = new Action(() =>
            {
                Console.WriteLine("insertion made");
            });
        }

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

        public void Consume(IEnumerable<DataRowEntity> rows, ValueStore valueStore)
        {
            if (_options == null)
                throw new ArgumentNullException("Init was not called before calling consume");

            using (TextWriter writer = File.AppendText(OutputFolder + "\\output.csv")) 
            {
                foreach (var row in rows)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (var field in row.Fields)
                    {
                        if (field.ProducesValue)
                        {
                            valueStore.Put(field.KeyValue, identityCounter++);
                        }
                        var value = valueStore.GetByKey(field.KeyValue);
                        sb.Append(value);
                        Console.WriteLine(value);
                    }
                    writer.Write(sb.ToString());

                    rowCounter++;
                    _reportInsertion();
                    writer.WriteLine();    
                }
                
                writer.Flush();
            }

        }

        public int TotalRows
        {
            get { return rowCounter; }
        }


        public Action ReportInsertionCallback
        {
            set { _reportInsertion = value; }
        }

        public Action<Exception, DataRowEntity> ReportErrorCallback
        {
            set { _reportError = value; }
        }
    }
}
