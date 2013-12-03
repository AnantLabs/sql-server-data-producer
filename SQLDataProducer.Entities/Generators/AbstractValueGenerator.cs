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

namespace SQLDataProducer.Entities.Generators
{
    public abstract class AbstractValueGenerator
    {
        protected AbstractValueGenerator(string generatorName)
        {
            GeneratorName = generatorName;
            GeneratorParameters = new GeneratorParameterCollection();
            GeneratorHelpText = GeneratorHelpTextManager.GetGeneratorHelpText(generatorName);
        }

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
            private set
            {
                if (_genParameters != value)
                {
                    _genParameters = value;
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
            private set
            {
                if (_generatorName != value)
                {
                    _generatorName = value;
                //    OnPropertyChanged("GeneratorName");
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
            private set
            {
                _generatorHelpText = value;
               // OnPropertyChanged("GeneratorHelpText");
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

        public override string ToString()
        {
            return string.Format("<GeneratorName = '{0}', GeneratorParameters = '{1}'>", GeneratorName, this.GeneratorParameters);
        }

        protected abstract object InternalGenerateValue(long n, GeneratorParameterCollection paramas);

        protected abstract object ApplyGeneratorTypeSpecificLimits(object value);
        
        public object GenerateValue(long n)
        {
            return ApplyGeneratorTypeSpecificLimits(InternalGenerateValue(n, GeneratorParameters));
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

        public AbstractValueGenerator Clone()
        {
            throw new NotImplementedException("Cloning not implemented");
        }
    }
}
