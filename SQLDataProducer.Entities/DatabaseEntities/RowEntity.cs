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


using SQLDataProducer.Entities.DatabaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Entities.DatabaseEntities
{
    public class RowEntity : EntityBase, IListSource, ICustomTypeDescriptor
    {

        public List<DataCell> Cells { get; set; }

        private RowEntity()
        {
            Cells = new List<DataCell>();
        }

        public static RowEntity Create(TableEntity table, long n)
        {
            var r = new RowEntity();
            for (int i = 0; i < table.Columns.Count; i++)
                r.Cells.Add(new DataCell { Column = table.Columns[i], Value = table.Columns[i].GenerateValue(n) });

            return r;
        }

        public bool ContainsListCollection
        {
            get { return true; }
        }

        public System.Collections.IList GetList()
        {
            return Cells;
        }

        private PropertyDescriptorCollection m_PropertyDescriptorCollectionCache;

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return new AttributeCollection(null);
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return null;
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return null;
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return null;
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return null;
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return null;
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return null;
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return new EventDescriptorCollection(null);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return new EventDescriptorCollection(null);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            if (m_PropertyDescriptorCollectionCache == null)
            {
                PropertyDescriptor[] properties = new PropertyDescriptor[Cells.Count];
                int i = 0;
                foreach (var key in Cells)
                {
                    properties[i] = new MyCustomClassPropertyDescriptor(key.Column.ColumnName);
                    i++;
                }
                m_PropertyDescriptorCollectionCache = new PropertyDescriptorCollection(properties);
            }
            return m_PropertyDescriptorCollectionCache;
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(null);
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

    }

    public class MyCustomClassPropertyDescriptor : PropertyDescriptor
    {
        public MyCustomClassPropertyDescriptor(string key)
            : base(key, null)
        {

        }

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override Type ComponentType
        {
            get { return typeof(List<DataCell>); }
        }

        public override object GetValue(object component)
        {
            return ((List<DataCell>)component).Where(x=> x.Column.ColumnName == base.Name);
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get { return typeof(string); }
        }

        public override void ResetValue(object component)
        {
            ((List<DataCell>)component).Where(x => x.Column.ColumnName == base.Name).Single().Value = string.Empty;
        }

        public override void SetValue(object component, object value)
        {
            ((List<DataCell>)component).Where(x => x.Column.ColumnName == base.Name).Single().Value = value.ToString();
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }

    public class DataCell
    {
        public ColumnEntity Column { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0} = '{1}'", Column, Value);
        }
    }
}
