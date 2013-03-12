using NUnit.Framework;
using SQLDataProducer.DataConsumers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.RandomTests.ConsumerTests
{
    [TestFixture]
    public class PluginLoaderTests
    {
        [Test]
        public void ShouldLoadAllPluginsFromFolder()
        {
            var folderName = Environment.CurrentDirectory;

            // Check that there is any files to load.
            var files = System.IO.Directory.GetFiles(folderName);
            Assert.That(files.Count(), Is.GreaterThan(0));

            var plugins = PluginLoader.LoadPluginsFromFolder(folderName);

            foreach (var p in plugins)
            {
                Console.WriteLine(p);
            }
            Assert.That(plugins.Count, Is.EqualTo(4));

        }

        [Test]
        public void ShouldBeAbleToCreateInstanceOfPlugin()
        {
            var folderName = Environment.CurrentDirectory;
            var plugins = PluginLoader.LoadPluginsFromFolder(folderName);

            foreach (var plugin in plugins)
            {
                var instance = plugin.CreateInstance();

                Assert.That(instance, Is.Not.Null);
            }
        }
    }
}
