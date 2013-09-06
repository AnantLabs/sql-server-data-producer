using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataProducer.Entities
{
    public static class ValidateUtil
    {
        /// <summary>
        /// Validate the parameter <paramref name="o"/> and throws a ArgumentNullException if it is null, using <paramref name="paramName"/> as parameter
        /// </summary>
        /// <param name="o"></param>
        /// <param name="paramName"></param>
        public static void ValidateNotNull(object o, string paramName)
        {
            if (o == null)
                throw new ArgumentNullException(paramName);
        }
    }
}
