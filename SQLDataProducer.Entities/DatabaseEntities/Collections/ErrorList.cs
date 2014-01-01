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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Entities.DatabaseEntities.Collections
{
    public class ErrorList : ICollection<string>
    {
        List<string> _queue = new List<string>();
        int _maximumSize;

        public ErrorList(int maximumSize)
        {
            if (maximumSize < 0)
                throw new ArgumentOutOfRangeException("maximumSize");
            this._maximumSize = maximumSize;
        }
        public ErrorList()
            : this(10)
        {
        }

        private string Dequeue()
        {
            if (_queue.Count > 0)
            {
                string value = _queue[0];
                _queue.RemoveAt(0);
                return value;
            }
            return default(string);
        }

        private string Peek()
        {
            if (_queue.Count > 0)
            {
                return _queue[0];
            }
            return default(string);
        }

        private void Enqueue(string item)
        {
            if (_queue.Contains(item))
            {
                _queue.Remove(item);
            }
            _queue.Add(item);
            while (_queue.Count > _maximumSize)
            {
                Dequeue();
            }
        }

        public int Count
        {
            get
            {
                return _queue.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(string item)
        {
            Enqueue(item);
        }

        public void Clear()
        {
            _queue.Clear();
        }

        public bool Contains(string item)
        {
            return _queue.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            foreach (string value in _queue)
            {
                if (arrayIndex >= array.Length) break;
                if (arrayIndex >= 0)
                {
                    array[arrayIndex] = value;
                }
                arrayIndex++;
            }
        }

        public bool Remove(string item)
        {
            if (Object.Equals(item, Peek()))
            {
                Dequeue();
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        public void AddRange(IEnumerable<string> strings)
        {
            foreach (var s in strings)
            {
                Add(s);
            }
        }
    }
}
