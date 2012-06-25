using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDataProducer.DatabaseEntities.Entities;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

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
