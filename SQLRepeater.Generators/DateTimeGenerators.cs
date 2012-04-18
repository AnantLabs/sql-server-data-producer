using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SQLRepeater.Generators
{
    public class DateTimeGenerator : GeneratorBase
    {

        private static DateTime _currentDate = DateTime.Now;
        private static DateTime StartDate { 
            get
            {
                return _currentDate;
            }
        }

        static DateTimeGenerator()
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

        public static object CurrentDate(int n, object param)
        {
            return Wrap(DateTime.Now);
        }

        public static object RandomDates(int n, object param)
        {
            return Wrap(StartDate.AddDays(RandomSupplier.Instance.GetNextInt() % 30));
        }

        public static object SecondsSeries(int n, object param)
        {
            return Wrap(StartDate.AddSeconds(n));
        }
        public static object MiliSecondSeries(int n, object param)
        {
            return Wrap(StartDate.AddMilliseconds(n));
        }
        public static object MinutesSeries(int n, object param)
        {
            return Wrap(StartDate.AddMinutes(n));
        }

        public static object HourSeries(int n, object param)
        {
            return Wrap(StartDate.AddHours(n));
        }

        public static object DaySeries(int n, object param)
        {
            return Wrap(StartDate.AddDays(n));
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
