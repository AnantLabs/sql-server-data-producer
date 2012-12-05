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
using SQLDataProducer.Entities.DatabaseEntities.Collections;
using System.Xml.Serialization;
using SQLDataProducer.Entities;
using System.Linq;
using System.Text;
using SQLDataProducer.Entities.ExecutionEntities;


namespace SQLDataProducer.Entities.DatabaseEntities
{
    public class TableEntity : EntityBase, IEquatable<TableEntity>, IXmlSerializable
    {

        public static event TableWithForeignKeyInsertedRowEventHandler ForeignKeyGenerated = delegate { };

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
                    OnPropertyChanged("Columns");
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
                    OnPropertyChanged("TableSchema");
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
                    OnPropertyChanged("TableName");
                }
            }
        }


        public override string ToString()
        {
            return string.Format("{0}.{1}", TableSchema, TableName);
        }



        //public bool Equals(TableEntity other)
        //{
        //    // Check whether the compared object is null.
        //    if (Object.ReferenceEquals(other, null)) return false;

        //    // Check whether the compared object references the same data.
        //    if (Object.ReferenceEquals(this, other)) return true;

        //    // Check whether the objects’ properties are equal.
        //    return this.ToString().Equals(other.ToString());
        //}

        //// If Equals returns true for a pair of objects,
        //// GetHashCode must return the same value for these objects.

        //public override int GetHashCode()
        //{
        //    // Get the hash code for the Textual field if it is not null.
        //    return ToString().GetHashCode();
        //}

        internal TableEntity Clone()
        {
            var t = TableEntity.Create(this.TableSchema, this.TableName, this.Columns.Clone());
            t.HasIdentityColumn = this.HasIdentityColumn;
            return t;
        }

        private static TableEntity Create(string tableSchema, string tableName, ColumnEntityCollection cols)
        {
            var t = new TableEntity(tableSchema, tableName);
            t.Columns = cols;
            
            return t;
        }

        #region XML Serialize
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            this.TableSchema = reader.GetAttribute("TableSchema");
            this.TableName = reader.GetAttribute("TableName");

            if (reader.ReadToDescendant("Columns"))
            {
                while (reader.Read() && reader.MoveToContent() == System.Xml.XmlNodeType.Element && reader.LocalName == "Column")
                {
                    ColumnEntity col = new ColumnEntity();
                    col.ReadXml(reader);
                    this.Columns.Add(col);
                }
            }
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
                    OnPropertyChanged("ParentExecutionItem");
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
                // this.Columns == b.Columns &&
                 this.HasIdentityColumn == b.HasIdentityColumn &&
                 this.HasWarning == b.HasWarning &&
                 //this.ParentExecutionItem == b.ParentExecutionItem &&
                 this.TableName == b.TableName &&
                 this.TableSchema == b.TableSchema &&
                 this.WarningText == b.WarningText;
                 
        }

        public override int GetHashCode()
        {
            return
                // this.Columns.GetHashCode() ^
                 this.HasIdentityColumn.GetHashCode() ^
                 this.HasWarning.GetHashCode() ^
                 //this.ParentExecutionItem.GetHashCode() ^
                 this.TableName.GetHashCode() ^
                 this.TableSchema.GetHashCode() ^
                 this.WarningText.GetHashCode();
        }
        //public static bool operator ==(TableEntity a, TableEntity b)
        //{
        //    // If both are null, or both are same instance, return true.
        //    if (System.Object.ReferenceEquals(a, b))
        //        return true;

        //    // If one is null, but not both, return false.
        //    if (((object)a == null) || ((object)b == null))
        //        return false;

        //    // Return true if the fields match:
        //    return
        //         //a.Columns == b.Columns &&
        //         a.HasIdentityColumn == b.HasIdentityColumn &&
        //         a.HasWarning == b.HasWarning &&
        //         //a.ParentExecutionItem == b.ParentExecutionItem &&
        //         a.TableName == b.TableName &&
        //         a.TableSchema == b.TableSchema &&
        //         a.WarningText == b.WarningText;

        //}
        //public static bool operator !=(TableEntity a, TableEntity b)
        //{
        //    return !(a == b);
        //}


    }


}
