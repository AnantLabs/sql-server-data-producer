using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.Snippets
{
    public class SmallIntSnippets 
    {
        public static System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate> Snippets { get; set; }

        static SmallIntSnippets()
        {
            Snippets = new System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate>();
            Snippets.Add(UpCounter);
            Snippets.Add(DownCounter);
            Snippets.Add(RandomSmallInt);
        }

        public static string UpCounter(int n)
        {
            return (n % short.MaxValue).ToString();
        }

        public static string DownCounter(int n)
        {
            return (1-(n % short.MaxValue)).ToString();
        }

        public static string RandomSmallInt(int n)
        {
            return (RandomSupplier.Instance.GetNextInt() % short.MaxValue).ToString(); 
        }
    }
}
