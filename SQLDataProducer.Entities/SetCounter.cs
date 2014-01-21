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

using System.Threading;

namespace SQLDataProducer.Entities
{
    public class SetCounter
    {
        long _counter;
        //long Counter { get { return _counter; } }
        public SetCounter()
        {
            _counter = 0;
        }

        public long GetNext()
        {
            return Interlocked.Increment(ref _counter);
        }
        public void Increment()
        {
            Interlocked.Increment(ref _counter);
        }
        public long Peek()
        {
            return _counter;
        }
        public bool IsEqual(long c)
        {
            return Interlocked.Equals(_counter, c);
        }

        public long Add(long n)
        {
            return Interlocked.Add(ref _counter, n);
        }

        public void Reset()
        {
            Interlocked.Exchange(ref _counter, 0);
        }
    }
}
