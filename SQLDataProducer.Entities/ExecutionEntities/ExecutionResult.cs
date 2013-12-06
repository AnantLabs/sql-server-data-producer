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
using SQLDataProducer.Entities.DatabaseEntities.Collections;

namespace SQLDataProducer.Entities.ExecutionEntities
{
    public class ExecutionResult : EntityBase
    {

        public ExecutionResult(DateTime startTime, DateTime endTime, long insertCount, ErrorList errors)
        {
            ValidateUtil.ValidateNotNull(startTime, "startTime");
            ValidateUtil.ValidateNotNull(endTime, "endTime");
            ValidateUtil.ValidateNotNull(errors, "errors");

            this.StartTime = startTime;
            this.EndTime = endTime;
            this.InsertCount = insertCount;
            this.Errors = errors;
        }

        public override string ToString()
        {
            return string.Format(@"
Inserted Rows(Approximation):    {0},
Start Time:                      {1},
End Time:                        {2},
Duration:                        {3},
Errors:                          {4}
", InsertCount, StartTime.ToString(), EndTime.ToString(), Duration.ToString(), Errors.Count);
        }

        public TimeSpan Duration { get { return EndTime - StartTime; }  }

        public DateTime StartTime { get; private set; }

        public DateTime EndTime { get; private set; }

        public long InsertCount { get; private set; }

        public ErrorList Errors { get; private set; }
    }
}
