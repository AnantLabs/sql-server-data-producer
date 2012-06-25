using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SQLRepeater.Controls.GeneratorConfigurationControls
{
    public class ConfigurationViewFactory
    {
        internal static System.Windows.Controls.Control GetConfiguratorForColumn(object p)
        {
            //if (p is BooleanParameter)
            //{
            //    return new BooleanConfiguration(p as BooleanParameter);
            //}
            //else if (p is DateTimeParameter)
            //{
            //    return new DateTimeConfiguration(p as DateTimeParameter);
            //}
            //else if (p is IntParameter)
            //{
            //    return new IntConfiguration(p as IntParameter);
            //}
            //else if (p is StringParameter)
            //{
            //    return new StringConfiguration(p as StringParameter);
            //}
            //else if (p is DecimalParameter)
            //{
            //    return new DecimalConfiguration(p as DecimalParameter);
            //}

            throw new ArgumentOutOfRangeException("the supplied Parameter type is not supported");
        }
    }
}
