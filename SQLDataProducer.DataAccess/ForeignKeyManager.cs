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
using SQLDataProducer.DatabaseEntities.Entities;
using System.Collections.ObjectModel;
using SQLDataProducer.DataAccess;

namespace SQLDataProducer.EntityQueryGenerator
{
    public class ForeignKeyManager
    {

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

        public ObservableCollection<string> GetPrimaryKeysForTable(string connectionString, TableEntity table, string primaryKeyColumn)
        {
            ObservableCollection<string> fkeys;

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

        private ObservableCollection<string> GetKeysForColumnInTable(string connectionString, TableEntity table, string primaryKeyColumn)
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(connectionString);
            return tda.GetPrimaryKeysForColumnInTable(table, primaryKeyColumn);
        }

        ObservableCollection<ForeignKeyContainer> ForeignKeyContainerCache { get; set; }
    }

    class ForeignKeyContainer
    {
        private DatabaseEntities.Entities.TableEntity _table;
        public DatabaseEntities.Entities.TableEntity Table
        {
            get { return _table; }
            set { _table = value; }
        }

        private ObservableCollection<string> _keyValues;
        public ObservableCollection<string> KeyValues
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

        public ForeignKeyContainer(DatabaseEntities.Entities.TableEntity table, string keyColumnName, ObservableCollection<string> keys)
        {
            this._table = table;
            this._keyValues = keys;
            this._keyColumnName = keyColumnName;
        }
    }
}
