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
using SQLDataProducer.Entities.DataEntities.Collections;

namespace SQLDataProducer.DataConsumers.DataToCSVConsumer
{
    [ConsumerMetaData("Output to CSV File", "Output Folder")]
    public class DataToCSVConsumer : IDataConsumer
    {

        Dictionary<string, string> _options;

        public string OutputFolder { get { return _options["Output Folder"]; } }

        public void Dispose()
        {
        }

        public Entities.ExecutionEntities.ExecutionResult Consume(DataRowSet rows)
        {
            using (TextWriter writer = File.AppendText(OutputFolder + rows.TargetTable.TableName))
            {
                foreach (var r in rows)
                {
                    var commaSeparatedStringOfValues = String.Join(", ", r.Cells.Select(s => s.Value));
                    writer.Write(commaSeparatedStringOfValues);

                    writer.WriteLine();
                    writer.Flush();
                }
            }
            return null;
        }

        public bool Init(string connectionString, Dictionary<string, string> options)
        {
            if (null == options)
                throw new ArgumentNullException("options");

            _options = options;

            DirectoryInfo di = new DirectoryInfo(OutputFolder);
            if (!di.Exists)
                Directory.CreateDirectory(OutputFolder);

            return true;
        }


        public void CleanUp(List<string> datasetNames)
        {
            foreach (var tableName in datasetNames)
            {
                var fileName = OutputFolder + tableName;
                
                if (File.Exists(fileName))
                    File.Delete(fileName);
            }
            
        }

        public void PreAction(string action)
        {
            
        }

        public void PostAction(string action)
        {
            
        }
    }
}
