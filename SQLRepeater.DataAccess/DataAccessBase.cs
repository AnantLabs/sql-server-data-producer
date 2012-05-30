using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace SQLRepeater.DataAccess
{
    public class DataAccessBase
    {
        protected string _connectionString = string.Empty;

        public DataAccessBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Asyncronously get one of T using the supplied <paramref name="sql"/> query string.
        /// </summary>
        /// <typeparam name="T">The type that will be retrieved</typeparam>
        /// <param name="sql">the query string to get the entity</param>
        /// <param name="itemBuilder">supply the Func that will be used to create one T by reading the fields from a SqlDataReader. The Func should return the created entity.</param>
        /// <param name="completedCallback">The Action which will be called when the retreiving of the entity is done</param>
        protected void BeginGetOne<T>(string sql, Func<SqlDataReader, T> itemBuilder, Action<T> completedCallback)
        {
            Action a = new Action(() =>
            {
                T m = GetOne<T>(sql, itemBuilder);
                completedCallback(m);
            });

            a.BeginInvoke(null, null);
        }


        protected void BeginExecuteNoResult(string sql)
        {
            Action a = new Action(() =>
                {
                    ExecuteNoResult(sql);
                });
        }

        protected void ExecuteNoResult(string sql)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Connection.Open();
                    cmd.ExecuteReader();
                }
            }
        }

        /// <summary>
        /// Asyncronously get many of T using the supplied <paramref name="sql"/> query string.
        /// </summary>
        /// <typeparam name="T">The type that will be retrieved</typeparam>
        /// <param name="sql">the query string to get the entities</param>
        /// <param name="itemBuilder">supply the Func that will be used to create one T by reading the fields from a SqlDataReader. The Func should return the created entity.</param>
        /// <param name="completedCallback">The Action which will be called when the retreiving of the entities is done. 
        /// The parameter to <paramref name="completedCallback"/> action will be the populated observerable collection of T</param>
        protected void BeginGetMany<T>(string sql, Func<SqlDataReader, T> itemBuilder, Action<ObservableCollection<T>> completedCallback)
        {
            Action a = new Action(() =>
            {
                ObservableCollection<T> items = GetMany(sql, itemBuilder);
                completedCallback(items);
            });

            a.BeginInvoke(null, null);
        }


        protected T GetOne<T>(string sql, Func<SqlDataReader, T> itemBuilder)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return itemBuilder(reader);
                    }

                }
            }
            return default(T);
        }

        protected ObservableCollection<T> GetMany<T>(string sql, Func<SqlDataReader, T> itemBuilder)
        {
            ObservableCollection<T> items = new ObservableCollection<T>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        items.Add(itemBuilder(reader));
                    }

                }
            }
            return items;
        }

    }
}
