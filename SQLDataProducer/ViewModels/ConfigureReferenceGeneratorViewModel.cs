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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDataProducer.Model;
using SQLDataProducer.Entities.DatabaseEntities;

namespace SQLDataProducer.ViewModels
{
    public class ConfigureReferenceGeneratorViewModel : ViewModelBase, IYesNoViewModel
    {

        ApplicationModel _model;
        public ApplicationModel Model
        {
            get
            {
                return _model;
            }
            set
            {
                if (_model != value)
                {
                    _model = value;
                    OnPropertyChanged("Model");
                }
            }
        }


        ColumnEntity _selectedColumn;
        public ColumnEntity ColumnToRefer
        {
            get
            {
                return _selectedColumn;
            }
            set
            {
                if (_selectedColumn != value)
                {
                    _selectedColumn = value;
                    OnPropertyChanged("ColumnToRefer");
                }
            }
        }

        public ConfigureReferenceGeneratorViewModel(ApplicationModel model, SQLDataProducer.Entities.Generators.GeneratorParameter param)
        {
            Model = model;
            OKAction = () =>
                {
                    param.Value = ColumnToRefer;
                };
        }

        public Action OKAction { get; set; }

        //public Action CancelAction { get; set; }

        public void OnYes()
        {
            OKAction();
        }

        public void OnNo()
        {
          //  CancelAction();
        }
    }
}
