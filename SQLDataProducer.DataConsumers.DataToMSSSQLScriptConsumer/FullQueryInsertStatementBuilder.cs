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
using SQLDataProducer.Entities.DataEntities.Collections;

namespace SQLDataProducer.DataConsumers.DataToMSSSQLScriptConsumer
{
    public static class FullQueryInsertStatementBuilder
    {
//        /// <summary>
//        /// Generate the full SQL script that can be executed outside the application and result in the same rows as it would have inside the application.
//        /// </summary>
//        /// <returns>The full sql query that will insert all the rows and values</returns>
//        public static string GenerateFullStatement(DataRowSet ei)
//        {
//            StringBuilder sb = new StringBuilder();
//            sb.AppendLine("DECLARE @Identity_output bigint");
//            sb.AppendLine(@"SET NOCOUNT ON
//SET XACT_ABORT ON
//
//BEGIN TRANSACTION");

//            //// set the values
//            //foreach (var ei in items)
//            //{
//                ExecutionItemQueryBuilder.AppendVariables(ei, sb);
//                TableQueryBuilder.AppendSqlScriptPartOfStatement(ei, sb);
//                ExecutionItemQueryBuilder.AppendInsertPartOfStatement(ei, sb);
//                ExecutionItemQueryBuilder.AppendValuePartOfInsertStatement(ei, sb);
//            //}

//            sb.AppendLine(@"
//COMMIT
//GO");
//            return sb.ToString();
//        }

//        public static string OneExecutionToString(ExecutionItemCollection execItems, Func<long> getN, SetCounter _rowInsertCounter)
//        {
//            //FullQueryInsertStatementBuilder builder = new FullQueryInsertStatementBuilder(execItems);
//            return FullQueryInsertStatementBuilder.GenerateFullStatement(getN, execItems);
//        }
    }
}
