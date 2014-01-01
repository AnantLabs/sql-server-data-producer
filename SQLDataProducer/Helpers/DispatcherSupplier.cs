// Copyright 2012-2014 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System.Security.Permissions;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace SQLDataProducer.Helpers
{
    public static class DispatcherSupplier
    {
        static Dispatcher disp = null;
        public static Dispatcher CurrentDispatcher
        {
            get
            {
                if (disp == null)
                {
                    if (Application.Current == null)
                    {
                        disp = Dispatcher.FromThread(Thread.CurrentThread);
                    }
                    else
                    {
                        disp = Application.Current.Dispatcher;
                    }
                }
                return disp;
            }
        }

        public static class DispatcherUtil
        {
            [SecurityPermissionAttribute(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            public static void DoEvents()
            {
                DispatcherFrame frame = new DispatcherFrame();
                DispatcherSupplier.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                    new DispatcherOperationCallback(ExitFrame), frame);
                Dispatcher.PushFrame(frame);
            }

            private static object ExitFrame(object frame)
            {
                ((DispatcherFrame)frame).Continue = false;
                return null;
            }
        }
    }


}
