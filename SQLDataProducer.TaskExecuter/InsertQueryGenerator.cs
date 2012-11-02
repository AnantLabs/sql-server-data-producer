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

using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDataProducer.Entities;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.ExecutionEntities;

namespace SQLDataProducer.TaskExecuter
{
    public class InsertQueryGenerator
    {
        //    public string GenerateQueryForExecutionItems(ExecutionItemCollection executionItems)
        //    {
        //        StringBuilder sb = new StringBuilder();

        //        sb.AppendLine("SET NOCOUNT ON");
        //        sb.AppendLine("SET XACT_ABORT ON");
        //        sb.AppendLine();
        //        sb.AppendLine("BEGIN TRANSACTION");
        //        sb.AppendLine();

        //        sb.AppendLine("DECLARE @N bigint = <N>");
        //        sb.AppendLine();


        //        foreach (var item in executionItems)
        //        {
        //            // Skip tables with no columns
        //            if (item.TargetTable.Columns.Count == 0)
        //            {
        //                continue;
        //            }

        //            bool hasIdentity = item.TargetTable.Columns.Any(col => col.IsIdentity);

        //            sb.AppendFormat("-- INSERT item {0} - {0}", item.Order, item.Description);

        //            if (item.ExecutionCondition != ExecutionConditions.None)
        //            {
        //                sb.AppendLine();
        //                sb.AppendFormat("IF @N {0} BEGIN", item.ExecutionCondition.ToCompareString(item.ExecutionConditionValue));
        //            }

        //            // Generate for each column
        //            sb.Append(GenerateInsertStatement(item));

        //            if (item.ExecutionCondition != ExecutionConditions.None)
        //            {
        //                sb.AppendLine("END");
        //                sb.AppendLine();
        //            }

        //            if (hasIdentity)
        //            {
        //                sb.AppendLine();
        //                sb.AppendFormat("DECLARE @i{0}_IDENTITY BIGINT = SCOPE_IDENTITY()", item.Order);
        //            }

        //            sb.AppendLine();
        //            sb.AppendFormat("-- DONE insert item {0}", item.Order);
        //            sb.AppendLine();
        //            sb.AppendLine();


        //        }
        //        sb.AppendLine("COMMIT");
        //        sb.AppendLine();

        //        return sb.ToString();
        //    }

        //    private string GenerateInsertStatement(ExecutionItem item)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        sb.AppendLine();

        //        sb.AppendFormat("INSERT {0}.{1} (", item.TargetTable.TableSchema, item.TargetTable.TableName);
        //        sb.AppendLine();

        //        for (int i = 0; i < item.TargetTable.Columns.Count; i++)
        //        {
        //            var col = item.TargetTable.Columns[i];

        //            if (col.IsIdentity)
        //                continue;

        //            sb.AppendFormat("\t{0}", col.ColumnName);
        //            sb.Append(i == item.TargetTable.Columns.Count - 1 ? string.Empty : ", ");
        //            sb.AppendLine();
        //        }

        //        sb.Append(")");
        //        sb.AppendLine();

        //        sb.AppendFormat("<{0}VALUES>", item.Order);

        //        return sb.ToString();
        //    }


        //    /// <summary>
        //    /// Generate the declaration section of the sqlquery, including the values for the variables
        //    /// </summary>
        //    /// <param name="n">The serial number to use when creating the values for the variables</param>
        //    /// <param name="execItems">the executionItems to be included in the variable declarations</param>
        //    /// <returns></returns>
        //    public string GenerateFinalQuery(string baseQuery, ExecutionItemCollection execItems, long n, System.Func<long> getN)
        //    {
        //        string modified = baseQuery.Clone() as string;

        //        modified = modified.Replace("<N>", n.ToString());

        //        // Skip tables with no columns
        //        foreach (var item in execItems.Where(x => x.TargetTable.Columns.Count > 0))
        //        {

        //            StringBuilder sb = new StringBuilder();

        //            sb.AppendFormat("\t-- Item {0}, {1}.{2}", item.Order, item.TargetTable.TableSchema, item.TargetTable.TableName);
        //            sb.AppendLine();
        //            sb.AppendLine("VALUES");
        //            for (int rep = 1; rep <= item.RepeatCount; rep++)
        //            {
        //                long rowGenerationNumber = getN();
        //                sb.Append("\t");
        //                sb.Append("(");

        //                // Generate all the values for all the columns using this generation number
        //                item.TargetTable.GenerateValuesForColumns(rowGenerationNumber);

        //                for (int i = 0; i < item.TargetTable.Columns.Count; i++)
        //                {
        //                    var col = item.TargetTable.Columns[i];

        //                    if (col.IsIdentity)
        //                        continue;
        //                    // Write the generated values to the script
        //                    sb.Append(col.PreviouslyGeneratedValue);
        //                    sb.Append(i == item.TargetTable.Columns.Count - 1 ? string.Empty : ", ");
        //                }

        //                sb.Append(")");
        //                sb.Append(item.RepeatCount == rep ? string.Empty : ", ");
        //                sb.AppendLine();
        //            }
        //            // Find the place in the big string where this set of values belong and replace the placeholder with the actual values.
        //            string placeholderIdentifier = string.Format("<{0}VALUES>", item.Order);
        //            modified = modified.Replace(placeholderIdentifier, sb.ToString());
        //        }

        //        return modified;
        //    }
        //}
    }
}