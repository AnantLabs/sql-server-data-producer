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


using SQLDataProducer.Entities.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataProducer.Entities
{
    public sealed class DataProducer
    {
        private NodeIterator it;
        
        //public static DataRowSet CreatePreview(NodeIterator it)
        //{
        //    long l = 0;
        //    Func<long> getN = new Func<long>(() => { return l++; });

        //    return ei.CreateData(getN, new SetCounter());
        //}

        public DataProducer(NodeIterator it)
        {
            this.it = it;
        }

      
        public IEnumerable<RowEntity> ProduceData(Func<long> getN, SetCounter insertCounter)
        {
            return null;
            //var dt = new DataRowSet();
            //dt.TargetTable = this.TargetTable;
            //dt.Order = this.Order;

            //long n = 0;
            //for (int i = 0; i < RepeatCount; i++)
            //{
            //    n = getN();
            //    //Console.WriteLine("Generating data with N = {0}", n);
            //    if (ShouldExecuteForThisN(n))
            //    {
            //        dt.Add(RowEntity.Create(TargetTable, n, i));
            //        insertCounter.Increment();
            //    }
            //}

            //return dt;
        }
    }
}
