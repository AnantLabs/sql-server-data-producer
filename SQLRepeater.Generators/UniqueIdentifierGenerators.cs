using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.Generators
{
    public class UniqueIdentifierGenerator : GeneratorBase
    {
        public UniqueIdentifierGenerator()
        {

        }
        
        #region Static Members
        public static System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate> Generators { get; set; }

        static UniqueIdentifierGenerator()
        {
            Generators = new System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate>();
            Generators.Add(NewGuid);
        }

        public static object NewGuid(int n, object param)
        {
            return Wrap(new Guid());
        } 
        #endregion



    }
}
