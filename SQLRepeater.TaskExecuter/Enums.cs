using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.TaskExecuter
{
    public enum WorkFlowStates
    {
        NotStarted,
        Preparation,
        Execution,
        Finalize,
        Done,
        Error
    }
}
