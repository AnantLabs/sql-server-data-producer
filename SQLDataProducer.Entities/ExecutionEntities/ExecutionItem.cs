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
using System.Data;

namespace SQLDataProducer.Entities.ExecutionEntities
{

    /// <summary>
    /// An execution item is a table that have been configured to get data generated.
    /// </summary>
    public sealed class ExecutionItem : EntityBase, IXmlSerializable//--, IComparable<ExecutionItem>
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
                    _targetTable.ParentExecutionItem = this;
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


        /// <summary>
        /// ExecutionItemCollection where this execution item is located.
        /// </summary>
        ExecutionItemCollection _parentCollection;
        public ExecutionItemCollection ParentCollection
        {
            get
            {
                return _parentCollection;
            }
            set
            {
                if (_parentCollection != value)
                {
                    _parentCollection = value;
                    OnPropertyChanged("ParentCollection");
                }
            }
        }


        bool _useIdenityInsert = false;
        public bool UseIdentityInsert
        {
            get
            {
                return _useIdenityInsert;
            }
            set
            {
                if (_useIdenityInsert != value)
                {
                    _useIdenityInsert = value;
                    OnPropertyChanged("UseIdentityInsert");
                }
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

        public static DataTable CreatePreview(ExecutionItem ei)
        {

            DataTable table = new DataTable(ei.TargetTable.TableName, ei.TargetTable.TableSchema);
            foreach (var c in ei.TargetTable.Columns)
            {
                table.Columns.Add(new DataColumn(c.ColumnName, typeof(object)));
            }

            for (int i = 1; i < 100; i++)
            {
                ei.TargetTable.GenerateValuesForColumns(i);
                var row = table.NewRow();
                table.Rows.Add(row);
                for (int k = 0; k < table.Columns.Count; k++)
                {
                    var value = ei.TargetTable.Columns[k].PreviouslyGeneratedValue;
                    row.SetField(table.Columns[k], value);
                }
                   
            }

            return table;
        }







        public override bool Equals(System.Object obj)
        {
            // If parameter cannot be casted return false:
            ExecutionItem p = obj as ExecutionItem;
            if ((object)p == null)
                return false;

            // Return true if the fields match:
            return GetHashCode() == p.GetHashCode();
        }

        public bool Equals(ExecutionItem b)
        {
            // Return true if the fields match:
            return
                    Description == b.Description &&
                    ExecutionCondition == b.ExecutionCondition &&
                    ExecutionConditionValue == b.ExecutionConditionValue &&
                    HasWarning == b.HasWarning &&
                    Order == b.Order &&
                    RepeatCount == b.RepeatCount &&
                    TargetTable == b.TargetTable &&
                    TruncateBeforeExecution == b.TruncateBeforeExecution &&
                    UseIdentityInsert == b.UseIdentityInsert &&
                    WarningText == b.WarningText;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^
                    Description.GetHashCode() ^
                    ExecutionCondition.GetHashCode() ^
                    ExecutionConditionValue.GetHashCode() ^
                    HasWarning.GetHashCode() ^
                    Order.GetHashCode() ^
                    RepeatCount.GetHashCode() ^
                    TargetTable.GetHashCode() ^
                    TruncateBeforeExecution.GetHashCode() ^
                    UseIdentityInsert.GetHashCode() ^
                    WarningText.GetHashCode();
        }
        //public static bool operator ==(ExecutionItem a, ExecutionItem b)
        //{
        //    // If both are null, or both are same instance, return true.
        //    if (System.Object.ReferenceEquals(a, b))
        //        return true;

        //    // If one is null, but not both, return false.
        //    if (((object)a == null) || ((object)b == null))
        //        return false;

        //    // Return true if the fields match:
        //    return                   
        //            a.Description == b.Description &&
        //            a.ExecutionCondition == b.ExecutionCondition &&
        //            a.ExecutionConditionValue == b.ExecutionConditionValue &&
        //            a.HasWarning == b.HasWarning &&
        //            a.Order == b.Order &&
        //            a.RepeatCount == b.RepeatCount &&
        //            //a.TargetTable == b.TargetTable &&
        //            a.TruncateBeforeExecution == b.TruncateBeforeExecution &&
        //            a.UseIdentityInsert == b.UseIdentityInsert &&
        //            a.WarningText == b.WarningText;
        
        //}
        //public static bool operator !=(ExecutionItem a, ExecutionItem b)
        //{
        //    return !(a == b);
        //}
        
    }
}
