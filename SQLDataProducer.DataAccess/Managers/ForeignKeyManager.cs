// Copyright 2012 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System.Linq;
using SQLDataProducer.Entities.DatabaseEntities;
using System.Collections.ObjectModel;
using SQLDataProducer.DataAccess;
using SQLDataProducer.Entities.DatabaseEntities.Collections;
using System;

namespace SQLDataProducer.EntityQueryGenerator
{
    public class ForeignKeyManager
    {
        ObservableCollection<ForeignKeyContainer> ForeignKeyContainerCache { get; set; }

        static readonly ForeignKeyManager _instance = new ForeignKeyManager();

        public static ForeignKeyManager Instance
        {
            get
            {
                return _instance;
            }
        }

        static ForeignKeyManager()
        {
        }

        ForeignKeyManager()
        {
            ForeignKeyContainerCache = new ObservableCollection<ForeignKeyContainer>();
        }

        /// <summary>
        /// When a new row is inserted in the table, add the key to the cache
        /// </summary>
        /// <param name="table"></param>
        /// <param name="insertedKey"></param>
        public void AddKeyToTable(TableEntity table, long insertedKey)
        {

        }

        public ForeignKeyCollection GetPrimaryKeysForTable(string connectionString, TableEntity table, string primaryKeyColumn)
        {
            ForeignKeyCollection fkeys;

            ForeignKeyContainer cached = ForeignKeyContainerCache.Where(x => x.Table == table).FirstOrDefault();
            if (cached != null)
            {
                fkeys = cached.KeyValues;
            }
            else
            {
                fkeys = GetKeysForColumnInTable(connectionString, table, primaryKeyColumn);
                ForeignKeyContainerCache.Add(new ForeignKeyContainer(table, primaryKeyColumn, fkeys));
            }

            return fkeys;
        }

        private ForeignKeyCollection GetKeysForColumnInTable(string connectionString, TableEntity table, string primaryKeyColumn)
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(connectionString);
            return tda.GetPrimaryKeysForColumnInTable(table, primaryKeyColumn);
        }

       
    }

    class ForeignKeyContainer
    {

        private TableEntity _table;
        public TableEntity Table
        {
            get { return _table; }
            set { _table = value; }
        }

        private ForeignKeyCollection _keyValues;
        public ForeignKeyCollection KeyValues
        {
            get { return _keyValues; }
            set { _keyValues = value; }
        }

        private string _keyColumnName;
        public string KeyColumnName
        {
            get { return _keyColumnName; }
            set { _keyColumnName = value; }
        }

        public ForeignKeyContainer(TableEntity table, string keyColumnName, ForeignKeyCollection keys)
        {
            this._table = table;
            this._keyValues = keys;
            this._keyColumnName = keyColumnName;
        }
    }
}
