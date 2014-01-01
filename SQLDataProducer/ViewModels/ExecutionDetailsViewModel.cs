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

using SQLDataProducer.Entities.DatabaseEntities;

using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.Entities.Generators;
using SQLDataProducer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SQLDataProducer.ViewModels
{
    public class ExecutionDetailsViewModel : ViewModelBase
    {

        public DelegateCommand<GeneratorParameter> ConfigureReferenceParameterCommand { private set; get; }
        public DelegateCommand<ExecutionNode> ShowPreviewCommand { private set; get; }
        

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

        public ExecutionDetailsViewModel(ApplicationModel model)
        {
            Model = model;
            ConfigureReferenceParameterCommand = new DelegateCommand<GeneratorParameter>(ShowParameterReferenceConfigurationWindow);

            ShowPreviewCommand = new DelegateCommand<ExecutionNode>(ei =>
                {
                    Object dt;// = ExecutionNode.CreatePreview(ei);
                    throw new NotImplementedException("Preview not implemented");
                    ModalWindows.ShowPreviewWindow win = new ModalWindows.ShowPreviewWindow(dt);
                    win.Show();
                    throw new NotImplementedException();
                });
        }

        private void ShowParameterReferenceConfigurationWindow(GeneratorParameter param)
        {
            ConfigureReferenceGeneratorViewModel configVM = new ConfigureReferenceGeneratorViewModel(Model, param);

            ModalWindows.YesNoWindow win = new ModalWindows.YesNoWindow(
                new SQLDataProducer.Views.ConfigureReferenceGeneratorView(configVM)
                , configVM);
            win.Show();
        }

        public ExecutionDetailsViewModel(SQLDataProducer.Model.ApplicationModel model, ExecutionNode execItem)
        {
            Model = model;
        }
    }
}
