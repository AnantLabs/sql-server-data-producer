using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLDataProducer.DataConsumers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDataProducer.Entities.DataConsumers;

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
            var plugins = GetPlugins();

            foreach (var p in plugins)
            {
                Console.WriteLine(p);
            }
            Assert.That(plugins.Count, Is.EqualTo(2), "Notice when new plugins are added");

        }

        private static List<IDataConsumerPluginWrapper> GetPlugins()
        {
            var folderName = Environment.CurrentDirectory;

            // Check that there is any files to load.
            var files = System.IO.Directory.GetFiles(folderName);
            Assert.That(files.Count(), Is.GreaterThan(0));

            var plugins = PluginLoader.LoadPluginsFromFolder(folderName);
            return plugins;
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

        [Test]
        [MSTest.TestMethod]
        public void ShouldBeAbleToRunAllPlugins()
        {
            var plugins = GetPlugins();
            foreach (var plugin in plugins)
            {
                var options = new Dictionary<string, string>();
                foreach (var key in plugin.OptionsTemplate.Keys)
                {
                    if (string.IsNullOrEmpty(plugin.OptionsTemplate[key]))
                    {
                        // this is a wild guess for now
                        options[key] = ".";
                    }
                }

                var test = new CommonDataConsumerTest(options, new Func<IDataConsumer>(() => { return plugin.CreateInstance(); }));
                test.ShouldConsumeAllValues();
                test.ShouldConsumeDataFromDifferentTables();
                test.ShouldCountNumberOfRowsConsumed();
                test.ShouldProduceValuesForIdentityColumns();
                test.ShouldResetTheTotalRowsConsumedAtInit();
                test.ShouldReturnResultAfterConsumption();
                test.ShouldThrowExceptionIfConsumingNull();
                test.ShouldThrowExceptionIfNotInitiatedBeforeRunning();
            }
        }
    }
}
