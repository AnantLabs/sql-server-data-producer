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
using SQLDataProducer.Entities.DatabaseEntities.Collections;
using System.Xml.Serialization;
using SQLDataProducer.Entities;
using System.Linq;
using System.Text;
using SQLDataProducer.Entities.ExecutionEntities;
using System.Xml.Linq;


namespace SQLDataProducer.Entities.DatabaseEntities
{
    public class TableEntity : EntityBase, IEquatable<TableEntity> 
    {

        public TableEntity(string tableSchema, string tableName)
            : this()
        {
            TableName = tableName;
            TableSchema = tableSchema;
        }

        public TableEntity()
        {
            Columns = new ColumnEntityCollection();
            Columns.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Columns_CollectionChanged);
        }

        // Columns added event handler
        void Columns_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (ColumnEntity item in e.NewItems)
            {
                if (item.HasWarning)
                    HasWarning = true;

                item.ParentTable = this;
            }

            HasIdentityColumn = Columns.Any(c => c.IsIdentity);
        }


        bool _hasIdentityColumn = false;
        public bool HasIdentityColumn
        {
            get
            {
                return _hasIdentityColumn;
            }
            private set
            {
                if (_hasIdentityColumn != value)
                {
                    _hasIdentityColumn = value;
                    OnPropertyChanged("HasIdentityColumn");
                }
            }
        }


        ColumnEntityCollection _columns;
        public ColumnEntityCollection Columns
        {
            get
            {
                return _columns;
            }
            private set
            {
                if (_columns != value)
                {
                    _columns = value;
                    HasWarning = _columns.Any(c => c.HasWarning);
                    //OnPropertyChanged("Columns");
                }
            }
        }

        string _tableSchema;
        public string TableSchema
        {
            get
            {
                return _tableSchema;
            }
            private set
            {
                if (_tableSchema != value)
                {
                    _tableSchema = value;
                    //OnPropertyChanged("TableSchema");
                }
            }
        }

        string _tableName;
        public string TableName
        {
            get
            {
                return _tableName;
            }
            private set
            {
                if (_tableName != value)
                {
                    _tableName = value;
                    //OnPropertyChanged("TableName");
                }
            }
        }


        public override string ToString()
        {
            return string.Format("{0}.{1}", TableSchema, TableName);
        }

       
        internal TableEntity Clone()
        {
            var t = new TableEntity(this.TableSchema, this.TableName);
            t.HasIdentityColumn = this.HasIdentityColumn;
            t.Columns = this.Columns.Clone();
            t.RefreshWarnings();
            return t;
        }

       

        #region XML Serialize

        public void ReadXml(XElement xe)
        {
            this.TableSchema = xe.Attribute("TableSchema").Value;
            this.TableName = xe.Attribute("TableName").Value;
            this.Columns.ReadXml(xe.Element("Columns"));
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {

            writer.WriteStartElement("Table");
            writer.WriteAttributeString("TableSchema", this.TableSchema);
            writer.WriteAttributeString("TableName", this.TableName);
            writer.WriteStartElement("Columns");
            foreach (var item in Columns)
            {
                item.WriteXml(writer);

            }
            writer.WriteEndElement();
            writer.WriteEndElement();
        } 
        #endregion

        public void GenerateValuesForColumns(long n)
        {
            foreach (var col in Columns)
            {
                col.GenerateValue(n);
            }
        }

        private bool _hasWarning = false;
        /// <summary>
        /// This Item have some kind of warning that might cause problems during execution
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
        /// Contains warning text if the this item have a warning that might cause problems during execution.
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
        ///  Execution item where this table is used.
        /// </summary>
        ExecutionItem _parentExecutionItem;
        public ExecutionItem ParentExecutionItem
        {
            get
            {
                return _parentExecutionItem;
            }
            set
            {
                if (_parentExecutionItem != value)
                {
                    _parentExecutionItem = value;
                    //OnPropertyChanged("ParentExecutionItem");
                }
            }
        }








        public override bool Equals(System.Object obj)
        {
            // If parameter cannot be casted return false:
            TableEntity p = obj as TableEntity;
            if ((object)p == null)
                return false;

            // Return true if the fields match:
            return GetHashCode() == p.GetHashCode();
        }

        public bool Equals(TableEntity b)
        {
            if ((object)b == null)
                return false;

            // Return true if the fields match:
            return
                Enumerable.SequenceEqual(this.Columns, b.Columns) &&
                 this.HasIdentityColumn == b.HasIdentityColumn &&
                 this.HasWarning == b.HasWarning &&
                 this.TableName == b.TableName &&
                 this.TableSchema == b.TableSchema &&
                 this.WarningText == b.WarningText;

        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 37;
                //hash = hash * 23 + base.GetHashCode();
                //hash = hash * 23 + Columns.GetHashCode();
                hash = hash * 23 + HasIdentityColumn.GetHashCode();
                hash = hash * 23 + HasWarning.GetHashCode();
                hash = hash * 23 + TableName.GetHashCode();
                hash = hash * 23 + TableSchema.GetHashCode();
                hash = hash * 23 + WarningText.GetHashCode();
                return hash;
            }
        }

        public void RefreshWarnings()
        {
            foreach (var col in Columns)
            {
                col.RefreshWarningStatus();
            }
            HasWarning = Columns.Any(col => col.HasWarning);
            WarningText = HasWarning ? "One of the columns have a warning" : string.Empty;
        }

        
    }


}
