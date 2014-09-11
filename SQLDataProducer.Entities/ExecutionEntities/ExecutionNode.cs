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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Entities.ExecutionEntities
{
    public class ExecutionNode : EntityBase, IEquatable<ExecutionNode>
    {
        private ExecutionNode _parent;
        public ExecutionNode Parent
        {
            get
            {
                return _parent;
            }
        }

        ObservableCollection<ExecutionNode> _children;
        public IEnumerable<ExecutionNode> Children
        {
            get
            {
                return _children;
            }
        }

        private readonly int _nodeId;
        public int NodeId
        {
            get
            {
                return _nodeId;
            }
        }

        
        public int Level
        {
            get
            {
                if (isRoot)
                    return 1;
                else
                    return Parent.Level + 1;
            }
        }

        private string _nodeName;
        public string NodeName
        {
            get
            {
                return _nodeName;
            }
            set
            {
                if (_nodeName != value)
                {
                    _nodeName = value;
                    OnPropertyChanged("NodeName");
                }
            }
        }

        bool _hasChildren;
        public bool HasChildren
        {
            get
            {
                return _hasChildren;
            }
            private set
            {
                if (_hasChildren != value)
                {
                    _hasChildren = value;
                    OnPropertyChanged("HasChildren");
                }
            }
        }

        ObservableCollection<TableEntity> _tables;
        public IEnumerable<TableEntity> Tables
        {
            get
            {
                return _tables;
            }
        }

        private static int nodeCounter = 0;
        private bool isRoot = false;

        public ExecutionNode AddChild(int repeatCount, string nodeName = "")
        {
            var n = new ExecutionNode(this, repeatCount, nodeName);
            this.HasChildren = true;
            _children.Add(n);
            return n;
        }

        private ExecutionNode AddChild(ExecutionNode node)
        {
            this.HasChildren = true;
            _children.Add(node);
            node._parent = this;
            return node;
        }

        public static ExecutionNode CreateLevelOneNode(int repeatCount, string nodeName = "")
        {
            var node = new ExecutionNode(null, repeatCount, nodeName);
            node.isRoot = true;
            return node;
        }

        private long _repeatCount  = 1;
        public long RepeatCount
        {
            get
            {
                return _repeatCount;
            }
            set
            {
                if (_repeatCount != value)
                {
                    _repeatCount = value;
                    OnPropertyChanged("RepeatCount");
                }
            }
        }

        private ExecutionNode(ExecutionNode parent, int repeatCount, string nodeName)
        {
            _parent = parent;
            _nodeId = nodeCounter++;
            _tables = new ObservableCollection<TableEntity>();
            _children = new ObservableCollection<ExecutionNode>();
            RepeatCount = repeatCount;
            NodeName = nodeName;
            
            HasChildren = false;
        }

        public override bool Equals(System.Object obj)
        {
            ExecutionNode p = obj as ExecutionNode;
            if ((object)p == null)
                return false;

            if (Object.ReferenceEquals(this, p))
                return true;

            return this.Equals(p);
        }

        public bool Equals(ExecutionNode b)
        {
            // Return true if the fields match:
            return
                this.NodeId.Equals(b.NodeId);
        }


        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 37;
                hash = hash * 23 + this.NodeId.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("[ExecutionNode NodeId: {0}, Level: {1}, RepeatCount: {2}]"
                , this.NodeId, this.Level, this.RepeatCount);
        }

        public ExecutionNode AddTable(TableEntity tableToAdd)
        {
            if (!Tables.Contains(tableToAdd))
                _tables.Add(tableToAdd);
            return this;
        }

        public bool HasWarning { get; private set; }

        public bool RemoveTable(TableEntity tableToRemove)
        {
            return _tables.Remove(tableToRemove);
        }

        public void MoveTableToParentNode(TableEntity tableToMove)
        {
            if (_tables.Remove(tableToMove))
                Parent.AddTable(tableToMove);
            else
                throw new KeyNotFoundException("unable to move table since it did not exist in the node");
        }

        public void MoveTableToNode(TableEntity tableToMove, ExecutionNode targetNode)
        {
            if (RemoveTable(tableToMove))
                targetNode.AddTable(tableToMove);
            else
                throw new KeyNotFoundException("unable to move table since it did not exist in the node");
        }

        public void MoveTableToNewChildNode(TableEntity tableToMove, int repeatCount, string nodeName = "")
        {
            if (RemoveTable(tableToMove))
            {
                var child = AddChild(repeatCount, nodeName);
                child.AddTable(tableToMove);
            }
            else
                throw new KeyNotFoundException("unable to move table since it did not exist in the node");
        }

        public void RemoveTables(params TableEntity[] tablesToRemove)
        {
            ValidateUtil.ValidateNotNull(tablesToRemove, "tablesToRemove");
            foreach (var tableToRemove in tablesToRemove)
            {
                RemoveTable(tableToRemove);
            }
        }

        /// <summary>
        /// Move the table up in the list of tables of this node
        /// </summary>
        /// <param name="tableToMove"></param>
        public void MoveTableUp(TableEntity tableToMove)
        {
            if (_tables.Contains(tableToMove))
            {
                int currentIndex = _tables.IndexOf(tableToMove);
                int newIndex = currentIndex - 1;
                if (newIndex < 0)
                    return;

                _tables.Move(currentIndex, newIndex);
                OnPropertyChanged("Tables");
            }
        }

        /// <summary>
        /// Move the table down in the list of tables in this node
        /// </summary>
        /// <param name="tableToMove"></param>
        public void MoveTableDown(TableEntity tableToMove)
        {
            if (_tables.Contains(tableToMove))
            {
                int currentIndex = _tables.IndexOf(tableToMove);
                int newIndex = currentIndex + 1;
                if (newIndex >= _tables.Count)
                    return;

                _tables.Move(currentIndex, newIndex);
                OnPropertyChanged("Tables");
            }
        }

        /// <summary>
        /// Get string displaying the hierarchy of the node and it's children.
        /// </summary>
        /// <remarks>
        /// Consider JP's answer at http://stackoverflow.com/questions/1208703/can-an-anonymous-method-in-c-sharp-call-itself
        /// </remarks>
        /// <returns></returns>
        public string getDebugString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            Action<ExecutionNode, StringBuilder> builder = null;
            builder = new Action<ExecutionNode, StringBuilder>( (root, sb) =>
            {
                sb.Append(new string('-', root.Level - 1));
                sb.Append(root.NodeName);

                foreach (var child in root.Children)
                {
                    sb.AppendLine();
                    builder.Invoke(child, sb);
                }
            });

            builder.Invoke(this, stringBuilder);

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Creates a new node, move current node to child as that node, returns the new node
        /// </summary>
        /// <param name="repeatCount"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public ExecutionNode AddParent(int repeatCount, string nodeName = "")
        {
            if (this.isRoot)
                return this;
            
            var newParent = this.Parent.AddChild(repeatCount, nodeName);

            MoveAtoTheLocationOfB(Parent._children, newParent, this);
            Parent._children.Remove(this);
            newParent.AddChild(this);
            return newParent;
        }

        private void MoveAtoTheLocationOfB(ObservableCollection<ExecutionNode> nodes, ExecutionNode a, ExecutionNode b)
        {
            int aIndex = nodes.IndexOf(a);
            int bIndex = nodes.IndexOf(b);
            if (aIndex > 0 && bIndex > 0)
            {
                nodes.Move(aIndex, bIndex);    
            }
        }

        public ExecutionNode MergeWithParent()
        {
            if (this.isRoot)
                return this;

            Parent._children.Remove(this);
            foreach (var table in Tables)
            {
                Parent.AddTable(table);
            }
            foreach (var child in Children)
            {
                Parent.AddChild(child);
            }

            return Parent;
        }

        public ExecutionNode RemoveNode()
        {
            if (isRoot)
                return this;

            Parent._children.Remove(this);
            return Parent;
        }

    }
    
}
