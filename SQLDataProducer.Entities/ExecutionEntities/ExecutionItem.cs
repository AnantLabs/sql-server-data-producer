using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDataProducer.DatabaseEntities.Entities;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

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
        public ExecutionItem(TableEntity table, int order)
        {
            TargetTable = table;
            Order = order;
        }
        public ExecutionItem()
        {

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


        public ExecutionItem CloneWithOrderNumber(int orderNr)
        {
            return ExecutionItem.Create(this.Description, this.RepeatCount, this.TargetTable.Clone(), this.TruncateBeforeExecution, orderNr);
        }

        private static ExecutionItem Create(string description, int repeatCount, TableEntity table, bool truncateBeforeExecution, int orderNr)
        {
            var tabl = new TableEntity(table.TableSchema, table.TableName);
            var ei = new ExecutionItem(table, orderNr);
            ei.Description = description;
            ei.RepeatCount = repeatCount;
            ei.TruncateBeforeExecution = truncateBeforeExecution;
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
        
        private int _executionConditionValue;
        public int ExecutionConditionValue
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
    }
}
