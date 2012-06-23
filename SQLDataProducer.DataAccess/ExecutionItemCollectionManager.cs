using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDataProducer.Entities.ExecutionEntities;
using System.Xml;
using SQLDataProducer.DatabaseEntities.Entities;

namespace SQLDataProducer.DataAccess
{
    public class ExecutionItemCollectionManager
    {
        public void Save(ExecutionItemCollection execItems, string fileName)
        {
            //System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(ExecutionItemCollection));
            using (XmlWriter xmlWriter = XmlTextWriter.Create(fileName))
            {
                execItems.WriteXml(xmlWriter);
            }
            
        }

        public ExecutionItemCollection Load(string fileName, string connectionString)
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
            using (XmlReader reader = XmlReader.Create(fileName))
            {
                loadedExecCollection.ReadXml(reader);
            }
            
            TableEntityDataAccess tda = new TableEntityDataAccess(connectionString);
            foreach (var item in loadedExecCollection)
            {
                TableEntity table = tda.GetTableAndColumns(item.TargetTable.TableSchema, item.TargetTable.TableName);
                foreach (var newColumn in table.Columns)
                {
                    foreach (var c2 in item.TargetTable.Columns)
                    {
                        if (newColumn.ColumnName == c2.ColumnName)
                        {
                            newColumn.Generator = newColumn.PossibleGenerators.Where(gen => gen.GeneratorName == c2.Generator.GeneratorName).Single();
                            // If this column is foreign key generator then use the foreign keys we just read from the DB
                            if (newColumn.Generator.GeneratorName.ToLower().Contains("foreign"))
                                continue;

                            newColumn.Generator.SetGeneratorParameters(c2.Generator.GeneratorParameters);
                        }
                    }
                }
                ExecutionItem ei = new ExecutionItem(table, item.Order);
                completeExecCollection.Add(ei);
            }

            return completeExecCollection;
        }
    }
}
