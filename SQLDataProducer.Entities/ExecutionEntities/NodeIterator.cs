// Copyright 2012-2014 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.


using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.ExecutionEntities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Entities.ExecutionEntities
{
    public class NodeIterator : IDisposable
    {
        private ExecutionNode executionNode;
        private System.Threading.CancellationTokenSource cancelationToken;


        public NodeIterator(ExecutionEntities.ExecutionNode node)
        {
            ValidateUtil.ValidateNotNull(node, "node");

            cancelationToken = new System.Threading.CancellationTokenSource();
            this.executionNode = node;
        }

        /// <summary>
        /// Gets all tables recursively, in the order of the nodes, repeated according to the node settings
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TableEntity> GetTablesRecursive()
        {
            return GetOrderedTables(executionNode);
        }

        /// <summary>
        /// Get tables,
        ///     in this node, repeat X times: Return my tables then return the tables of my children
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private IEnumerable<TableEntity> GetOrderedTables(ExecutionNode node)
        {
            for (int i = 0; i < node.RepeatCount; i++)
            {
                if (cancelationToken.IsCancellationRequested)
                    break;

                foreach (var table in node.Tables)
                {
                    if (cancelationToken.IsCancellationRequested)
                        break;
                    
                    yield return table;
                }

                if (node.Children.Any( x => true))
                {
                    foreach (var child in node.Children)
                    {
                        foreach (var m in GetOrderedTables(child))
                        {
                            yield return m;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// get the total expected insert count, in order to predict progress
        /// </summary>
        /// <returns>the number of expected inserts</returns>
        public int GetExpectedInsertCount()
        {
            return GetTablesRecursive().Count();
        }

        public void Dispose()
        {
            if (cancelationToken != null)
            {
                cancelationToken.Dispose();
            }
        }

        public void Cancel()
        {
            cancelationToken.Cancel();
        }
    }
   
   

    //public static class NodeExtensions
    //{
    //    /// <summary>
    //    /// http://stackoverflow.com/questions/2012274/how-to-unroll-a-recursive-structure
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="subjects"></param>
    //    /// <param name="selector"></param>
    //    /// <returns></returns>
    //    public static IEnumerable<T> SelectRecursive<T>(this IEnumerable<T> subjects,
    //        Func<T, IEnumerable<T>> selector)
    //    {
    //        if (subjects == null)
    //        {
    //            yield break;
    //        }

    //        Queue<T> stillToProcess = new Queue<T>(subjects);

    //        while (stillToProcess.Count > 0)
    //        {
    //            T item = stillToProcess.Dequeue();
    //            yield return item;
    //            foreach (T child in selector(item))
    //            {
    //                stillToProcess.Enqueue(child);
    //            }
    //        }
    //    }

    //    public static IEnumerable<T> SelectRecursiveKeepOrder<T>(this IEnumerable<T> subjects, Func<T, IEnumerable<T>> selector)
    //    {
    //        if (subjects == null)
    //            yield break;

    //        var stack = new Stack<IEnumerator<T>>();

    //        stack.Push(subjects.GetEnumerator());

    //        while (stack.Count > 0)
    //        {
    //            var en = stack.Peek();
    //            if (en.MoveNext())
    //            {
    //                var subject = en.Current;
    //                yield return subject;

    //                stack.Push(selector(subject).GetEnumerator());
    //            }
    //            else
    //            {
    //                stack.Pop();
    //            }
    //        }
    //    }
    //}
}
