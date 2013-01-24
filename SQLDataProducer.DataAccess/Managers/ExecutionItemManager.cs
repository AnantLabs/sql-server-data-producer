// Copyright 2012-2013 Peter Henell

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
using System.Xml;
using System.Xml.Linq;

namespace SQLDataProducer.DataAccess.Factories
{
    public static class ExecutionItemManager
    {
        public static IEnumerable<ExecutionItem> GetExecutionItemsFromTables(IEnumerable<TableEntity> tables, TableEntityDataAccess tda)
        {
            // Clone the selected table so that each generation of that table is configurable uniquely
            foreach (var table in tables)
            {
                yield return CloneFromTable(table, tda);
            }
        }

        public static ExecutionItem CloneFromTable(TableEntity table, TableEntityDataAccess tda)
        {
            TableEntity clonedTable = tda.CloneTable(table);
            return new ExecutionItem(clonedTable);
        }

        public static void Save(ExecutionItemCollection execItems, string fileName)
        {
            
            using (XmlWriter xmlWriter = XmlTextWriter.Create(fileName))
            {
                execItems.WriteXml(xmlWriter);
            }
        }
        public static ExecutionItemCollection Load(string fileName, TableEntityDataAccess tda)
        {
            return Load(XDocument.Load(fileName), tda);
        }
        public static ExecutionItemCollection Load(XDocument doc, TableEntityDataAccess tda)
        {
            // The logic here is that we only load(and save) relevant configuration.
            // When we load, we will only load up 
            //      The execution items and their tables.
            //      The columns and their selected generator and it's parameters  
            //      
            // We then get the correct information from the database and applies the loaded changes to the tables and columns retrieved from the database.
            // By doing this we will get all the columns correctly from the database, and we only need to store the configured parameter values in the savefile.
            ExecutionItemCollection loadedExecCollection = new ExecutionItemCollection();

            ExecutionItemCollection completeExecCollection = new ExecutionItemCollection();
            loadedExecCollection.ReadXml(doc);
            
            foreach (var loadedExec in loadedExecCollection)
            {
                TableEntity table = tda.GetTableAndColumns(loadedExec.TargetTable.TableSchema, loadedExec.TargetTable.TableName);
                foreach (var newColumn in table.Columns)
                {
                    foreach (var loadedColumn in loadedExec.TargetTable.Columns)
                    {
                        if (newColumn.ColumnName == loadedColumn.ColumnName)
                        {
                            newColumn.Generator = newColumn.PossibleGenerators.Where(gen => gen.GeneratorName == loadedColumn.Generator.GeneratorName).Single();
                            // If this column is foreign key generator then use the foreign keys we just read from the DB
                            if (newColumn.Generator.GeneratorName.ToLower().Contains("foreign"))
                                continue;

                            newColumn.Generator.SetGeneratorParameters(loadedColumn.Generator.GeneratorParameters);
                        }
                        
                        //// Set the parameters for all the possible generators
                        //foreach (var n in newColumn.PossibleGenerators)
                        //{
                        //    var l = loadedColumn.PossibleGenerators.Where(x => x.GeneratorName == n.GeneratorName).Single();
                        //    n.SetGeneratorParameters(l.GeneratorParameters);
                        //}
                    }
                    
                }

                ExecutionItem ei = new ExecutionItem(table, loadedExec.Description);
                ei.RepeatCount = loadedExec.RepeatCount;
                ei.ExecutionCondition = loadedExec.ExecutionCondition;
                ei.ExecutionConditionValue = loadedExec.ExecutionConditionValue;
                ei.TruncateBeforeExecution = loadedExec.TruncateBeforeExecution;
                ei.UseIdentityInsert = loadedExec.UseIdentityInsert;
                completeExecCollection.Add(ei);
            }

            return completeExecCollection;
        }
    }
}
