using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.DataAccess
{
    public class DatabaseDataAccess : DataAccessBase
    {

        public DatabaseDataAccess(string connectionString)
            : base(connectionString)
        {
        }

        public bool DoesDatabaseExist()
        {

            AdhocDataAccess adhc = new AdhocDataAccess(_connectionString);
           // adhc.ExecuteBoolQuery(sql);
            
            return false;
        }

    }
}
