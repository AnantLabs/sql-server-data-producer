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
using System.Data.Common;
using SQLDataProducer.Entities.DatabaseEntities;
//using SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer.Builders.EntityBuilders;
using SQLDataProducer.Entities.DataEntities.Collections;

namespace SQLDataProducer.DataConsumers.DataToMSSSQLInsertionConsumer.Builders.EntityBuilders
{
    static class ExecutionItemQueryBuilder
    {
        /// <summary>
        /// Generate the INSERT-part of the statement, example <example>INSERT schemaName.TableName(c1, c2)</example>
        /// </summary>
        /// <param name="sb">the stringbuilder to append the insert statement to</param>
        public static void AppendInsertPartOfStatement(DataRowSet ei, StringBuilder sb)
        {
            sb.AppendLine();
            //sb.AppendLine(string.Format("-- {0}", ei.TargetTable.TableName));

            TableQueryBuilder.AppendInsertPartForTable(ei, sb);

            sb.AppendLine();
        }


        public static void AppendValuePartOfInsertStatement(DataRowSet ei, StringBuilder sb)
        {
            sb.AppendLine();
            sb.AppendLine("VALUES");
            for (int rep = 1; rep <= ei.Count; rep++)
            {
                sb.Append("\t");
                sb.Append("(");

                TableQueryBuilder.CreateValuesPartForTable(ei.TargetTable, sb, rep);

                sb.Append(")");
                sb.Append(ei.Count == rep ? ";" : ", ");
                sb.AppendLine();
            }
            // If the table have idenitity column then we shuold store that in a variable
            if (ei.TargetTable.HasIdentityColumn)
            {
                var identityVariableName = string.Format("@Identity_i{0}", ei.Order);
                sb.AppendFormat("DECLARE {0} bigint = SCOPE_IDENTITY();", identityVariableName);
                sb.AppendLine();
                sb.AppendFormat("select @Identity_output  = {0};", identityVariableName);
                sb.AppendLine();
            }
            sb.AppendLine();
        }

        

        /// <summary>
        /// Set values for the parameters. The parameters must already be generated and should have been in the init() function
        /// </summary>
        /// <param name="getN">will be called once per row</param>
        public static void AppendVariables(DataRowSet ei, StringBuilder sb)
        {
            sb.AppendLine();
            for (int rep = 1; rep <= ei.Count; rep++)
            {
                TableQueryBuilder.AppendVariablesForTable(ei, sb, rep);
            }
        }
    }
}
