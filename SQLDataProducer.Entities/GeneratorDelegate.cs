using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDataProducer.Entities.Generators;
using System.Collections.ObjectModel;
using SQLDataProducer.Entities.Generators.Collections;
using SQLDataProducer.Entities.ExecutionEntities;

namespace SQLDataProducer.Entities
{
    // Functions that deliver an instance of something that can generate values
    //public delegate IValueCreator GeneratorDelegate();

    // Functions that can generate values using the serial number n and the supplied parameters
    public delegate object ValueCreatorDelegate(int n, GeneratorParameterCollection genParameters);

    //Func<string, IEnumerable<ExecutionItem>, string>
    public delegate string FinalQueryGeneratorDelegate(string baseQUery, IEnumerable<ExecutionItem> executionItems, int n);

    // Action<>
    public delegate void ExecutionTaskDelegate();

    //Action<int>
    public delegate void ExecutionDoneCallbackDelegate(int n);
}
