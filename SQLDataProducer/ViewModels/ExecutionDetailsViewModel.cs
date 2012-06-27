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

using SQLDataProducer.Entities.ExecutionEntities;

namespace SQLDataProducer.ViewModels
{
    public class ExecutionDetailsViewModel : ViewModelBase
    {

        SQLDataProducer.Model.ApplicationModel _model;
        public SQLDataProducer.Model.ApplicationModel Model
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


        ExecutionItem _execItem;
        public ExecutionItem ExecutionItem
        {
            get
            {
                return _execItem;
            }
            set
            {
                if (_execItem != value)
                {
                    _execItem = value;
                    OnPropertyChanged("ExecutionItem");
                }
            }
        }

        public ExecutionDetailsViewModel(SQLDataProducer.Model.ApplicationModel model, ExecutionItem execItem)
        {
            Model = model;
        }
    }
}
