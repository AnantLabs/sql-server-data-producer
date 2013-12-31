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

using SQLDataProducer.Entities.DatabaseEntities.Collections;
//using SQLDataProducer.Entities.DataConsumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataProducer.Entities.ExecutionEntities
{
    public class ExecutionResultBuilder
    {
        private DateTime _startTime;
        private Dictionary<Exception, int> _errors;
        private SetCounter _counter;

        public ExecutionResultBuilder()
        {
            _errors = new Dictionary<Exception, int>();
            _counter = new SetCounter();
        }

        public void Begin()
        {
            _startTime = DateTime.Now;
        }

        public void AddError(Exception exception)
        {
            int errorCount = 0;
            _errors.TryGetValue(exception, out errorCount);
            _errors.Add(exception, errorCount++);
        }

        public void Increment()
        {
            _counter.Increment();
        }

        public ExecutionResult Build()
        {
            ErrorList errorList = new ErrorList(25);
            errorList.AddRange(_errors.Keys.Select(x => x.ToString()));
           return new ExecutionResult(_startTime, DateTime.Now, _counter.Peek(), errorList);
        }

        
    }
}
