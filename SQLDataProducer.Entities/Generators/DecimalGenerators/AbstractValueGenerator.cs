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
using SQLDataProducer.Entities.Generators.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Entities.Generators.DecimalGenerators
{
    public abstract class AbstractValueGenerator
    {
        ///// <summary>
        ///// to hold The method used to generate values
        ///// </summary>
        //protected ValueCreatorDelegate ValueGenerator { get; set; }

        GeneratorParameterCollection _genParameters;
        /// <summary>
        /// get The parameters for the generator
        /// </summary>
        public GeneratorParameterCollection GeneratorParameters
        {
            get
            {
                return _genParameters;
            }
            protected set
            {
                if (_genParameters != value)
                {
                    _genParameters = value;
                    OnPropertyChanged("GeneratorParameters");
                }
            }
        }


        string _generatorName;
        /// <summary>
        /// Get name of generator
        /// </summary>
        public string GeneratorName
        {
            get
            {
                return _generatorName;
            }
            protected set
            {
                if (_generatorName != value)
                {
                    _generatorName = value;
                    if (string.IsNullOrEmpty(GeneratorHelpText))
                        GeneratorHelpText = GeneratorHelpTextManager.GetGeneratorHelpText(_generatorName);
                    OnPropertyChanged("GeneratorName");
                }
            }
        }
        private string _generatorHelpText;
        /// <summary>
        /// get Help text for the generator
        /// </summary>
        public string GeneratorHelpText
        {
            get
            {
                return _generatorHelpText;
            }
            protected set
            {
                _generatorHelpText = value;
                OnPropertyChanged("GeneratorHelpText");
            }
        }


        /// <summary>
        /// Column where this generator is attached.
        /// </summary>
        ColumnEntity _parentColumn;
        /// <summary>
        /// Get Set the Column of whom this generator belong to.
        /// </summary>
        public ColumnEntity ParentColumn
        {
            get
            {
                return _parentColumn;
            }
            set
            {
                if (_parentColumn != value)
                {
                    _parentColumn = value;
                    OnPropertyChanged("ParentColumn");
                }
            }
        }

        bool _isTakingValueFromOtherColumn = false;
        /// <summary>
        /// true If this generator is taking value from another column instead of generating its own
        /// Used for identity values and other similar
        /// </summary>
        public bool IsTakingValueFromOtherColumn
        {
            get
            {
                return _isTakingValueFromOtherColumn;
            }
        }

        protected AbstractValueGenerator(string generatorName)
        {
            GeneratorParameters = new GeneratorParameterCollection();
            GeneratorName = generatorName;
            GeneratorHelpText = GeneratorHelpTextManager.GetGeneratorHelpText(generatorName);
        }

        /// <summary>
        /// returns GeneratorName
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("<GeneratorName = '{0}', GeneratorParameters = '{1}'>", GeneratorName, this.GeneratorParameters);
        }

        protected abstract object InternalGenerateValue(long n, GeneratorParameterCollection paramas);

        protected abstract object ApplyTypeSpecificLimits(object value);
        
        public object GenerateValue(long n)
        {
            return ApplyTypeSpecificLimits(InternalGenerateValue(n, GeneratorParameters));
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override bool Equals(System.Object obj)
        {
            AbstractValueGenerator p = obj as AbstractValueGenerator;
            if ((object)p == null)
                return false;

            return GetHashCode().Equals(p.GetHashCode());
        }

        public bool Equals(AbstractValueGenerator other)
        {
            return
                this.GeneratorName.Equals(other.GeneratorName);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 37;
                hash = hash * 23 + GeneratorName.GetHashCode();
                return hash;
            }
        }
    }
}
