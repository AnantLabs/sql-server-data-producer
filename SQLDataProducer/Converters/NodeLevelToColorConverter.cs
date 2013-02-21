using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SQLDataProducer.Converters
{
    class NodeLevelToColorConverter: IValueConverter
    {
       
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                int val = (int)value;
                switch (val)
                {
                    case 1:
                        return System.Windows.Media.Brushes.Azure;
                    case 2:
                        return System.Windows.Media.Brushes.LightBlue;
                    case 3:
                        return System.Windows.Media.Brushes.LightGreen;
                    case 4:
                        return System.Windows.Media.Brushes.OldLace;

                    default:
                        return System.Windows.Media.Brushes.WhiteSmoke;
                }

            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                return 1;
            }
        
    }
}
