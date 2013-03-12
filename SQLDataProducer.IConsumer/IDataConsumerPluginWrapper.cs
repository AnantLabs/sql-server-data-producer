using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.DataConsumers
{
    public class IDataConsumerPluginWrapper
    {
        public Type PluginType { get; private set; }
        public string PluginName { get; private set; }
        public Dictionary<string, string> OptionsTemplate { get; private set; }

        public IDataConsumerPluginWrapper(string pluginName, Type pluginType, Dictionary<string, string> optionsTemplate)
        {
            if (string.IsNullOrEmpty(pluginName))
                throw new ArgumentNullException("pluginName");
            if(pluginType == null)
                throw new ArgumentNullException("pluginType");
            if (optionsTemplate == null)
                throw new ArgumentNullException("optionsTemplate");

            PluginType = pluginType;
            PluginName = pluginName;
            OptionsTemplate = optionsTemplate;
        }

        public IDataConsumer CreateInstance()
        {
            IDataConsumer instance = Activator.CreateInstance(PluginType) as IDataConsumer;
            return instance;
        }

        public override string ToString()
        {
            return string.Format("PluginType = {0}, PluginName = {1}, OptionsTemplate = {2}", PluginType, PluginName, OptionsTemplate);
        }
    }
}
