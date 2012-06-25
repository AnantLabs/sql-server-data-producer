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

            TableEntity table = tda.GetTableAndColumns("dbo", "Item");

            ExecutionItemCollection original = SetupExecutionItems(table);
            WorkflowManager manager = new WorkflowManager();

            ExecutionTaskOptions options = new ExecutionTaskOptions();
            options.ExecutionType = ExecutionTypes.ExecutionCountBased;
            options.FixedExecutions = 200;
            options.MaxThreads = 20;
            options.OnlyOutputToFile = true;
            options.ScriptOutputFolder = @"c:\temp\repeater";
            string preScript = string.Empty;
            string postScript = string.Empty;

            ExecutionItemCollectionManager m = new ExecutionItemCollectionManager();
            m.Save(original, @"c:\temp\repeater.xml");

            ExecutionItemCollection loaded = m.Load(@"c:\temp\repeater.xml", Connection());
            //Console.WriteLine(loaded);
            TestIfEqual(original, loaded);
            //ExecutionResult res = manager.RunWorkFlow(options, Connection(), list);

            //Console.WriteLine(res.ToString());
            Console.WriteLine("Done");
            Console.ReadKey();

        }

        private static void TestIfEqual(ExecutionItemCollection original, ExecutionItemCollection loaded)
        {
            if (Assert(original.Count == loaded.Count, "Not same amount of exec items"))
            {
                for (int i = 0; i < original.Count; i++)
                {
                    Assert(original[i].Description == loaded[i].Description, "description does not match");
                    Assert(original[i].Order == loaded[i].Order, "Order does not match");

                    Assert(original[i].TargetTable.TableName == loaded[i].TargetTable.TableName, "Target table does not match");
                    Assert(original[i].TargetTable.TableSchema == loaded[i].TargetTable.TableSchema, "Target table TableSchema does not match");
                    if (Assert(original[i].TargetTable.Columns.Count == loaded[i].TargetTable.Columns.Count, "Column count does not match"))
                    {
                        for (int j = 0; j < original[i].TargetTable.Columns.Count; j++)
                        {
                            ColumnEntity loadedColumn = loaded[i].TargetTable.Columns[j];
                            ColumnEntity originalColumn = original[i].TargetTable.Columns[j];

                            Assert(originalColumn.ColumnName == loadedColumn.ColumnName, "Column names does not match");
                            Assert(originalColumn.ColumnDataType == loadedColumn.ColumnDataType, "ColumnDataType does not match");
                            Assert(originalColumn.IsForeignKey == loadedColumn.IsForeignKey, "IsForeignKey does not match");

                            Assert(originalColumn.IsIdentity == loadedColumn.IsIdentity, "IsIdentity does not match");
                            Assert(originalColumn.IsNotIdentity == loadedColumn.IsNotIdentity, "IsNotIdentity does not match");
                            Assert(originalColumn.OrdinalPosition == loadedColumn.OrdinalPosition, "OrdinalPosition does not match");

                            //Assert(list[i].TargetTable.Columns[j].PossibleGenerators == loaded[i].TargetTable.Columns[j].PossibleGenerators, "PossibleGenerators does not match");
                            Assert(originalColumn.PossibleGenerators.Count == loadedColumn.PossibleGenerators.Count, "PossibleGenerators count is not equal");


                            Assert(originalColumn.Generator.GeneratorName == loadedColumn.Generator.GeneratorName, "Generator does not matchdoes");
                            Assert(originalColumn.Generator.GeneratorParameters.Count == loadedColumn.Generator.GeneratorParameters.Count, "GeneratorParameters does not match");
                            for (int ii = 0; ii < originalColumn.Generator.GeneratorParameters.Count; ii++)
                            {
                                Assert(originalColumn.Generator.GeneratorParameters[ii].ParameterName == loadedColumn.Generator.GeneratorParameters[ii].ParameterName, "Parameter name not equal");
                                if(!Assert(originalColumn.Generator.GeneratorParameters[ii].Value.ToString() == loadedColumn.Generator.GeneratorParameters[ii].Value.ToString(), "Value not equal"))
                                    Console.WriteLine("{0} - {1}", originalColumn.Generator.GeneratorParameters[ii].Value, loadedColumn.Generator.GeneratorParameters[ii].Value);
                                
                                Assert(originalColumn.Generator.GeneratorParameters[ii].IsWriteEnabled == loadedColumn.Generator.GeneratorParameters[ii].IsWriteEnabled, "IsWriteEnabled not equal");
                            }
                        }
                    }
                }
            }
        }

        private static bool Assert(bool p, string message)
        {
            if (!p)
                Console.WriteLine(message);
            return p;
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
