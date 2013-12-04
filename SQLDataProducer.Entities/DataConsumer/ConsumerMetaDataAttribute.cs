using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Entities.DataConsumers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConsumerMetaDataAttribute : Attribute
    {
        public ConsumerMetaDataAttribute(string consumerName, params string[] optionsTemplate)
        {
            ConsumerName = consumerName;

            OptionsTemplate = new Dictionary<string, string>();

            if (null != optionsTemplate)
            {
                foreach (var kv in optionsTemplate)
                    OptionsTemplate.Add(kv, "");
            }

        }

        public string ConsumerName { get; set; }
        public Dictionary<string, string> OptionsTemplate { get; set; }
    }
}
