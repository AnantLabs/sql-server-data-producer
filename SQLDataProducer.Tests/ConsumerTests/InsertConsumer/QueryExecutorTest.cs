// Copyright 2012-2013 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.Generators.DecimalGenerators;
using SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer.DataAccess;
using System.IO;
using System.Data.SqlServerCe;

namespace SQLDataProducer.Tests.ConsumerTests.InsertConsumer
{
    [TestFixture]
    [MSTest.TestClass]
    public class QueryExecutorTest
    {
        private SqlCeEngine engine;
        private SqlCeConnection conn;

        public QueryExecutorTest()
        {
            CommandFactory.DbProviderFactoryFactory.ProviderFactory = SqlCeProviderFactory.Instance;
            
            if (File.Exists("Test.sdf"))
                File.Delete("Test.sdf");

            engine = new SqlCeEngine("Data Source = Test.sdf");
            engine.CreateDatabase();
        }
       
        [Test]
        [MSTest.TestMethod]
        public void ShouldInstantiateQueryExecutor()
        {
            QueryExecutor executor;
            using (executor = new QueryExecutor("Data Source = Test.sdf"))
            {
                Assert.That(executor, Is.Not.Null);
            }
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldRunCommandWithNoParameters()
        {
            using (QueryExecutor executor = new QueryExecutor("Data Source = Test.sdf"))
            {
                executor.ExecuteNonQuery("create table Customer(CustomerId int identity(1, 1) primary key, name nvarchar(100) not null)");
                executor.ExecuteNonQuery("insert into Customer(Name) values('Peter')");
            }
            using (conn = new SqlCeConnection("Data Source = Test.sdf"))
            {
                conn.Open();

                SqlCeCommand cmd = conn.CreateCommand();
                cmd.CommandText = "Select CustomerId, Name from Customer";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    Assert.That(id, Is.GreaterThan(0));
                    Assert.That(name, Is.Not.Null);
                }
            }
        }
    }
}
