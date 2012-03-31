using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SQLRepeater.Snippets
{
    public class DateTimeSnippets
    {

        private static DateTime _currentDate = DateTime.Now;
        private static DateTime StartDate { 
            get
            {
                return _currentDate;
            }
        }

        static DateTimeSnippets()
        {
            Snippets = new ObservableCollection<ValueCreatorDelegate>();
            Snippets.Add(CurrentDate);
            Snippets.Add(RandomDates);
            Snippets.Add(DaySeries);
            Snippets.Add(HourSeries);
            Snippets.Add(MinutesSeries);
            Snippets.Add(SecondsSeries);
        }      

        public static string CurrentDate(int n)
        {
            return DateTime.Now.ToString();
        }

        public static string RandomDates(int n)
        {
            return StartDate.AddDays(RandomSupplier.Instance.GetNextInt() % 30).ToString();
        }

        public static string SecondsSeries(int n)
        {
            return StartDate.AddSeconds(n).ToString();
        }
        public static string MiliSecondSeries(int n)
        {
            return StartDate.AddMilliseconds(n).ToString();
        }
        public static string MinutesSeries(int n)
        {
            return StartDate.AddMinutes(n).ToString();
        }

        public static string HourSeries(int n)
        {
            return StartDate.AddHours(n).ToString();
        }

        public static string DaySeries(int n)
        {
             return StartDate.AddDays(n).ToString();
        }


        static ObservableCollection<ValueCreatorDelegate> _snippets;
        public static  ObservableCollection<ValueCreatorDelegate> Snippets
        {
            get
            {
                return _snippets;
            }
            set
            {
                if (_snippets != value)
                {
                    _snippets = value;
                }
            }
        }

       
    }
}
