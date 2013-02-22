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
using SQLDataProducer.Entities.DatabaseEntities;

namespace SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer.Builders.Helpers
{
    static class QueryBuilderHelper
    {
        /// <summary>
        /// Create the parameter name used when the generator is Sql Query generator
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string GetSqlQueryParameterName(ColumnEntity c)
        {
            return GetParamName(1, c).Replace("@", "@c_");
        }

        public static string GetParamName(int rep, ColumnEntity col)
        {
            var ei = col.ParentTable.ParentExecutionItem;
            // TODO: string.Format is eating a lot of performance. Replace-fix
            // TODO: Replace is eating alot of performance, Replace-fix
            return string.Format("@{0}_{1}_{2}", col.ColumnName.Replace(" ", "").Replace(".", ""), ei.Order, rep);
        }
    }
}
