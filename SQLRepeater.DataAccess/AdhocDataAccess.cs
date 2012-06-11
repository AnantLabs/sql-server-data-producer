using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.DataAccess
{
    public class AdhocDataAccess : DataAccessBase
    {
        public AdhocDataAccess(string connectionString)
            : base (connectionString)
        {

        }

        public void ExecuteNonQuery(string query)
        {
            base.ExecuteNoResult(query);
        }
    }
}
