using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.Entities.OptionEntities
{
    public class ExecutionTaskOptionsManager : EntityBase
    {
        static readonly ExecutionTaskOptionsManager _instance = new ExecutionTaskOptionsManager();

        public static ExecutionTaskOptionsManager Instance
        {
            get
            {
                return _instance;
            }
        }

        ExecutionTaskOptions _options;
        public ExecutionTaskOptions Options
        {
            get
            {
                return _options;
            }
            set
            {
                if (_options != value)
                {
                    _options = value;
                    OnPropertyChanged("Options");
                }
            }
        }

        static ExecutionTaskOptionsManager()
        {        
        }

        ExecutionTaskOptionsManager()
        {
           Options = new ExecutionTaskOptions();
        }
    }
}
