using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDataProducer.EntityQueryGenerator;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.DatabaseEntities.Entities;
using SQLDataProducer.DataAccess;
using SQLDataProducer.TaskExecuter;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using SQLDataProducer.Entities;
using System.Threading;
using SQLDataProducer.Entities.OptionEntities;
using SQLDataProducer;

namespace TestConsoleApplication
{
    class Program
    {

        static void Main(string[] args)
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());

            TableEntity table = tda.GetTableAndColumns("chat", "badword");

            ExecutionItemCollection list = SetupExecutionItems(table);
            WorkflowManager manager = new WorkflowManager();

            ExecutionTaskOptions options = new ExecutionTaskOptions();
            options.ExecutionType = ExecutionTypes.ExecutionCountBased;
            options.FixedExecutions = 200;
            options.MaxThreads = 20;
            options.OnlyOutputToFile = true;
            options.ScriptOutputFolder = @"c:\temp\repeater";
            string preScript = string.Empty;
            string postScript = string.Empty;
            
            int res = manager.RunWorkFlow(options, Connection(), list);

            Console.WriteLine(res);
            Console.ReadKey();
            
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
