using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLRepeater.EntityQueryGenerator;
using SQLRepeater.Entities.ExecutionOrderEntities;
using SQLRepeater.DatabaseEntities.Entities;
using SQLRepeater.DataAccess;
using SQLRepeater.TaskExecuter;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using SQLRepeater.Entities;
using System.Threading;
using SQLRepeater.Entities.OptionEntities;

namespace TestConsoleApplication
{
    class Program
    {

        static void Main(string[] args)
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());

            TableEntity table = tda.GetTableAndColumns("dbo", "Customer");

            //ExecutionTaskOptionsManager.Instance.Options.ExecutionType = ExecutionTypes.DurationBased;
            //ExecutionTaskOptionsManager.Instance.Options.SecondsToRun = 30;
            //ExecutionTaskOptionsManager.Instance.Options.MaxThreads = 20;

            ExecutionItemCollection list = SetupExecutionItems(table);
            TaskExecuter executor = new TaskExecuter(Connection());


            InsertQueryGenerator queryGenerator = new InsertQueryGenerator();
            string result = queryGenerator.GenerateQueryForExecutionItems(list);


            SetCounter newSetCounter = RunExecutionBasedTest(list, executor, queryGenerator, result);

            Thread.Sleep(4000);

            Console.WriteLine(newSetCounter.Peek());
            Console.ReadKey();
            
        }

        private static SetCounter RunExecutionBasedTest(ExecutionItemCollection list, TaskExecuter executor, InsertQueryGenerator queryGenerator, string result)
        {
            ExecutionTaskOptionsManager.Instance.Options.ExecutionType = ExecutionTypes.ExecutionCountBased;
            ExecutionTaskOptionsManager.Instance.Options.FixedExecutions = 2;
            ExecutionTaskOptionsManager.Instance.Options.MaxThreads = 20;
            ExecutionTaskDelegate a = executor.CreateSQLTaskForExecutionItems(list, result, queryGenerator.GenerateFinalQuery);
            SetCounter newSetCounter = new SetCounter();
            executor.CreateExecutionCountBasedAction(a, newSetCounter)();
            return newSetCounter;
        }

        private static ExecutionItemCollection SetupExecutionItems(TableEntity table)
        {
            ExecutionItemCollection list = new ExecutionItemCollection();
            ExecutionItem item = new ExecutionItem(table, 1);
            item.RepeatCount = 10;
            item.TruncateBeforeExecution = false;
            ExecutionItem item2 = new ExecutionItem(table, 2);
            item2.RepeatCount = 1;
            list.Add(item);
            list.Add(item2);
            //list.Save(@"c:\temp\test.xml");
            return list;
        }

      

        private static string Connection()
        {
            return "Data Source=localhost;Initial Catalog=CustomerDB;Integrated Security=True";
        }
    }
}
