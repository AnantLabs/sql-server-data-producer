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
using NUnit.Framework;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DatabaseEntities.Factories;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.DataAccess;
using SQLDataProducer.ContinuousInsertion.Builders;
using SQLDataProducer.Entities.DatabaseEntities.Collections;
using SQLDataProducer.ContinuousInsertion;
using SQLDataProducer.DataAccess.Factories;


namespace SQLDataProducer.Entities.Tests
{
    public class GeneratorTests
    {
        [TestFixture]
        public class DatabaseEntitityTests
        {
            //IEnumerable<Generators.Generator> allGens = Generators.Generator.GetDateTimeGenerators()
            //                                    .Concat(Generators.Generator.GetDecimalGenerators())
            //                                    .Concat(Generators.Generator.GetGeneratorsForBigInt())
            //                                    .Concat(Generators.Generator.GetGeneratorsForBit())
            //                                    .Concat(Generators.Generator.GetGeneratorsForInt())
            //                                    .Concat(Generators.Generator.GetGeneratorsForSmallInt())
            //                                    .Concat(Generators.Generator.GetGeneratorsForTinyInt())
            //                                    .Concat(Generators.Generator.GetGUIDGenerators())
            //                                    .Concat(Generators.Generator.GetStringGenerators(1))
            //                                    .Concat(new Generators.Generator[] { Generators.Generator.CreateNULLValueGenerator() });

        }

        [Test]
        public void SmallTest()
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());

            TableEntityCollection tables = tda.GetAllTablesAndColumns();

            foreach (TableEntity table in tables)
            {
                ExecutionItem ie = new ExecutionItem(table);
                ie.RepeatCount = 3;
                TableEntityInsertStatementBuilder builder = new TableEntityInsertStatementBuilder(ie);
                int i = 1;
                builder.GenerateValues(() => i++);

                //foreach (var p in builder.Parameters)
                //{
                //    Console.WriteLine(p.Value + ":" + p.Value.Value);
                //}
                //Console.WriteLine(builder.InsertStatement);

                Console.WriteLine();
                Console.WriteLine(builder.GenerateFullStatement());
                Console.WriteLine("GO");
            }
        }

        [Test]
        public void ExecutionManagerDoOneExecution()
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Connection());
            TableEntityCollection tables = tda.GetAllTablesAndColumns();
            ContinuousInsertionManager manager = new ContinuousInsertionManager(Connection());
            ExecutionItemCollection items = new ExecutionItemCollection();
            items.AddRange(new ExecutionItemFactory(Connection()).GetExecutionItemsFromTables(tables));
            
            int i = 1;
            manager.DoOneExecution(items, () => i++);
        }

        private static string Connection()
        {
            return "Data Source=localhost;Initial Catalog=AdventureWorks;Integrated Security=True";
        }
    }
}
