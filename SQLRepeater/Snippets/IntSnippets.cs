using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.Snippets
{
    public class IntSnippets 
    {
        public static System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate> Snippets { get; set; }

        static IntSnippets()
        {
            Snippets = new System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate>();
            Snippets.Add(UpCounter);
            Snippets.Add(DownCounter);
            Snippets.Add(RandomInt);
        }

        public static string UpCounter(int n)
        {
            return n.ToString();
        }

        public static string DownCounter(int n)
        {
         return (1-n).ToString();
        }

        public static string RandomInt(int n)
        {
            return RandomSupplier.Randomer.Next().ToString(); 
        }
    }
}
