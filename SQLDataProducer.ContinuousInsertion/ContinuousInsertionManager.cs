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
//using System.Linq;
//using SQLDataProducer.Entities.ExecutionEntities;
//using SQLDataProducer.DataAccess;
////using SQLDataProducer.ContinuousInsertion.Builders;
//using System.Collections.ObjectModel;
//using SQLDataProducer.Entities;
//using System.Transactions;

//namespace SQLDataProducer.ContinuousInsertion
//{
//    public class ContinuousInsertionManager //: IDisposable
//    {
//        //private string ConnectionString { get; set; }
//        //QueryExecutor _queryExecutor;
//        //ObservableCollection<TableEntityInsertStatementBuilder> _builders;

//        //public ContinuousInsertionManager(string connectionString, ExecutionItemCollection items)
//        //{
//        //    ConnectionString = connectionString;
//        //    _queryExecutor = new QueryExecutor(ConnectionString);
//        //    _builders = TableEntityInsertStatementBuilder.CreateBuilders(items);
//        //}

//        //public void DoOneExecution(Func<long> getN, SetCounter _rowInsertCounter, long executionCount)
//        //{
//        //    using (var tran = new TransactionScope())
//        //    {
//        //        foreach (var b in _builders)
//        //        {
//        //            var dataRows = b.ExecuteItem.CreateData(getN);
//        //            b.SetParameterValues(dataRows);

//        //            if (!b.ExecuteItem.TargetTable.HasIdentityColumn)
//        //                _queryExecutor.ExecuteNonQuery(b.InsertStatement, b.Parameters);
//        //            else
//        //                b.ExecuteItem.TargetTable.Columns.Where(c => c.IsIdentity)
//        //                    .First()
//        //                    .PreviouslyGeneratedValue = _queryExecutor.ExecuteIdentity(b.InsertStatement, b.Parameters);

//        //            _rowInsertCounter.Add(b.ExecuteItem.RepeatCount);

//        //        }
//        //        tran.Complete();
//        //    }
//        //}

//        //public static string OneExecutionToString(ExecutionItemCollection execItems, Func<long> getN, SetCounter _rowInsertCounter)
//        //{
//        //    return FullQueryInsertStatementBuilder.GenerateFullStatement(getN, execItems);
//        //}

//        //public void Dispose()
//        //{
//        //    _builders.Clear();
//        //    _queryExecutor.Dispose();
//        //}
//    }
//}
