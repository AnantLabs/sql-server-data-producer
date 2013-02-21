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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Entities.ExecutionEntities
{

       //RootNode = Node.CreateLevelOneNode(1);
       //     var player = RootNode.AddChild(100);
       //     player.AddExecutionItem("Player");
       //     player.AddExecutionItem("PlayerDetails");
       //     player.AddExecutionItem("SW Account");
       //     player.AddExecutionItem("Bonus Account");


       //     var playerSession = player.AddChild(1);
       //     playerSession.AddExecutionItem("PlayerSession");

       //     playerSession.AddChild(1).AddExecutionItem("Deposit into account").AddExecutionItem("SeamlessWalletTransaction");

       //     var gameRound = playerSession.AddChild(50);
       //     gameRound.AddExecutionItem("GameRound");
       //     gameRound.AddExecutionItem("GameRoundTransaction");
       //     gameRound.AddExecutionItem("AccountTransaction (bet)").AddExecutionItem("SeamlessWalletTransaction"); ;
       //     gameRound.AddExecutionItem("AccountTransaction (win)").AddExecutionItem("SeamlessWalletTransaction"); ;
       //     gameRound.AddExecutionItem("GameTracking");
       //     gameRound.AddExecutionItem("LocalJackpotContribution");

       //     playerSession.AddChild(1).AddExecutionItem("Withdraw all money from account").AddExecutionItem("SeamlessWalletTransaction");
       //     playerSession.AddChild(1).AddExecutionItem("Update PlayerSession END");

    public class ExecutionNode : EntityBase
    {
        
            ExecutionNode _parent;
            public ExecutionNode Parent
            {
                get
                {
                    return _parent;
                }
                set
                {
                    if (_parent != value)
                    {
                        _parent = value;
                        OnPropertyChanged("Parent");
                    }
                }
            }

            ObservableCollection<ExecutionNode> _children;
            public ObservableCollection<ExecutionNode> Children
            {
                get
                {
                    return _children;
                }
                set
                {
                    if (_children != value)
                    {
                        _children = value;
                        OnPropertyChanged("Children");
                    }
                }
            }


            int _nodeId;
            public int NodeId
            {
                get
                {
                    return _nodeId;
                }
                set
                {
                    if (_nodeId != value)
                    {
                        _nodeId = value;
                        OnPropertyChanged("NodeId");
                    }
                }
            }


            int _level;
            public int Level
            {
                get
                {
                    return _level;
                }
                set
                {
                    if (_level != value)
                    {
                        _level = value;
                        OnPropertyChanged("Level");
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
                set
                {
                    if (_hasChildren != value)
                    {
                        _hasChildren = value;
                        OnPropertyChanged("HasChildren");
                    }
                }
            }

            public List<ExecutionItem> ExecutionItems { get; set; }

            private static int nodeCounter = 0;

            public ExecutionNode AddChild(int repeatCount)
            {
                if (Children == null)
                    Children = new ObservableCollection<ExecutionNode>();

                var n = new ExecutionNode(this.Level + 1, this, repeatCount);
                this.HasChildren = true;
                Children.Add(n);
                return n;
            }


            public static ExecutionNode CreateLevelOneNode(int repeatCount)
            {
                return new ExecutionNode(1, null, repeatCount);
            }

            public int RepeatCount { get; set; }

            private ExecutionNode(int level, ExecutionNode parent, int repeatCount)
            {
                Parent = parent;
                NodeId = nodeCounter++;
                ExecutionItems = new List<ExecutionItem>();
                RepeatCount = repeatCount;

                Level = level;
                HasChildren = false;
            }

        }
    
}
