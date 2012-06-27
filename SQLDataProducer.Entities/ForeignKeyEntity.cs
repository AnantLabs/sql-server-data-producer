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

using SQLDataProducer.DatabaseEntities.Entities;
using System.Collections.ObjectModel;

namespace SQLDataProducer.Entities
{
    
    public class ForeignKeyEntity : EntityBase
    {
        public ForeignKeyEntity()
        {
            Keys = new ObservableCollection<int>();
        }

        private TableEntity _referencingTable;
        [System.ComponentModel.ReadOnly(true)]
        public TableEntity ReferencingTable
        {
            get
            {
                return _referencingTable;
            }
            set
            {
                if (_referencingTable != value)
                {
                    _referencingTable = value;
                    OnPropertyChanged("ReferencingTable");
                }
            }
        }

        private string _referencingColumn;
        [System.ComponentModel.ReadOnly(true)]
        public string ReferencingColumn
        {
            get
            {
                return _referencingColumn;
            }
            set
            {
                if (_referencingColumn != value)
                {
                    _referencingColumn = value;
                    OnPropertyChanged("ReferencingColumn");
                }
            }
        }

        private ObservableCollection<int> _keys;
        [System.ComponentModel.ReadOnly(true)]
        public ObservableCollection<int> Keys
        {
            get
            {
                return _keys;
            }
            set
            {
                if (_keys != value)
                {
                    _keys = value;
                    OnPropertyChanged("Keys");
                }
            }
        }


        internal ForeignKeyEntity Clone()
        {
            ForeignKeyEntity fk = new ForeignKeyEntity();
            fk.ReferencingColumn = this.ReferencingColumn;
            fk.ReferencingTable = this.ReferencingTable.Clone();
            fk.Keys = new ObservableCollection<int>(this.Keys); ;
            return fk;
        }

        //public System.Xml.Schema.XmlSchema GetSchema()
        //{
        //    throw new NotImplementedException();
        //}

        //public void ReadXml(System.Xml.XmlReader reader)
        //{
        //    throw new NotImplementedException();
        //}

        //public void WriteXml(System.Xml.XmlWriter writer)
        //{
        //    writer.WriteStartElement("ForeignKey");
        //    writer.WriteAttributeString("ReferencingColumn", this.ReferencingColumn);
        //    writer.WriteAttributeString("ReferencingTable.TableName", this.ReferencingTable.TableName);
        //    writer.WriteAttributeString("ReferencingTable.TableSchema", this.ReferencingTable.TableSchema);
        //    writer.WriteEndElement();
        //}
    }
}
