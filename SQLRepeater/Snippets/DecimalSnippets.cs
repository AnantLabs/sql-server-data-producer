using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SQLRepeater.Snippets
{
    public class DecimalSnippets 
    {
        public static ObservableCollection<ValueCreatorDelegate> Snippets { get; set; }

        static DecimalSnippets()
        {
            Snippets = new ObservableCollection<ValueCreatorDelegate>();
            Snippets.Add(UpCounter);
            Snippets.Add(DownCounter);
            Snippets.Add(SmallRandomValues);
        }


        public static string UpCounter(int n)
        {
            return new Decimal(n).ToString();
        }

        public static string DownCounter(int n)
        {
            return new Decimal(0-n).ToString();
        }

        public static string SmallRandomValues(int n)
        {
            return new Decimal(RandomSupplier.Instance.GetNextInt() % 500).ToString();
        }

    }
}
