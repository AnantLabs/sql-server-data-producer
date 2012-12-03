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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace SQLDataProducer.Helpers
{
    static class NotepadHelper
    {
        private class Notepad
        {
            [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
            public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

            [DllImport("User32.dll")]
            public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

            public static void ShowInNotepad(string message)
            {
                Process notepad = Process.Start(new ProcessStartInfo("notepad.exe"));
                notepad.WaitForInputIdle();

                if (notepad != null)
                {
                    IntPtr child = FindWindowEx(notepad.MainWindowHandle, new IntPtr(0), "Edit", null);
                    SendMessage(child, 0x000C, 0, message);
                }
            }
        }

        public static void ShowInNotepad(string message)
        {
            Notepad.ShowInNotepad(message);
        }
    }
}
