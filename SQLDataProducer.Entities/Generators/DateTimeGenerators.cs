using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SQLDataProducer.Entities.Generators.Collections;


namespace SQLDataProducer.Entities.Generators
{
    public partial class Generator
    {
        private static DateTime _currentDate = DateTime.Now;
        private static DateTime StartDate
        {
            get
            {
                return _currentDate;
            }
        }


        internal static ObservableCollection<Generator> GetDateTimeGenerators()
        {
            // TODO: Add edge case date generators. 1ms after midgnight, 1ms before midnight etc.
            ObservableCollection<Generator> valueGenerators = new ObservableCollection<Generator>();
            valueGenerators.Add(CreateCurrentDateGenerator());
            valueGenerators.Add(CreateStaticDateGenerator());
            valueGenerators.Add(CreateQueryGenerator());
            valueGenerators.Add(CreateDaysSeriesGenerator());
            valueGenerators.Add(CreateHoursSeriesGenerator());
            valueGenerators.Add(CreateMiliSecondSeriesGenerator());
            valueGenerators.Add(CreateMinutesSeriesGenerator());
            valueGenerators.Add(CreateRandomDateGenerator());
            valueGenerators.Add(CreateSecondSeriesGenerator());
            valueGenerators.Add(CreateSQLGetDateGenerator());
            
            return valueGenerators;
        }

        private static Generator CreateCurrentDateGenerator()
        {
            Generator gen = new Generator("Current Date", (n, p) =>
            {
                return Wrap(DateTime.Now.ToString());
            }
                , null);
            return gen;
        }

        private static Generator CreateStaticDateGenerator()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            
            paramss.Add(new GeneratorParameter("DATE", DateTime.Now.ToString()));

            Generator gen = new Generator("Static Date", (n, p) =>
            {
                return Wrap(GetParameterByName(p, "DATE").ToString());
            }
                , paramss);
            return gen;
        }

        private static Generator CreateSQLGetDateGenerator()
        {
            Generator gen = new Generator("SQL GetDate()", (n, p) =>
            {
                return "Getdate()";
            }
                , null);
            return gen;
        }


        private static Generator CreateRandomDateGenerator()
        {
            Generator gen = new Generator("Random Date", (n, p) =>
            {
                return Wrap(StartDate.AddDays(RandomSupplier.Instance.GetNextInt() % 30));
            }
                , null);
            return gen;
        }

        private static Generator CreateSecondSeriesGenerator()
        {
            Generator gen = new Generator("Seconds Series", (n, p) =>
            {
                return Wrap(StartDate.AddSeconds(n));
            }
                , null);
            return gen;
        }

        private static Generator CreateMiliSecondSeriesGenerator()
        {
            Generator gen = new Generator("Miliseconds Series", (n, p) =>
            {
                return Wrap(StartDate.AddMilliseconds(n));
            }
                , null);
            return gen;
        }

        private static Generator CreateMinutesSeriesGenerator()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            paramss.Add(new GeneratorParameter("Shift Seconds", 0));

            Generator gen = new Generator("Minutes Series", (n, p) =>
            {
                int shiftSeconds = int.Parse(GetParameterByName(p, "Shift Seconds").ToString());
                return Wrap(StartDate.AddMinutes(n).AddSeconds(shiftSeconds));
            }
                , paramss);
            return gen;
        }
        private static Generator CreateHoursSeriesGenerator()
        {
            Generator gen = new Generator("Hours Series", (n, p) =>
            {
                return Wrap(StartDate.AddHours(n));
            }
                , null);
            return gen;
        }
        private static Generator CreateDaysSeriesGenerator()
        {
            Generator gen = new Generator("Days Series", (n, p) =>
            {
                return Wrap(StartDate.AddHours(n));
            }
                , null);
            return gen;
        }
       



    }
}
