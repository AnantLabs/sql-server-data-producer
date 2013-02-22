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


using SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer.DataAccess;
using SQLDataProducer.DataConsumers.DataToMSSSQLScriptConsumer;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DataEntities.Collections;
using SQLDataProducer.Entities.ExecutionEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer
{
    public class InsertComsumer : IDataConsumer
    {
        

        private void RunTruncationOnExecutionItems(string connectionString, ExecutionItemCollection executionItems)
        {
            //
            throw new NotSupportedException("Move this method to the consumers");

            //if (executionItems.Any(e => e.TruncateBeforeExecution))
            //{
            //    AdhocDataAccess ahd = new AdhocDataAccess(connectionString);
            //    foreach (var item in executionItems.Where(x => x.TruncateBeforeExecution).Select(x => x.TargetTable).Distinct())
            //    {
            //        string sql = string.Format("DELETE {0}.{1};", item.TableSchema, item.TableName);
            //        ahd.ExecuteNonQuery(sql);
            //    }
            //}
        }

        private void RunPostScript(string connectionString, string postScript)
        {
            throw new NotSupportedException("Move this method to the consumers");

            //if (string.IsNullOrEmpty(postScript))
            //    return;
            //AdhocDataAccess adhd = new AdhocDataAccess(connectionString);
            //adhd.ExecuteNonQuery(postScript);
        }

        private void RunPrepare(string connectionString, string preScript)
        {
            throw new NotSupportedException("Move this method to the consumers");

            //if (string.IsNullOrEmpty(preScript))
            //    return;
            //AdhocDataAccess adhd = new AdhocDataAccess(connectionString);
            //adhd.ExecuteNonQuery(preScript);
        }

        //private ExecutionResult Execute(string connectionString, ExecutionItemCollection executionItems)
        //{
        //    //ExecutionTaskDelegate taskToExecute = _executor.CreateSQLTaskForExecutionItems(executionItems);

        //    return _executor.Execute();
        //}

        public string _connectionString { get; set; }

        public bool Init(string target)
        {
            _connectionString = target;
            _queryExecutor = new QueryExecutor(_connectionString);

            return true;
        }

        public ExecutionResult Consume(DataRowSet rows)
        {
            // starta transaction
            // set identity insert on om det är en column med identity insert generator
            // skapa insert statement med columner som finns i settet
            //      future feature: om det är anviget att använda default constraint value ange inte kolumnen -> nej, generera itne ens cellen för den kolumnen
            // fyll parametrar med värden från settet
            // kör insert
            // om det är en identity så ska vi plocka identity värdet som skickats tillbaka
            // commit

            DoOneExecution(rows);
            return null;
        }

        QueryExecutor _queryExecutor;

        private void DoOneExecution(DataRowSet ds)
        {
             
            var b = TableEntityInsertStatementBuilder.Create(ds);
            //b.SetParameterValues(ds);

            using (var tran = new TransactionScope())
            {
                
                if (!ds.TargetTable.HasIdentityColumn)
                    _queryExecutor.ExecuteNonQuery(b.InsertStatement, b.Parameters);
                else
                   ds.TargetTable.Columns.Where(c => c.IsIdentity)
                        .First()
                        .PreviouslyGeneratedValue = _queryExecutor.ExecuteIdentity(b.InsertStatement, b.Parameters);

                tran.Complete();
            }
        }
    
       

        public void CleanUp(List<string> datasetNames)
        {
        }

        public void PreAction(string action)
        {
        }

        public void PostAction(string action)
        {
            
        }

        public void Dispose()
        {
            _queryExecutor.Dispose();
        }
    }
}
