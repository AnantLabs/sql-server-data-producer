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

using System;
using SQLDataProducer.Entities.DatabaseEntities;
using System.Xml.Serialization;
using System.Linq;

namespace SQLDataProducer.Entities.ExecutionEntities
{

    /// <summary>
    /// An execution item is a table that have been configured to get data generated.
    /// </summary>
    public class ExecutionItem : EntityBase, IXmlSerializable
    {
        TableEntity _targetTable;
        /// <summary>
        /// The table to generate data for.
        /// </summary>
        public TableEntity TargetTable
        {
            get
            {
                return _targetTable;
            }
            private set
            {
                if (_targetTable != value)
                {
                    _targetTable = value;
                    HasWarning = _targetTable.HasWarning;
                    OnPropertyChanged("TargetTable");
                }
            }
        }


        int _order;
        public int Order
        {
            get
            {
                return _order;
            }
            set
            {
                if (_order != value)
                {
                    _order = value;
                    OnPropertyChanged("Order");
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="table">The table to generate data for</param>
        /// <param name="order">the order of the execution item. Is used to generate the name of variables so that other execution items can depend on this</param>
        public ExecutionItem(TableEntity table, string description = "")
            : this()
        {
            TargetTable = table;
            Description = description;
        }

        public ExecutionItem()
        {
            Order = int.MinValue;
        }


        string _description;
        /// <summary>
        /// Description of the Execution Item. Use to describe the purpose of it
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        bool _truncateBeforeExecution;
        /// <summary>
        /// Should the table be truncated before running the data generation?
        /// </summary>
        public bool TruncateBeforeExecution
        {
            get
            {
                return _truncateBeforeExecution;
            }
            set
            {
                if (_truncateBeforeExecution != value)
                {
                    _truncateBeforeExecution = value;
                    OnPropertyChanged("TruncateBeforeExecution");
                }
            }
        }


        int _repeatExectution = 1;
        public int RepeatCount
        {
            get
            {
                return _repeatExectution;
            }
            set
            {
                if (_repeatExectution != value)
                {
                    if (value < 1)
                        value = 1;
                    if (value > 1000)
                        value = 1000;
                    
                    _repeatExectution = value;
                    OnPropertyChanged("RepeatCount");
                }
            }
        }


        public ExecutionItem Clone()
        {
            TableEntity clonedTable = this.TargetTable.Clone();

            var tabl = new TableEntity(clonedTable.TableSchema, clonedTable.TableName);
            var ei = new ExecutionItem(clonedTable);
            ei.Description = this.Description;
            ei.RepeatCount = this.RepeatCount;
            ei.TruncateBeforeExecution = this.TruncateBeforeExecution;
            return ei;
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            this.Description = reader.GetAttribute("Description");
            this.Order = reader.TryGetIntAttribute("Order", 1);
            this.RepeatCount = reader.TryGetIntAttribute("RepeatCount", 1);
            this.TruncateBeforeExecution = reader.TryGetBoolAttribute("TruncateBeforeExecution", false);

            this.ExecutionCondition = (ExecutionConditions)reader.TryGetIntAttribute("ExecutionCondition", 0);
            this.ExecutionConditionValue = reader.TryGetIntAttribute("ExecutionConditionValue", 0);
            

            if (reader.ReadToDescendant("Table"))
            {
                TableEntity table = new TableEntity();
                table.ReadXml(reader);
                this.TargetTable = table;
            }

        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("ExecutionItem");
            writer.WriteAttributeString("Description", this.Description);
            writer.WriteAttributeString("Order", this.Order.ToString());
            writer.WriteAttributeString("RepeatCount", this.RepeatCount.ToString());
            writer.WriteAttributeString("TruncateBeforeExecution", this.TruncateBeforeExecution.ToString());
            writer.WriteAttributeString("ExecutionCondition", this.ExecutionCondition.ToString());
            writer.WriteAttributeString("ExecutionConditionValue", this.ExecutionConditionValue.ToString());
            this.TargetTable.WriteXml(writer);
            writer.WriteEndElement();

        }

        private ExecutionConditions _executionCondition = ExecutionConditions.None;
        public ExecutionConditions ExecutionCondition
        {
            get
            {
                return _executionCondition;
            }
            set
            {
                _executionCondition = value;
                OnPropertyChanged("ExecutionCondition");
            }
        }
        
        private long _executionConditionValue;
        public long ExecutionConditionValue
        {
            get
            {
                return _executionConditionValue;
            }
            set
            {
                _executionConditionValue = value;
                OnPropertyChanged("ExecutionConditionValue");
            }
        }

        public override string ToString()
        {
            return string.Format("{0} : {1}", Order, TargetTable.ToString());
        }

        private bool _hasWarning = false;
        /// <summary>
        /// This Execution Item have some kind of warning that might cause problems during execution
        /// </summary>
        public bool HasWarning
        {
            get
            {
                return _hasWarning;
            }
            set
            {
                _hasWarning = value;
                OnPropertyChanged("HasWarning");
            }
        }

        private string _warningText = string.Empty;
        /// <summary>
        /// Contains warning text if the this execution item have a warning that might cause problems during execution.
        /// </summary>
        public string WarningText
        {
            get
            {
                return _warningText;
            }
            set
            {
                _warningText = value;
                OnPropertyChanged("WarningText");
            }
        }

        public bool ShouldExecuteForThisN(long N)
        {
            switch (ExecutionCondition)
            {
                case ExecutionConditions.None:
                    return true;
                case ExecutionConditions.LessThan:
                    return ExecutionConditionValue < N;
                case ExecutionConditions.LessOrEqualTo:
                    return ExecutionConditionValue <= N;
                case ExecutionConditions.EqualTo:
                    return ExecutionConditionValue == N;
                case ExecutionConditions.EqualOrGreaterThan:
                    return ExecutionConditionValue >= N;
                case ExecutionConditions.GreaterThan:
                    return ExecutionConditionValue > N;
                case ExecutionConditions.NotEqualTo:
                    return ExecutionConditionValue != N;
                case ExecutionConditions.EveryOtherX:
                    return ExecutionConditionValue % N == 0;
                default:
                    throw new NotSupportedException("ExecutionConditions not supported");
            }
        }
    }
}
