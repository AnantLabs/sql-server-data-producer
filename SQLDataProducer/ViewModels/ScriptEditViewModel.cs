using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.ViewModels
{
    public class ScriptEditViewModel : ViewModelBase
    {

        string _scriptText;
        public string ScriptText
        {
            get
            {
                return _scriptText;
            }
            set
            {
                if (_scriptText != value)
                {
                    _scriptText = value;
                    OnPropertyChanged("ScriptText");
                }
            }
        }

    }
}
