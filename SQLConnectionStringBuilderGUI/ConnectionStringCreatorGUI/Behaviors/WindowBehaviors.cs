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

using System.Windows;

namespace ConnectionStringCreatorGUI.Behaviors
{
    internal static class WindowBehaviors
    {
        public static readonly DependencyProperty IsOpenProperty =
                 DependencyProperty.RegisterAttached("IsOpen", typeof(bool), typeof(WindowBehaviors),
                 new PropertyMetadata(IsOpenChanged));

        private static void IsOpenChanged(DependencyObject obj,
                                          DependencyPropertyChangedEventArgs args)
        {
            Window window = Window.GetWindow(obj);

            if (window != null && !((bool)args.NewValue))
                window.Close();
        }

        public static bool GetIsOpen(Window target)
        {
            return (bool)target.GetValue(IsOpenProperty);
        }

        public static void SetIsOpen(Window target, bool value)
        {
            target.SetValue(IsOpenProperty, value);
        }
    }
}
