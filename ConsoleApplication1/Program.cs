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

namespace TestConsoleApplication
{
    class Program
    {
        
        static void Main(string[] args)
        {
            TableEntityDataAccess tda = new TableEntityDataAccess( Connection());

            ObservableCollection<TableEntity> tables = tda.GetAllTablesWithColumns();

            ObservableCollection<ExecutionItem> list = ExecutionItem.FromTables(tables);
            TaskExecuter executor = new TaskExecuter();

            InsertQueryGenerator queryGenerator = new InsertQueryGenerator();
            string result = queryGenerator.GenerateQueryForExecutionItems(list);

            Action<int> a = executor.CreateSQLTaskForExecutionItems(list, result, Connection());

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
