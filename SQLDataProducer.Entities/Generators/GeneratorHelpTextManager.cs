using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SQLDataProducer.Entities.Generators
{
    class GeneratorHelpTextManager
    {
        // cache to hold the generator texts to avoid reading the xml file multiple times.
        private static Dictionary<string, string> generatorHelpTexts;
        /// <summary>
        /// Get the help text for the supplied generator name. The name of the generator need to match the one in the helptext xml file
        /// </summary>
        /// <param name="generatorName"></param>
        /// <returns></returns>
        public static string GetGeneratorHelpText(string generatorName)
        {
            if (generatorHelpTexts == null)
                generatorHelpTexts = LoadGeneratorHelpTexts();

            // If the generator name was not found in the dictionary just return empty string.
            string ret = String.Empty;
            if (generatorHelpTexts.ContainsKey(generatorName))
                ret = generatorHelpTexts[generatorName];

            return ret;
        }

        private static Dictionary<string, string> LoadGeneratorHelpTexts()
        {
            var dic = new Dictionary<string, string>();

            try
            {
                string helpTextFile = @".\Generators\resources\GeneratorHelpTexts.xml";

                XDocument doc = XDocument.Load(helpTextFile);
                var texts = from en in doc.Descendants("Text")
                            select new
                            {
                                GenName = en.Attribute("generatorName").Value,
                                Text = en.Value
                            };

                foreach (var kv in texts)
                    dic.Add(kv.GenName, kv.Text);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return dic;
        }
    }
}
