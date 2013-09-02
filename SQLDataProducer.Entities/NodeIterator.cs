using SQLDataProducer.Entities.ExecutionEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Entities
{
    public class NodeIterator
    {
        private ExecutionNode executionNode;

        public NodeIterator(ExecutionEntities.ExecutionNode node)
        {
            this.executionNode = node;
        }

        public IEnumerable<ExecutionNode> GetNodesRecursive()
        {
            return GetNodes(executionNode);
        }

        private static IEnumerable<ExecutionNode> GetNodes(ExecutionNode node)
        {
            if (node == null)
                yield break;

            if (node.Level == 1)
                yield return node;

            if (node.Children.Count > 0)
            {
                foreach (var child in node.Children)
                {
                    yield return child;
                    foreach (var m in GetNodes(child))
                    {
                        yield return m;
                    }
                }
            }
        }
       
    }

    public static class NodeExtensions
    {
        /// <summary>
        /// http://stackoverflow.com/questions/2012274/how-to-unroll-a-recursive-structure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subjects"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<T> SelectRecursive<T>(this IEnumerable<T> subjects,
            Func<T, IEnumerable<T>> selector)
        {
            if (subjects == null)
            {
                yield break;
            }

            Queue<T> stillToProcess = new Queue<T>(subjects);

            while (stillToProcess.Count > 0)
            {
                T item = stillToProcess.Dequeue();
                yield return item;
                foreach (T child in selector(item))
                {
                    stillToProcess.Enqueue(child);
                }
            }
        }

        public static IEnumerable<T> SelectRecursiveKeepOrder<T>(this IEnumerable<T> subjects, Func<T, IEnumerable<T>> selector)
        {
            if (subjects == null)
                yield break;

            var stack = new Stack<IEnumerator<T>>();

            stack.Push(subjects.GetEnumerator());

            while (stack.Count > 0)
            {
                var en = stack.Peek();
                if (en.MoveNext())
                {
                    var subject = en.Current;
                    yield return subject;

                    stack.Push(selector(subject).GetEnumerator());
                }
                else
                {
                    stack.Pop();
                }
            }
        }
    }
}
