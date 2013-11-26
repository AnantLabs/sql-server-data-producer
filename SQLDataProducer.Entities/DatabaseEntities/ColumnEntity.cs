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
using System.Linq;
using SQLDataProducer.Entities.Generators;
using System.Collections.ObjectModel;
using SQLDataProducer.Entities;
using System.Xml.Serialization;
using SQLDataProducer.Entities.DatabaseEntities;
using System.Xml.Linq;
using System.Threading;

namespace SQLDataProducer.Entities.DatabaseEntities
{
	public partial class ColumnEntity : EntityBase, IEquatable<ColumnEntity>
	{
		ColumnDataTypeDefinition _columnDataType;

		[System.ComponentModel.ReadOnly(true)]
		public ColumnDataTypeDefinition ColumnDataType {
			get {
				return _columnDataType;
			}
			private set {
				if (_columnDataType != value) {
					_columnDataType = value;
					OnPropertyChanged("ColumnDataType");
				}
			}
		}

		bool _isIdentity;
		[System.ComponentModel.ReadOnly(true)]
		public bool IsIdentity {
			get {
				return _isIdentity;
			}
			private set {
				if (_isIdentity != value) {
					_isIdentity = value;
					OnPropertyChanged("IsIdentity");
					OnPropertyChanged("IsNotIdentity");
				}
			}
		}

		[System.ComponentModel.ReadOnly(true)]
		public bool IsNotIdentity {
			get {
				return !_isIdentity;
			}
		}

		string _columnName;
		[System.ComponentModel.ReadOnly(true)]
		public string ColumnName {
			get {
				return _columnName;
			}
			private set {
				if (_columnName != value) {
					_columnName = value;
					OnPropertyChanged("ColumnName");
				}
			}
		}

		int _ordinalPosition;
		[System.ComponentModel.ReadOnly(true)]
		public int OrdinalPosition {
			get {
				return _ordinalPosition;
			}
			private set {
				if (_ordinalPosition != value) {
					_ordinalPosition = value;
					OnPropertyChanged("OrdinalPosition");
				}
			}
		}

		Generator _generator;
		public Generator Generator {
			get {
				return _generator;
			}
			set {
				if (_generator != value) {
					_generator = value;
					_generator.ParentColumn = this;
					OnPropertyChanged("Generator");
				}
			}
		}

		ObservableCollection<Generator> _valueGenerators;

		public ObservableCollection<Generator> PossibleGenerators {
			get {
				return _valueGenerators;
			}
			set {
				if (_valueGenerators != value) {
					_valueGenerators = value;
					OnPropertyChanged("PossibleGenerators");
				}
			}
		}

		private bool _isForeignKey;

		[System.ComponentModel.ReadOnly(true)]
		public bool IsForeignKey {
			get {
				return _isForeignKey;
			}
			private set {
				if (_isForeignKey != value) {
					_isForeignKey = value;
					OnPropertyChanged("IsForeignKey");
				}
			}
		}

		private ForeignKeyEntity _foreignKey;

		[System.ComponentModel.ReadOnly(true)]
		public ForeignKeyEntity ForeignKey {
			get {
				return _foreignKey;
			}
            private set
            {
                if (_foreignKey != value)
                {
                    _foreignKey = value;

                    // TODO: Indicate when foreign keys have been added such that the HasWarning flag is unset.

                    OnPropertyChanged("ForeignKey");
                }
            }
		}

		string _constraints;

		[System.ComponentModel.ReadOnly(true)]
		public string Constraints {
			get {
				return _constraints;
			}
			private set {
				if (_constraints != value) {
					_constraints = value;
					_hasConstraints = !string.IsNullOrEmpty(_constraints);
					OnPropertyChanged("Constraints");
					OnPropertyChanged("HasConstraints");
				}
			}
		}

		bool _hasConstraints;

		[System.ComponentModel.ReadOnly(true)]
		public bool HasConstraints {
			get {
				return _hasConstraints;
			}
		}

		internal ColumnEntity(string columnName, ColumnDataTypeDefinition columnDatatype, bool isIdentity, int ordinalPosition, bool isForeignKey, string constraintDefinition, ForeignKeyEntity foreignKeyEntity)
		{
			this.ColumnName = columnName;
			this.ColumnDataType = columnDatatype;
			this.OrdinalPosition = ordinalPosition;
			this.IsIdentity = isIdentity;

			this.IsForeignKey = isForeignKey;
			this.ForeignKey = foreignKeyEntity;

			this.Constraints = constraintDefinition;

			RefreshWarningStatus();

		}

		public ColumnEntity()
		{

		}

        ThreadLocal<Guid> columnIdentity = new ThreadLocal<Guid>(() =>
        {
            return Guid.NewGuid();
        });

        public Guid ColumnIdentity
        {
            get { return columnIdentity.Value; }
        
        }

        public object GenerateValue(long n)
        {
            return  Generator.GenerateValue(n);
        }

		public override string ToString()
		{
			return string.Format("ColumnName: {0}", ColumnName);
		}

		public string ToFullString()
		{
			return string.Format(@"ColumnName = {0} 
ColumnDataType = {1} 
Constraints = {2} 
ForeignKey = {3} 
Generator = {4} 
HasConstraints = {5} 
HasWarning = {6} 
IsForeignKey = {7} 
IsIdentity = {8} 
IsNotIdentity = {9} 
OrdinalPosition = {10} 
PossibleGenerators = {11} 
WarningText = {12}  ", this.ColumnName,
			                              this.ColumnDataType,
			                              this.Constraints,
			                              this.ForeignKey,
			                              this.Generator,
			                              this.HasConstraints,
			                              this.HasWarning,
			                              this.IsForeignKey,
			                              this.IsIdentity,
			                              this.IsNotIdentity,
			                              this.OrdinalPosition,
			                              this.PossibleGenerators,
			                              this.WarningText);



		}

		private bool _hasWarning = false;

		/// <summary>
		/// This Item have some kind of warning that might cause problems during execution
		/// </summary>
		public bool HasWarning {
			get {
				return _hasWarning;
			}
			set {
				_hasWarning = value;
				OnPropertyChanged("HasWarning");
			}
		}

		private string _warningText = string.Empty;

		/// <summary>
		/// Contains warning text if the this item have a warning that might cause problems during execution.
		/// </summary>
		public string WarningText {
			get {
				return _warningText;
			}
			set {
				_warningText = value;
				OnPropertyChanged("WarningText");
			}
		}

		TableEntity _parentTable;

		public TableEntity ParentTable {
			get {
				return _parentTable;
			}
			set {
				if (_parentTable != value) {
					_parentTable = value;
					OnPropertyChanged("ParentTable");
				}
			}
		}

		public bool Equals(ColumnEntity other)
		{
			return this.GetHashCode().Equals(other.GetHashCode());
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != typeof(ColumnEntity))
				return false;
    		
			ColumnEntity other = (ColumnEntity)obj;
			return _columnName == other._columnName;
		}

		public override int GetHashCode()
		{
			unchecked {
				return (_columnName != null ? _columnName.GetHashCode() : 0);
			}
		}

		internal void RefreshWarningStatus()
		{
			if (IsForeignKey && ForeignKey.Keys.Count == 0) {
				HasWarning = true;
				WarningText = "This column is referencing a table without foreign keys. Insertion might fail unless you use the Identity from item# generator.";
			} else {
				HasWarning = false;
				WarningText = string.Empty;
			}
		}
	}
}
