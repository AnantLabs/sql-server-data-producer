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

using System;
using System.Windows.Input;

namespace ConnectionStringCreatorGUI
{
    public class DelegateCommand<T> : ICommand
    {
        Action<T> _executeDelegate;

        public DelegateCommand(Action<T> executeDelegate)
        {
            _executeDelegate = executeDelegate;
        }

        public void Execute(T parameter)
        {
            _executeDelegate(parameter);
        }

        public bool CanExecute(object parameter) { return true; }
        public event EventHandler CanExecuteChanged;


        public void Execute(object parameter)
        {
            _executeDelegate((T)parameter);
        }
    }

    public class DelegateCommand : ICommand
    {
        Action _executeDelegate;

        public DelegateCommand(Action executeDelegate)
        {
            _executeDelegate = executeDelegate;
        }

        public void Execute()
        {
            _executeDelegate();
        }

        public bool CanExecute(object parameter) { return true; }
        public event EventHandler CanExecuteChanged;


        public void Execute(object parameter)
        {
            _executeDelegate();
        }
    }

}
