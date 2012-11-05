// Copyright 2012 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using SQLDataProducer.RandomTestsnStuff;


namespace TestConsoleApplication
{
    class Program
    {

        static void Main(string[] args)
        {
            //TableEntityDataAccess tda = new TableEntityDataAccess(Connection());

            //TableEntity table = tda.GetTableAndColumns("Person", "Address");
            //ExecutionItem ei = new ExecutionItem(table);
            //ei.RepeatCount = 2;
            //TableEntityInsertStatementBuilder builder = new TableEntityInsertStatementBuilder(ei);
            //int i = 1;
            //builder.GenerateValues(() => i++);

            //foreach (var p in builder.Parameters)
            //{
            //    Console.WriteLine(p.Value + ":" + p.Value.Value);
            //}
            //Console.WriteLine(builder.InsertStatement);

            //Console.WriteLine();
            //Console.WriteLine(builder.GenerateFullStatement());

            RandomTests t = new  RandomTests();
            t.ShouldExecuteWithNewNForEachRow();
            t.ShouldGenerateValuesAndInsertStatementsForAllTables();
            t.ShouldExecuteOnlyOnCondition_EqualOrGreaterThan();

            Console.WriteLine("Done");
            Console.ReadKey();

        }

        //private static void TestIfEqual(ExecutionItemCollection original, ExecutionItemCollection loaded)
        //{
        //    if (Assert(original.Count == loaded.Count, "Not same amount of exec items"))
        //    {
        //        for (int i = 0; i < original.Count; i++)
        //        {
        //            Assert(original[i].Description == loaded[i].Description, "description does not match");
        //            Assert(original[i].Order == loaded[i].Order, "Order does not match");

        //            Assert(original[i].TargetTable.TableName == loaded[i].TargetTable.TableName, "Target table does not match");
        //            Assert(original[i].TargetTable.TableSchema == loaded[i].TargetTable.TableSchema, "Target table TableSchema does not match");
        //            if (Assert(original[i].TargetTable.Columns.Count == loaded[i].TargetTable.Columns.Count, "Column count does not match"))
        //            {
        //                for (int j = 0; j < original[i].TargetTable.Columns.Count; j++)
        //                {
        //                    ColumnEntity loadedColumn = loaded[i].TargetTable.Columns[j];
        //                    ColumnEntity originalColumn = original[i].TargetTable.Columns[j];

        //                    Assert(originalColumn.ColumnName == loadedColumn.ColumnName, "Column names does not match");
        //                    Assert(originalColumn.ColumnDataType == loadedColumn.ColumnDataType, "ColumnDataType does not match");
        //                    Assert(originalColumn.IsForeignKey == loadedColumn.IsForeignKey, "IsForeignKey does not match");

        //                    Assert(originalColumn.IsIdentity == loadedColumn.IsIdentity, "IsIdentity does not match");
        //                    Assert(originalColumn.IsNotIdentity == loadedColumn.IsNotIdentity, "IsNotIdentity does not match");
        //                    Assert(originalColumn.OrdinalPosition == loadedColumn.OrdinalPosition, "OrdinalPosition does not match");

        //                    //Assert(list[i].TargetTable.Columns[j].PossibleGenerators == loaded[i].TargetTable.Columns[j].PossibleGenerators, "PossibleGenerators does not match");
        //                    Assert(originalColumn.PossibleGenerators.Count == loadedColumn.PossibleGenerators.Count, "PossibleGenerators count is not equal");


        //                    Assert(originalColumn.Generator.GeneratorName == loadedColumn.Generator.GeneratorName, "Generator does not matchdoes");
        //                    Assert(originalColumn.Generator.GeneratorParameters.Count == loadedColumn.Generator.GeneratorParameters.Count, "GeneratorParameters does not match");
        //                    for (int ii = 0; ii < originalColumn.Generator.GeneratorParameters.Count; ii++)
        //                    {
        //                        Assert(originalColumn.Generator.GeneratorParameters[ii].ParameterName == loadedColumn.Generator.GeneratorParameters[ii].ParameterName, "Parameter name not equal");
        //                        if(!Assert(originalColumn.Generator.GeneratorParameters[ii].Value.ToString() == loadedColumn.Generator.GeneratorParameters[ii].Value.ToString(), "Value not equal"))
        //                            Console.WriteLine("{0} - {1}", originalColumn.Generator.GeneratorParameters[ii].Value, loadedColumn.Generator.GeneratorParameters[ii].Value);
                                
        //                        Assert(originalColumn.Generator.GeneratorParameters[ii].IsWriteEnabled == loadedColumn.Generator.GeneratorParameters[ii].IsWriteEnabled, "IsWriteEnabled not equal");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        private static bool Assert(bool p, string message)
        {
            if (!p)
                Console.WriteLine(message);
            return p;
        }

        //private static ExecutionItemCollection SetupExecutionItems(TableEntity table)
        //{
        //    ExecutionItemCollection list = new ExecutionItemCollection();
        //    ExecutionItem item = new ExecutionItem(table);
        //    item.RepeatCount = 10;
        //    item.TruncateBeforeExecution = false;
        //    ExecutionItem item2 = new ExecutionItem(table);
        //    item2.RepeatCount = 1;
        //    list.Add(item);
        //    list.Add(item2);
        //    //list.Save(@"c:\temp\test.xml");
        //    return list;
        //}

      

        private static string Connection()
        {
            return "Data Source=localhost;Initial Catalog=AdventureWorks;Integrated Security=True";
        }
    }
}
