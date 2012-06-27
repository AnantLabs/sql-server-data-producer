// Copyright 2012 Peter Henell

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
        int _counter;
        int Counter { get { return _counter; } }
        public SetCounter()
        {
            _counter = 0;
        }

        public int GetNext()
        {
            return Interlocked.Increment(ref _counter);
        }
        public void Increment()
        {
            Interlocked.Increment(ref _counter);
        }
        public int Peek()
        {
            return _counter;
        }
        public bool IsEqual(int c)
        {
            return Interlocked.Equals(_counter, c);
        }
        object _lock = new object();
         
        internal bool IncrementIfLessThan(int targetNumExecutions)
        {
            lock (_lock)
            {
                int next = Peek() + 1;
                if (next < targetNumExecutions)
                {
                    Increment();
                    return true;
                }
                return false;
            }
        }
    }
}
