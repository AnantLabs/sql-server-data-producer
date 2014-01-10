// Copyright 2012-2014 Peter Henell

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

namespace SQLDataProducer.ViewModels
{
    public abstract partial class ViewModelBase : NotifyingObject
    {
        public ViewModelBase(ProjectModel model)
        {
            this.model = model;
        }

        private ProjectModel model;
        public ProjectModel Model
        {
            get { return model; }
            set { 
                model = value;
                OnPropertyChanged("Model");
            }
        }
    }
}
