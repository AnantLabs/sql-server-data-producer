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
        private static DateTime CurrentDate { 
            get
            {
                return _currentDate;
            }
        }

        static DateTimeSnippets()
        {
            Snippets = new ObservableCollection<ValueCreatorDelegate>();
            Snippets.Add(CurrentDateSnippet);
            Snippets.Add(RandomDateSnippet);
            Snippets.Add(DaySeriesSnippet);
            Snippets.Add(HourSeriesSnippet);
            Snippets.Add(MinutesSeriesSnippet);
            Snippets.Add(SecondsSeriesSnippet);
        }      

        public static string CurrentDateSnippet(int n)
        {
            return DateTime.Now.ToString();
        }

        public static string RandomDateSnippet(int n)
        {
            return CurrentDate.AddDays(RandomSupplier.Randomer.Next() % 30).ToString();
        }

        public static string SecondsSeriesSnippet(int n)
        {
            return CurrentDate.AddSeconds(n).ToString();
        }

        public static string MinutesSeriesSnippet(int n)
        {
            return CurrentDate.AddMinutes(n).ToString();
        }

        public static string HourSeriesSnippet(int n)
        {
                return CurrentDate.AddHours(n).ToString();
        }

        public static string DaySeriesSnippet(int n)
        {
             return CurrentDate.AddDays(n).ToString();
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
