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

using SQLDataProducer.Entities.DatabaseEntities;
//using SQLDataProducer.Entities.DataConsumer;
using SQLDataProducer.Entities.DataEntities;
using SQLDataProducer.Entities.ExecutionEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Entities.DataConsumers
{
    /// <summary>
    /// Implementers of this interface should consume the flow of data rows generated and return a result of how the the consumation went.
    /// </summary>
    public interface IDataConsumer : IDisposable
    {
        /// <summary>
        /// Initialize the data consumer, Will be called once before running anything.
        /// </summary>
        
        /// <param name="connectionString">Connectionstring to be used when consuming</param>
        /// <param name="options">string string dictionary with options that this consumer will use</param>
        /// <returns>true if the consumer was initialized successfully</returns>
        bool Init(string connectionString, Dictionary<string, string> options);

        /// <summary>
        /// To be called when an consumation is made in order to follow
        /// progress outside the consumer
        /// </summary>
        Action ReportInsertion { set; }

        /// <summary>
        /// To be called when some error occur during consumation.
        /// </summary>
        Action<Exception, DataRowEntity> ReportError { set; }

        /// <summary>
        /// Consume the data feed
        /// </summary>
        /// <param name="rows">the datarows containing the values that should be consumed</param>
        /// <returns>An executionResult with the result of this execution. 
        /// The calling manager is responsible for collecting this 
        /// delta and append it to the big result</returns>
        void Consume(IEnumerable<DataRowEntity> rows, ValueStore valueStore);

    }
}
