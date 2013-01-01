// Copyright 2012-2013 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;

namespace ConnectionStringCreatorGUI
{
    internal class ConnectionStringBuilderWindowViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public DelegateCommand OkClickCommand { get; private set; }

        // Action to perform when the window closes
        private Action<SqlConnectionString> _exitReturnAction;

        private SqlConnectionString _connString;
        public SqlConnectionString ConnectionString { 
            get 
            {
                return _connString;
            }
            set {
                
                if (_connString == value)
                    return;
                
                _connString = value;
                OnPropertyChanged("ConnectionString");
            }
        }

        bool _isOpen = true;
        public bool IsOpen
        {
            get
            {
                return _isOpen;
            }
            private set
            {
                if (_isOpen == value)
                    return;

                _isOpen = value;
                OnPropertyChanged("IsOpen");
            }
        }


        public ConnectionStringBuilderWindowViewModel(ConnectionStringCreatorGUI.SqlConnectionString connStr, Action<SqlConnectionString> action)
        {
            ConnectionString = connStr ?? new SqlConnectionString();
            _exitReturnAction = action;

            OkClickCommand = new DelegateCommand(() =>
                {
                    string connectionString = ConnectionString.ToString();
                    IsOpen = false;
                    _exitReturnAction(ConnectionString);
                });
        }


        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

    }
}
