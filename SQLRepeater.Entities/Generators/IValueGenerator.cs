using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SQLRepeater.Entities.Generators
{
    public interface IValueCreator
    {
        object GenerateValue(int n);
        IValueCreator Clone();
        //static ObservableCollection<GeneratorDelegate> ValueCreators { get; set; }
    }
}
