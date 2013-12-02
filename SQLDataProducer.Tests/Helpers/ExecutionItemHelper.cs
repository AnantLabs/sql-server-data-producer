//// Copyright 2012-2013 Peter Henell

////   Licensed under the Apache License, Version 2.0 (the "License");
////   you may not use this file except in compliance with the License.
////   You may obtain a copy of the License at

////       http://www.apache.org/licenses/LICENSE-2.0

////   Unless required by applicable law or agreed to in writing, software
////   distributed under the License is distributed on an "AS IS" BASIS,
////   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
////   See the License for the specific language governing permissions and
////   limitations under the License.

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SQLDataProducer.Entities.DatabaseEntities;
//using SQLDataProducer.Entities.ExecutionEntities;
//using SQLDataProducer.Entities.DatabaseEntities.Factories;
//using SQLDataProducer.Entities;
//using SQLDataProducer.DataAccess;

//namespace SQLDataProducer.Tests.Helpers
//{

//    public static class ExecutionItemHelper
//    {
//        public static ExecutionItemCollection GetFakeExecutionItemCollection()
//        {
//            TableEntity table = CreateTableWithIdenitityAnd5Columns("Person", "Address");
//            TableEntity table2 = CreateTableWithIdenitityAnd5Columns("Person", "NewPerson");
//            ExecutionNode ei = new ExecutionNode(table);
//            ExecutionNode ei2 = new ExecutionNode(table2);
//            ei.RepeatCount = 10;
//            var items = new ExecutionItemCollection();
//            items.Add(ei);
//            items.Add(ei2);
//            return items;
//        }

//        public static TableEntity CreateTableWithIdenitityAnd5Columns(string schemaName, string tableName)
//        {
//            var table = new TableEntity(schemaName, tableName);
//            table.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("id", new ColumnDataTypeDefinition("int", false), true, 1, false, string.Empty, null));
//            table.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("name", new ColumnDataTypeDefinition("varchar(500)", false), false, 2, false, string.Empty, null));
//            table.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("created", new ColumnDataTypeDefinition("datetime", false), false, 3, false, string.Empty, null));
//            table.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("enabled", new ColumnDataTypeDefinition("bit", false), false, 4, false, string.Empty, null));
//            table.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("description", new ColumnDataTypeDefinition("varchar(2000)", true), false, 5, false, string.Empty, null));

//            return table;
//        }

//        /// <summary>
//        /// Creates a table with 5 columns with NO identity
//        /// </summary>
//        /// <param name="schemaName"></param>
//        /// <param name="tableName"></param>
//        /// <returns></returns>
//        public static TableEntity CreateTableWith5Columns(string schemaName, string tableName)
//        {
//            var table = new TableEntity(schemaName, tableName);
//            table.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("id", new ColumnDataTypeDefinition("int", false), false, 1, false, string.Empty, null));
//            table.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("name", new ColumnDataTypeDefinition("varchar(500)", false), false, 2, false, string.Empty, null));
//            table.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("created", new ColumnDataTypeDefinition("datetime", false), false, 3, false, string.Empty, null));
//            table.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("enabled", new ColumnDataTypeDefinition("bit", false), false, 4, false, string.Empty, null));
//            table.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("description", new ColumnDataTypeDefinition("varchar(2000)", true), false, 5, false, string.Empty, null));

//            return table;
//        }

//        public static TableEntity CreateTableAnd1DateTimeColumn(string schemaName, string tableName)
//        {
//            var table = new TableEntity(schemaName, tableName);
//            table.Columns.Add(DatabaseEntityFactory.CreateColumnEntity("created", new ColumnDataTypeDefinition("datetime", false), true, 1, false, string.Empty, null));
            
//            return table;
//        }


//        public static ExecutionItemCollection GetRealExecutionItemCollection(string connection)
//        {
//            var t1 = new TableEntityDataAccess(connection).GetTableAndColumns("Person", "Address");
//            var t2 = new TableEntityDataAccess(connection).GetTableAndColumns("Person", "NewPerson");

//            var t3 = new TableEntityDataAccess(connection).GetTableAndColumns("Person", "Address");
//            var t4 = new TableEntityDataAccess(connection).GetTableAndColumns("Person", "NewPerson");


//            ExecutionNode ei = new ExecutionNode(t1);
//            ExecutionNode ei2 = new ExecutionNode(t2);

//            ExecutionNode ei3 = new ExecutionNode(t3);
//            ExecutionNode ei4 = new ExecutionNode(t4);
//            SetNoneDefaultValuesOnItem(ei3);
//            SetNoneDefaultValuesOnItem(ei4);

//            ei.RepeatCount = 10;
//            var items = new ExecutionItemCollection();
//            items.Add(ei);
//            items.Add(ei2);
//            items.Add(ei3);
//            items.Add(ei4);
//            return items;
            
//        }

//        private static void SetNoneDefaultValuesOnItem(ExecutionNode ei)
//        {
//            ei.Description = "Peters exec item with none default values";
//            ei.ExecutionCondition = ExecutionConditions.GreaterThan;
//            ei.ExecutionConditionValue = 7;
//            ei.RepeatCount = 66;
//            ei.TruncateBeforeExecution = true;
//            ei.UseIdentityInsert = true;
//        }

//        //internal static void SetSomeParameters(ExecutionItemCollection execItems)
//        //{
//        //    var generatorParameters = execItems.SelectMany(x => x.TargetTable.Columns).SelectMany(y => y.PossibleGenerators).SelectMany(z => z.GeneratorParameters);

//        //    foreach (var parr in generatorParameters) 
//        //    {
//        //        parr.Value = "123987456";
//        //    }
//        //}
//    }
//}
