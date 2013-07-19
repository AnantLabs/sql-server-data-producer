using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SQLDataProducer.DataAccess;
using SQLDataProducer.DataConsumers;
using SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.Entities.OptionEntities;
using SQLDataProducer.RandomTests.Helpers;
using SQLDataProducer.TaskExecuter;

namespace SQLDataProducer.RandomTests.IntegrationTests
{
    class BugTest : TestBase
    {
        [Test]
        public void ShouldTestIdentityInsert()
        {

            DataAccess.TableEntityDataAccess tda = new TableEntityDataAccess(Connection());
            var oneTable = tda.GetTableAndColumns("dbo", "one");
            var twoTable = 


            WorkflowManager manager = new WorkflowManager();
            ExecutionTaskOptions option = new ExecutionTaskOptions();
            ExecutionItemCollection execItems = null;

            
            IDataConsumer consumer = new InsertComsumer();
            
            var res = manager.RunWorkFlow(new TaskExecuter.TaskExecuter(option, Connection(), execItems, consumer));
        }

        public BugTest()
        {
            string sql = @"
CREATE TABLE One(
id INT IDENTITY(1, 1) PRIMARY KEY,
NAME VARCHAR(50))

CREATE TABLE Two(
id INT IDENTITY(1, 1) PRIMARY KEY,
FK INT FOREIGN KEY REFERENCES one(id)
)
";
            AdhocDataAccess adhd = new AdhocDataAccess(Connection());
            adhd.ExecuteNonQuery(sql);

        }
    }
}
