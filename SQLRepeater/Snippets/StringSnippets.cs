using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.Snippets
{
    public class StringSnippets 
    {
        public static System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate> Snippets { get; set; }

        static StringSnippets()
        {
            Snippets = new System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate>();
            Snippets.Add(CountrySnippet);
            Snippets.Add(FemaleNameSnippet);
            Snippets.Add(MaleNameSnippet);
        }

        private static List<string> _countries;
        static List<string> Countries
        {
            get
            {
                if (_countries == null)
                {
                    _countries = new List<string>();
                    _countries.AddRange(System.IO.File.ReadAllLines(@".\Snippets\resources\Countries.txt"));
                }
                return _countries;
            }
        }

        private static List<string> _females;
        static List<string> Females
        {
            get
            {
                if (_females == null)
                {
                    _females = new List<string>();
                    _females.AddRange(System.IO.File.ReadAllLines(@".\Snippets\resources\FemaleNames.txt"));
                }
                return _females;
            }
        }
        private static List<string> _males;
        static List<string> Males
        {
            get
            {
                if (_males == null)
                {
                    _males = new List<string>();
                    _males.AddRange(System.IO.File.ReadAllLines(@".\Snippets\resources\MaleNames.txt"));
                }
                return _males;
            }
        }


        public static string CountrySnippet(int n)
        {
            return Countries[n % Countries.Count];
        }
        public static string FemaleNameSnippet(int n)
        {
            return Females[n % Females.Count];
        }
        public static string MaleNameSnippet(int n)
        {
            return Males[n % Males.Count];
        }

    }
}
