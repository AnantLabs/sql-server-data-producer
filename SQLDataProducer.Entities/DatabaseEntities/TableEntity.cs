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

namespace SQLDataProducer.DatabaseEntities.Entities
{
    public class TableEntity : SQLDataProducer.Entities.EntityBase, IEquatable<TableEntity>, IXmlSerializable
    {

        public static event TableWithForeignKeyInsertedRowEventHandler ForeignKeyGenerated = delegate { };

        public TableEntity(string tableSchema, string tableName)
        {
            Columns = new ColumnEntityCollection();
            TableName = tableName;
            TableSchema = tableSchema;
        }
        public TableEntity()
        {
            Columns = new ColumnEntityCollection();
        }

        ColumnEntityCollection _columns;
        public ColumnEntityCollection Columns
        {
            get
            {
                return _columns;
            }
            set
            {
                if (_columns != value)
                {
                    _columns = value;
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



        public bool Equals(TableEntity other)
        {
            // Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            // Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            // Check whether the objects’ properties are equal.
            return this.ToString().Equals(other.ToString());
        }

        // If Equals returns true for a pair of objects,
        // GetHashCode must return the same value for these objects.

        public override int GetHashCode()
        {
            // Get the hash code for the Textual field if it is not null.
            return ToString().GetHashCode();

        }

        internal TableEntity Clone()
        {
            return TableEntity.Create(this.TableSchema, this.TableName, this.Columns.Clone());
        }

        private static TableEntity Create(string tableSchema, string tableName, ColumnEntityCollection cols)
        {
            var t = new TableEntity(tableSchema, tableName);
            t.Columns = cols;
            return t;
        }

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
    }
}
