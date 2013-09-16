using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Entities
{
    public class ValueStore
    {
        Dictionary<Guid, object> values;

        public ValueStore()
        {
            values = new Dictionary<Guid, object>();
        }

        public void Put(KeyValuePair<Guid, object> kv)
        {
            Put(kv.Key, kv.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Put(Guid key, object value)
        {
            values[key] = value;
        }

        /// <summary>
        /// Get value from store by key
        /// </summary>
        /// <param name="key">key to get value for</param>
        /// <returns>value, or null if key not found</returns>
        public object GetByKey(Guid key)
        {
            object value = null;
            values.TryGetValue(key, out value);
            return value;
        }
    }
}
