﻿using System.Data;
using System.Data.Common;


namespace SQLDataProducer.ContinuousInsertion.DataAccess
{

    public static class CommandFactory
    {

        /// <summary>
        /// Create DbCommand using the configured DbFactory, the commandType will be CommandType.Text
        /// </summary>
        /// <param name="sqlQuery">the query to use in the command</param>
        /// <param name="connStr">the connection string to use in the connection of the command</param>
        /// <returns>A DbCommand with a closed connection</returns>
        public static DbCommand Create(string connectionString)
        {
            return Create(connectionString);
        }

        ///// <summary>
        ///// Create DbCommand using the configured DbFactory
        ///// </summary>
        ///// <param name="sqlQuery">the query to use in the command</param>
        ///// <param name="connStr">the connection string to use in the connection of the command</param>
        ///// <param name="commandType">the type of command this should be</param>
        ///// <returns>A DbCommand with a closed connection</returns>
        //public static DbCommand Create(string sqlQuery, string connStr, CommandType commandType)
        //{
        //    DbConnection conn = ConnectionFactory.Create(connStr);
        //    DbCommand cmd = conn.CreateCommand();
        //    cmd.CommandText = sqlQuery;
        //    cmd.CommandType = commandType;
        //    return cmd;
        //}

        /// <summary>
        /// Create DbParameter using the configured DbFactory
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DbParameter CreateParameter(string name, object value)
        {
            DbParameter param = DbProviderFactoryFactory.GetInstance().CreateParameter();
            param.Value = value;
            param.ParameterName = name;

            return param;
        }

        /// <summary>
        /// Factory to create DbConnections
        /// </summary>
        private static class ConnectionFactory
        {
            public static DbConnection Create(string connStr)
            {
                DbProviderFactory factory = DbProviderFactoryFactory.GetInstance();
                DbConnection conn = factory.CreateConnection();
                conn.ConnectionString = connStr;

                return conn;
            }
        }


        /// <summary>
        /// Singleton DbProviderFactory
        /// </summary>
        private static class DbProviderFactoryFactory
        {
            private static DbProviderFactory factory = null;
            public static DbProviderFactory GetInstance()
            {
                if (factory == null)
                {
                    // http://msdn.microsoft.com/en-us/library/dd0w4a2z.aspx

                    // This configuration is needed if you are going to use SqlClient.
                    // If you want to use any other client like MySql then you need to modify this setting.
                    //<appSettings>
                    //  <add key="DbConnFactory" value="System.Data.SqlClient" />
                    //</appSettings>
                    string provider = "System.Data.SqlClient";
                    if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["DbConnFactory"]))
                    {
                        provider = System.Configuration.ConfigurationManager.AppSettings["DbConnFactory"];
                    }
                    factory = DbProviderFactories.GetFactory(provider);
                }

                return factory;
            }
        }
    }

}
