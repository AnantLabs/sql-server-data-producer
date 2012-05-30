using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLRepeater.Entities.Generators;
using System.Collections.ObjectModel;
using SQLRepeater.Entities.Generators.Collections;

namespace SQLRepeater.Entities
{
    // Functions that deliver an instance of something that can generate values
    //public delegate IValueCreator GeneratorDelegate();

    // Functions that can generate values using the serial number n and the supplied parameters
    public delegate object ValueCreatorDelegate(int n, GeneratorParameterCollection genParameters);
}
