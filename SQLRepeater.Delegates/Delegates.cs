using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater
{
    public delegate object ValueCreatorDelegate(int n, object genParameter);
    //public delegate string ValueCreatorDelegateForTable(int n, SQLRepeater.DatabaseEntities.Entities.TableEntity table, object genParameter);
}
