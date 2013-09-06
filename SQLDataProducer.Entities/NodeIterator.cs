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


using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.ExecutionEntities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Entities
{
    public class NodeIterator
    {
        private ExecutionNode executionNode;

        public NodeIterator(ExecutionEntities.ExecutionNode node)
        {
            ValidateUtil.ValidateNotNull(node, "node");

            this.executionNode = node;
        }

        //public IEnumerable<ExecutionNode> GetNodesRecursive()
        //{
        //    return GetNodes(executionNode);
        //}

        //private static IEnumerable<ExecutionNode> GetNodes(ExecutionNode node)
        //{
        //    if (node.Level == 1)
        //        yield return node;

        //    if (node.Children.Count > 0)
        //    {
        //        foreach (var child in node.Children)
        //        {
        //            yield return child;
        //            foreach (var m in GetNodes(child))
        //            {
        //                yield return m;
        //            }
        //        }
        //    }
        //}

        public IEnumerable<TableEntity> GetTablesRecursive()
        {
            return GetOrderedTables(executionNode);
        }

        private IEnumerable<TableEntity> GetOrderedTables(ExecutionNode node)
        {
            for (int i = 0; i < node.RepeatCount; i++)
            {
                foreach (var table in node.Tables)
                {
                    yield return table;
                }

                if (node.Children.Count > 0)
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
