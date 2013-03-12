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

using SQLDataProducer.DataConsumers;
using SQLDataProducer.Entities.DataEntities.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.DataConsumers
{
    [ConsumerMetaData("Output to Console", null)]
    public class DataToConsoleConsumer : IDataConsumer
    {

        public Entities.ExecutionEntities.ExecutionResult Consume(DataRowSet rows)
        {
            foreach (var r in rows)
            {
                foreach (var cell in r.Cells)
                {
                    Console.WriteLine("{0} = {1}", cell.Column.ColumnName, cell.Value);
                }

            }
            return null;
        }

        public void Dispose()
        {
            
        }

        public bool Init(string connectionString, Dictionary<string, string> options = null)
        {
            return true;
        }


        public void CleanUp(List<string> datasetNames)
        {
         
        }

        public void PreAction(string action)
        {
         
        }

        public void PostAction(string action)
        {
         
        }

        Dictionary<string, string> _options;

        public Dictionary<string, string> ConsumerOptions
        {
            get { return _options; }
        }

        
    }
}
