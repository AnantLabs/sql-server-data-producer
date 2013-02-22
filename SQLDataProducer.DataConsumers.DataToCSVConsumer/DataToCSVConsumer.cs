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
    public class DataToCSVConsumer : IDataConsumer
    {
        string FileName; 

        public void Dispose()
        {
        }

        Dictionary<string, bool> _initializedFiles = new Dictionary<string, bool>();

        public Entities.ExecutionEntities.ExecutionResult Consume(DataRowSet rows, string datasetName)
        {
            using (TextWriter writer = File.AppendText(FileName))
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

        public bool Init(string target)
        {
            if (string.IsNullOrEmpty(target))
                throw new ArgumentNullException("target");

            FileName = target;

            return true;
        }


        public void CleanUp(List<string> datasetNames)
        {
            if (File.Exists(FileName))
                File.Delete(FileName);
        }

        public void PreAction(string action)
        {
            
        }

        public void PostAction(string action)
        {
            
        }
    }
}
