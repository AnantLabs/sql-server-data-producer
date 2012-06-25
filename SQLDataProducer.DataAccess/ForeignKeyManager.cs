using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDataProducer.DatabaseEntities.Entities;
using System.Data.SqlClient;
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

        public ObservableCollection<int> GetPrimaryKeysForTable(string connectionString, TableEntity table, string primaryKeyColumn)
        {
            ObservableCollection<int> fkeys;

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

        private ObservableCollection<int> GetKeysForColumnInTable(string connectionString, TableEntity table, string primaryKeyColumn)
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

        private ObservableCollection<int> _keyValues;
        public ObservableCollection<int> KeyValues
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

        public ForeignKeyContainer(DatabaseEntities.Entities.TableEntity table, string keyColumnName, ObservableCollection<int> keys)
        {
            this._table = table;
            this._keyValues = keys;
            this._keyColumnName = keyColumnName;
        }
    }
}
