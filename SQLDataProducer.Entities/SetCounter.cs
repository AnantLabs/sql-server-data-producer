using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
