using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLRepeater.Entities.Generators;
using System.Collections.ObjectModel;
using SQLRepeater.Entities.Generators.Collections;
using SQLRepeater.Entities.ExecutionOrderEntities;

namespace SQLRepeater.Entities
{
    // Functions that deliver an instance of something that can generate values
    //public delegate IValueCreator GeneratorDelegate();

    // Functions that can generate values using the serial number n and the supplied parameters
    public delegate object ValueCreatorDelegate(int n, GeneratorParameterCollection genParameters);

    //Func<string, int, IEnumerable<ExecutionItem>, string>
    public delegate string FinalQueryGeneratorDelegate(string baseQUery, int n, IEnumerable<ExecutionItem> executionItems);

    // Action<int>
    public delegate void ExecutionTaskDelegate(int n);

    //Action<int>
    public delegate void ExecutionDoneCallbackDelegate(int n);
}
