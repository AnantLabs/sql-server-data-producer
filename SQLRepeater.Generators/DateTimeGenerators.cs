using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SQLRepeater.Generators
{
    public class DateTimeGenerators
    {

        private static DateTime _currentDate = DateTime.Now;
        private static DateTime StartDate { 
            get
            {
                return _currentDate;
            }
        }

        static DateTimeGenerators()
        {
            Generators = new ObservableCollection<ValueCreatorDelegate>();
            Generators.Add(CurrentDate);
            Generators.Add(RandomDates);
            Generators.Add(DaySeries);
            Generators.Add(HourSeries);
            Generators.Add(MinutesSeries);
            Generators.Add(SecondsSeries);
            Generators.Add(MiliSecondSeries);
        }

        public static string CurrentDate(int n, object param)
        {
            return DateTime.Now.ToString();
        }

        public static string RandomDates(int n, object param)
        {
            return StartDate.AddDays(RandomSupplier.Instance.GetNextInt() % 30).ToString();
        }

        public static string SecondsSeries(int n, object param)
        {
            return StartDate.AddSeconds(n).ToString();
        }
        public static string MiliSecondSeries(int n, object param)
        {
            return StartDate.AddMilliseconds(n).ToString();
        }
        public static string MinutesSeries(int n, object param)
        {
            return StartDate.AddMinutes(n).ToString();
        }

        public static string HourSeries(int n, object param)
        {
            return StartDate.AddHours(n).ToString();
        }

        public static string DaySeries(int n, object param)
        {
             return StartDate.AddDays(n).ToString();
        }


        static ObservableCollection<ValueCreatorDelegate> _Generators;
        public static  ObservableCollection<ValueCreatorDelegate> Generators
        {
            get
            {
                return _Generators;
            }
            set
            {
                if (_Generators != value)
                {
                    _Generators = value;
                }
            }
        }

       
    }
}
