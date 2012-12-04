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
using SQLDataProducer.RandomTests;
using SQLDataProducer.ContinuousInsertion.Builders;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.DataAccess;
using SQLDataProducer.Entities.DatabaseEntities;
using System.IO;
using System.Linq;
using Generators = SQLDataProducer.Entities.Generators;
using SQLDataProducer.RandomTests.SerializationTests;

namespace TestConsoleApplication
{
    class Program
    {

        static void Main(string[] args)
        {
            //var t = new GeneratorTests();
            //t.ShouldGenerate_Long_RandomInt();

            //var t = new RandomTests();
            //t.ShouldExecuteOnlyOnCondition_EQUALTO();
            var et = new DatabaseEntitityTests();
            et.ShouldBeAbleToCompareExecutionItems();
            et.ShouldBeAbleToCompareTables();
            et.ShouldBeAbletoCloneExecutionItem();

            var t = new SaveLoadTests();
            t.ShouldBeAbleToSaveAndLoadExecutionItemCollectionUsingManager();

            //TableEntityDataAccess tda = new TableEntityDataAccess(Connection());

            //TableEntity table = tda.GetTableAndColumns("Person", "Address");
            //TableEntity table2 = tda.GetTableAndColumns("Person", "NewPerson");
            //ExecutionItem ei = new ExecutionItem(table);
            //ExecutionItem ei2 = new ExecutionItem(table2);
            //ei.RepeatCount = 10;
            //var items = new ExecutionItemCollection();
            //items.Add(ei);
            //items.Add(ei2);
            //items.Add(GetSQLGetDateExecutionItem());

            //long j = 0;
            //var fileName = @"c:\temp\repeater\test.sql";
            //File.Delete(fileName);
            //using (StreamWriter writer = new StreamWriter(fileName))
            //{
            //    var b = new FullQueryInsertStatementBuilder(items);
            //    writer.Write(b.GenerateFullStatement(() => { return j++; }));
            //}

            //GetAllTablesEI();

            Console.WriteLine("Done");
            Console.ReadKey();

        }

        //private static void GetAllTablesEI()
        //{
        //    var tda = new TableEntityDataAccess(Connection());
        //    var tables = tda.GetAllTablesAndColumns();
        //    ExecutionItemCollection col = new ExecutionItemCollection();
        //    foreach (var t in tables)
        //    {
        //        col.Add(new ExecutionItem(t));
        //    }

        //    long j = 0;
        //    var fileName = @"c:\temp\repeater\testBig.sql";
        //    File.Delete(fileName);
        //    using (StreamWriter writer = new StreamWriter(fileName))
        //    {
        //        //var b = new FullQueryInsertStatementBuilder(col);
        //        writer.Write(FullQueryInsertStatementBuilder.GenerateFullStatement(() => { return j++; }, col));
        //    }

        //}

        //private static ExecutionItem GetSQLGetDateExecutionItem()
        //{
        //    var tda = new TableEntityDataAccess(Connection());
        //    var table = tda.GetTableAndColumns("Person", "NewPerson");
            
        //    var ei = new ExecutionItem(table);
        //    ei.RepeatCount = 1;
            

        //    var col = table.Columns.Where(x => x.ColumnDataType.Raw == "datetime").FirstOrDefault();
        //    col.Generator = col.PossibleGenerators.Where(g => g.GeneratorName == Generators.Generator.GENERATOR_SQLGetDate).FirstOrDefault();


        //    var bigintCol = table.Columns.Where(x => x.ColumnDataType.DBType == System.Data.SqlDbType.BigInt).FirstOrDefault();
        //    bigintCol.Generator = bigintCol.PossibleGenerators.Where(g => g.GeneratorName == Generators.Generator.GENERATOR_IdentityFromPreviousItem).FirstOrDefault();
        //    bigintCol.Generator.GeneratorParameters[0].Value = 1;

        //    return ei;
        //}

        //private static string Connection()
        //{
        //    return "Data Source=localhost;Initial Catalog=AdventureWorks;Integrated Security=True";
        //}
    }
}
