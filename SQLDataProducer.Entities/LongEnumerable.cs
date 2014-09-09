using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataProducer.Entities
{
    //public class LongEnumerable 
    //{
    //    public static IEnumerable<long> Range(long start, long count)
    //    {
    //        for (long current = 0; current < count; ++current)
    //        {
    //            yield return start + current;
    //        }
    //    }
    //}
    public static class LongEnumerable
    {
        public static IEnumerable<long> Range(long start, long count)
        {
            return new RangeEnumerable(start, count);
        }
        private class RangeEnumerable : IEnumerable<long>
        {
            private long _Start;
            private long _Count;
            public RangeEnumerable(long start, long count)
            {
                _Start = start;
                _Count = count;
            }
            public virtual IEnumerator<long> GetEnumerator()
            {
                return new RangeEnumerator(_Start, _Count);
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        private class RangeEnumerator : IEnumerator<long>
        {
            private long _Current;
            private long _End;
            public RangeEnumerator(long start, long count)
            {
                _Current = start - 1;
                _End = start + count;
            }
            public virtual void Dispose()
            {
                _Current = _End;
            }
            public virtual void Reset()
            {
                throw new NotImplementedException();
            }
            public virtual bool MoveNext()
            {
                ++_Current;
                return _Current < _End;
            }
            public virtual long Current { get { return _Current; } }
            object IEnumerator.Current { get { return Current; } }
        }
    }
}
