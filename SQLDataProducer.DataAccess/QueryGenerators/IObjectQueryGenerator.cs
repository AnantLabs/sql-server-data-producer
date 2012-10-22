using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.DataAccess.QueryGenerators
{
    public interface IObjectQueryGenerator
    {
        Func<bool> GetInsertionQuery();
    }
}
