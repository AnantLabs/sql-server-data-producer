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

            TableEntity table = tda.GetTableAndColumns("chat", "badword");

            ExecutionTaskOptionsManager.Instance.Options.ExecutionType = ExecutionTypes.ExecutionCountBased;
            ExecutionTaskOptionsManager.Instance.Options.FixedExecutions = 1;

            ObservableCollection<ExecutionItem> list = new ObservableCollection<ExecutionItem>();
            ExecutionItem item = new ExecutionItem(table, 1);
            item.RepeatCount = 5;
            ExecutionItem item2 = new ExecutionItem(table, 2);
            item2.RepeatCount = 1;

            TaskExecuter executor = new TaskExecuter(Connection());
            list.Add(item);
            list.Add(item2);

            InsertQueryGenerator queryGenerator = new InsertQueryGenerator();
            string result = queryGenerator.GenerateQueryForExecutionItems(list);

            ExecutionTaskDelegate a = executor.CreateSQLTaskForExecutionItems(list, result, queryGenerator.GenerateFinalQuery);


            executor.BeginExecute(a, v =>
                {

                });
           // Thread.Sleep(1000);
        }

      

        private static string Connection()
        {
            return "Data Source=localhost;Initial Catalog=CustomerDB;Integrated Security=True";
        }
    }
}
