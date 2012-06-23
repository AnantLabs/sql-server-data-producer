using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Entities
{
    public static class Extensions
    {
        public static int TryGetIntAttribute(this System.Xml.XmlReader reader, string attributeName, int fallback)
        {
            int a;
            if (int.TryParse(reader.GetAttribute(attributeName), out a))
                return a;
            
            return fallback;
        }
    }
}
