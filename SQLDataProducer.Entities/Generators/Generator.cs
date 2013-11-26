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
using System.ComponentModel;
using SQLDataProducer.Entities.Generators.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using SQLDataProducer.Entities.DatabaseEntities;

namespace SQLDataProducer.Entities.Generators
{
    public partial class Generator : INotifyPropertyChanged, IEquatable<Generator>
    {
       /// <summary>
       /// to hold The method used to generate values
       /// </summary>
        protected ValueCreatorDelegate ValueGenerator { get; set; }

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
            private set
            {
                if (_generatorName != value)
                {
                    _generatorName = value;
                    if(string.IsNullOrEmpty(GeneratorHelpText))
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
            private set
            {
                _generatorHelpText = value;
                OnPropertyChanged("GeneratorHelpText");
            }
        }


        bool _isSqlQueryGenerator = false;
        // Gets whether this generator is generating pure sql queries
        public bool IsSqlQueryGenerator
        {
            get
            {
                return _isSqlQueryGenerator;
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
       public bool IsTakingValueFromOtherColumn
       {
           get
           {
               return _isTakingValueFromOtherColumn;
           }
           //set
           //{
           //    if (_isTakingValueFromOtherColumn != value)
           //    {
           //        _isTakingValueFromOtherColumn = value;
           //        OnPropertyChanged("IsTakingValueFromOtherColumn");
           //    }
           //}
       }
        


        public Generator(string generatorName, ValueCreatorDelegate generator, GeneratorParameterCollection genParams, bool isSqlQueryGenerator = false)
        {
            ValueGenerator = generator;
            GeneratorParameters = genParams ?? new GeneratorParameterCollection();
            GeneratorName = generatorName;
            GeneratorHelpText = GeneratorHelpTextManager.GetGeneratorHelpText(generatorName);
            _isSqlQueryGenerator = isSqlQueryGenerator;
        }

        /// <summary>
        /// Create an instance of a Generator without possibility to generate any values.
        /// </summary>
        public Generator()
        {
            GeneratorParameters = new GeneratorParameterCollection();
        }

        

        /// <summary>
        /// returns GeneratorName
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("<GeneratorName = '{0}', IsSqlQueryGenerator = '{1}', GeneratorParameters = '{2}'>", GeneratorName, this.IsSqlQueryGenerator, this.GeneratorParameters);
        }

        /// <summary>
        /// Generate a value based on the parameter N
        /// </summary>
        /// <param name="n">generation number N</param>
        /// <returns>an object of the type specified by the type of generator</returns>
        public object GenerateValue(long n)
        {
            return ValueGenerator(n, GeneratorParameters);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal Generator Clone()
        {
            // TODO: Clone using the same entity as the LOAD/SAVE functionality
            var clonedParameters = this.GeneratorParameters.Clone();
            var gen = new Generator(this.GeneratorName, this.ValueGenerator, clonedParameters);
            gen._isSqlQueryGenerator = this.IsSqlQueryGenerator;
            return gen;
        }

        public static void InitGeneratorStartValues(OptionEntities.ExecutionTaskOptions options)
        {
            Generator.StartDate = options.DateTimeGenerationStartTime;
        }

        public override bool Equals(System.Object obj)
        {
            Generator p = obj as Generator;
            if ((object)p == null)
                return false;

            return GetHashCode().Equals(p.GetHashCode());
        }

        public bool Equals(Generator other)
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

    public static class GeneratorExtensions
    {
        internal static object GetParameterByName(this GeneratorParameterCollection paramas, string name)
        {
            return paramas.Where(x => x.ParameterName == name).First().Value;
        }

    }
}
