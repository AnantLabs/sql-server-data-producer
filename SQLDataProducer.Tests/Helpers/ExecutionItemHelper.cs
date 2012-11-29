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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.Entities.DatabaseEntities;

namespace SQLDataProducer.RandomTests.Helpers
{
    public static class ExecutionItemHelper
    {
        public static ExecutionItemCollection GetExecutionItemCollection()
        {
            TableEntity table = CreateTableAnd5Columns("Person", "Address");
            TableEntity table2 = CreateTableAnd5Columns("Person", "NewPerson");
            ExecutionItem ei = new ExecutionItem(table);
            ExecutionItem ei2 = new ExecutionItem(table2);
            ei.RepeatCount = 10;
            var items = new ExecutionItemCollection();
            items.Add(ei);
            items.Add(ei2);
            return items;
        }

        private static TableEntity CreateTableAnd5Columns(string schemaName, string tableName)
        {
            var table = new TableEntity(schemaName, tableName);
            table.Columns.Add(Entities.DatabaseEntities.Factories.DatabaseEntityFactory.CreateColumnEntity("id", new ColumnDataTypeDefinition("int", false), true, 1, false, null));
            table.Columns.Add(Entities.DatabaseEntities.Factories.DatabaseEntityFactory.CreateColumnEntity("name", new ColumnDataTypeDefinition("int", false), false, 2, false, null));
            table.Columns.Add(Entities.DatabaseEntities.Factories.DatabaseEntityFactory.CreateColumnEntity("created", new ColumnDataTypeDefinition("int", false), false, 3, false, null));
            table.Columns.Add(Entities.DatabaseEntities.Factories.DatabaseEntityFactory.CreateColumnEntity("enabled", new ColumnDataTypeDefinition("int", false), false, 4, false, null));
            table.Columns.Add(Entities.DatabaseEntities.Factories.DatabaseEntityFactory.CreateColumnEntity("description", new ColumnDataTypeDefinition("int", true), false, 5, false, null));

            return table;
        }
    }
}
