//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SQLDataProducer.Entities.ExecutionEntities;
//using SQLDataProducer.Entities;
//using SQLDataProducer.ContinuousInsertion.DataAccess;
//using System.Data.Common;

//namespace SQLDataProducer.ContinuousInsertion
//{
//    public class ContinuousInserter
//    {
//        internal ContinuousInserter(ExecutionItemCollection items, Func<long> getN, SetCounter _rowInsertCounter, long executionCount)
//        {
//        }

//        DbCommand _cmd;

//        public void DoOneExecution()
//        {
//            var adhd = new QueryExecutor(ConnectionString);
//            var builders = Prepare(items);
//            foreach (var b in builders)
//            {
//                if (b.ExecuteItem.ShouldExecuteForThisN(executionCount))
//                {
//                    b.GenerateValues(getN);
//                    adhd.ExecuteNonQuery(b.InsertStatement, b.Parameters);
//                    _rowInsertCounter.Add(b.ExecuteItem.RepeatCount);
//                }
//            }
//        }
//    }
//}
