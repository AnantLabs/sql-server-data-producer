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

namespace TestConsoleApplication
{
    class Program
    {
        
        static void Main(string[] args)
        {
            List<ExecutionItem> lits = new List<ExecutionItem>(); ;

            TableEntityDataAccess tda = new TableEntityDataAccess( Connection());

            TableEntity table = tda.GetTableAndColumns("core", "eventlog");
            TableEntity table2 = tda.GetTableAndColumns("core", "staff");

            lits.Add(new ExecutionItem(table, 1));
            lits.Add(new ExecutionItem(table2, 2));


            TaskExecuter executor = new TaskExecuter();


            InsertQueryGenerator queryGenerator = new InsertQueryGenerator();
            string result = queryGenerator.GenerateQueryForExecutionItems(lits);

            Action<int> a = executor.CreateSQLTaskForExecutionItems(lits, result, Connection());

            for (int n = 0; n < 10; n++)
            {
                executor.ExecuteOneTime(a, n);
            }
        }

      

        private static string Connection()
        {
            return "Data Source=localhost;Initial Catalog=CustomerDB;Integrated Security=True";
        }
    }
}
