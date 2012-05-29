using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace SQLRepeater.Converters
{

    /// <summary>
    /// A type converter for visibility and boolean values.
    /// </summary>
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            bool visibility = (bool)value;
            return visibility ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;
            return (visibility == Visibility.Visible);
        }
    }

}