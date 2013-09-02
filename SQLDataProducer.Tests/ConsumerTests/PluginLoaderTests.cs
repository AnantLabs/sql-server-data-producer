using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLDataProducer.DataConsumers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Tests.ConsumerTests
{
    [TestFixture]
    [MSTest.TestClass]
    public class PluginLoaderTests
    {
        [Test]
        [MSTest.TestMethod]
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
        [MSTest.TestMethod]
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
