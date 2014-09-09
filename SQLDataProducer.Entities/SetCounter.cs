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
        
        public SetCounter()
        {
            _counter = 0;
        }

        /// <summary>
        /// Increment the current value by one and return the new value
        /// </summary>
        /// <returns>Returns the new value</returns>
        public long GetNext()
        {
            return Interlocked.Increment(ref _counter);
        }
        /// <summary>
        /// Increment the current value by one
        /// </summary>
        public void Increment()
        {
            Interlocked.Increment(ref _counter);
        }
        /// <summary>
        /// Returns the current value
        /// </summary>
        /// <returns></returns>
        public long Peek()
        {
            return Interlocked.Read(ref _counter);
            //return _counter;
        }
        /// <summary>
        /// Returns true if the current value is equal to <paramref name="c"/>
        /// </summary>
        /// <param name="c">The value to compare the current value to.</param>
        /// <returns>true if the current value is equal to <paramref name="c"/>, otherwise false</returns>
        public bool IsEqual(long c)
        {
            return Interlocked.Equals(_counter, c);
        }
        /// <summary>
        /// Add <paramref name="n"/> to the current value of the counter
        /// </summary>
        /// <param name="n">The value to add to the current value</param>
        /// <returns>The value after adding <paramref name="n"/></returns>
        public long Add(long n)
        {
            return Interlocked.Add(ref _counter, n);
        }
        /// <summary>
        /// Reset the counter back to zero
        /// </summary>
        public void Reset()
        {
            Interlocked.Exchange(ref _counter, 0);
        }
    }
}
