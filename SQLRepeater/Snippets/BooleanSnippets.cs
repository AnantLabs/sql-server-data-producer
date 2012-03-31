using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.Snippets
{
    public static class BooleanSnippets
    {

        public static System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate> Snippets { get; set; }

        static BooleanSnippets()
        {
            Snippets = new System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate>();
            Snippets.Add(RandomBoolean);
            Snippets.Add(EveryOtherTrueEveryOtherFalse);
        }

        public static string EveryOtherTrueEveryOtherFalse(int n)
        {
            return (n % 2 == 0).ToString();
        }

        public static string RandomBoolean(int n)
        {
            return (RandomSupplier.Instance.GetNextInt() % 2 == 0).ToString();
        }
    }
}
