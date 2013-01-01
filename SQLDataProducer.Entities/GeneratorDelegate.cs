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

using System.Collections.Generic;
using SQLDataProducer.Entities.Generators.Collections;
using SQLDataProducer.Entities.ExecutionEntities;

namespace SQLDataProducer.Entities
{
    // Functions that deliver an instance of something that can generate values
    //public delegate IValueCreator GeneratorDelegate();

    // Functions that can generate values using the serial number n and the supplied parameters
    public delegate object ValueCreatorDelegate(long n, GeneratorParameterCollection genParameters);

    //Func<string, IEnumerable<ExecutionItem>, string>
    public delegate string FinalQueryGeneratorDelegate(string baseQUery, ExecutionItemCollection executionItems, long n, System.Func<long> numberGenerator);

    // Action<>
    public delegate void ExecutionTaskDelegate();

    //Action<int>
    public delegate void ExecutionDoneCallbackDelegate(int n);

    public delegate void TableWithForeignKeyInsertedRowEventHandler(string tableSchema, string tableName, int insertedFKId);
}
