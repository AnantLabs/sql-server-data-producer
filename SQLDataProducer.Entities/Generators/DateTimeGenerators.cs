// Copyright 2012 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Collections.ObjectModel;
using SQLDataProducer.Entities.Generators.Collections;
using SQLDataProducer.Entities.DatabaseEntities;


namespace SQLDataProducer.Entities.Generators
{
    public partial class Generator
    {
        public static readonly string GENERATOR_CurrentDate = "Current Date";
        public static readonly string GENERATOR_StaticDate = "Static Date";
        public static readonly string GENERATOR_SQLGetDate = "SQL GetDate()";
        public static readonly string GENERATOR_RandomDate = "Random Date";
        public static readonly string GENERATOR_SecondsSeries = "Seconds Series";
        public static readonly string GENERATOR_MilisecondsSeries = "Miliseconds Series";
        public static readonly string GENERATOR_MinutesSeries = "Minutes Series";
        public static readonly string GENERATOR_HoursSeries = "Hours Series";
        public static readonly string GENERATOR_DaysSeries = "Days Series";
        public static readonly string GENERATOR_ValueFromOtherDateTimeColumn = "Value from other Column";

        private static DateTime _currentDate = new DateTime(DateTime.Now.Year -1, 12, 31);
        private static DateTime StartDate
        {
            get
            {
                return _currentDate;
            }
            set 
            {
                if (_currentDate != value)
                    _currentDate = value; 
            }
            
        }


        public static ObservableCollection<Generator> GetDateTimeGenerators()
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
            valueGenerators.Add(CreateValueFromOtherDateTimeColumnGenerator());
            
            return valueGenerators;
        }

        private static Generator CreateCurrentDateGenerator()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("Shift Hours", 0));
            paramss.Add(new GeneratorParameter("Shift Minutes", 0));
            paramss.Add(new GeneratorParameter("Shift Seconds", 0));
            paramss.Add(new GeneratorParameter("Shift Milliseconds", 0));

            Generator gen = new Generator(GENERATOR_CurrentDate, (n, p) =>
            {
                int h = int.Parse(GetParameterByName(p, "Shift Hours").ToString());
                int min = int.Parse(GetParameterByName(p, "Shift Minutes").ToString());
                int s = int.Parse(GetParameterByName(p, "Shift Seconds").ToString());
                int ms = int.Parse(GetParameterByName(p, "Shift Milliseconds").ToString());

                return Wrap(DateTime.Now.AddDays(n).AddHours(h).AddMinutes(min).AddSeconds(s).AddMilliseconds(ms));
            }
                , paramss);
            return gen;
        }

        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.DateTime)]
        private static Generator CreateStaticDateGenerator()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            
            paramss.Add(new GeneratorParameter("DATE", DateTime.Now.ToString()));

            Generator gen = new Generator(GENERATOR_StaticDate, (n, p) =>
            {
                return Wrap(GetParameterByName(p, "DATE").ToString());
            }
                , paramss);
            return gen;
        }

        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.DateTime)]
        private static Generator CreateSQLGetDateGenerator()
        {
            Generator gen = new Generator(GENERATOR_SQLGetDate, (n, p) =>
            {
                return "Getdate()";
            }
                , null);
            return gen;
        }


        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.DateTime)]
        private static Generator CreateRandomDateGenerator()
        {
            Generator gen = new Generator(GENERATOR_RandomDate, (n, p) =>
            {
                return Wrap(StartDate.AddDays(RandomSupplier.Instance.GetNextInt() % 30));
            }
                , null);
            return gen;
        }

        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.DateTime)]
        private static Generator CreateSecondSeriesGenerator()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("Shift Hours", 0));
            paramss.Add(new GeneratorParameter("Shift Minutes", 0));
            paramss.Add(new GeneratorParameter("Shift Seconds", 0));
            paramss.Add(new GeneratorParameter("Shift Milliseconds", 0));

            Generator gen = new Generator(GENERATOR_SecondsSeries, (n, p) =>
            {
                int h = int.Parse(GetParameterByName(p, "Shift Hours").ToString());
                int min = int.Parse(GetParameterByName(p, "Shift Minutes").ToString());
                int s = int.Parse(GetParameterByName(p, "Shift Seconds").ToString());
                int ms = int.Parse(GetParameterByName(p, "Shift Milliseconds").ToString());

                return Wrap(StartDate.AddDays(n).AddHours(h).AddMinutes(min).AddSeconds(s).AddMilliseconds(ms));
            }
                , paramss);
            return gen;
        }

        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.DateTime)]
        private static Generator CreateMiliSecondSeriesGenerator()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("Shift Hours", 0));
            paramss.Add(new GeneratorParameter("Shift Minutes", 0));
            paramss.Add(new GeneratorParameter("Shift Seconds", 0));
            paramss.Add(new GeneratorParameter("Shift Milliseconds", 0));

            Generator gen = new Generator(GENERATOR_MilisecondsSeries, (n, p) =>
            {
                int h = int.Parse(GetParameterByName(p, "Shift Hours").ToString());
                int min = int.Parse(GetParameterByName(p, "Shift Minutes").ToString());
                int s = int.Parse(GetParameterByName(p, "Shift Seconds").ToString());
                int ms = int.Parse(GetParameterByName(p, "Shift Milliseconds").ToString());

                return Wrap(StartDate.AddDays(n).AddHours(h).AddMinutes(min).AddSeconds(s).AddMilliseconds(ms));
            }
                , paramss);
            return gen;
        }

        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.DateTime)]
        private static Generator CreateMinutesSeriesGenerator()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("Shift Hours", 0));
            paramss.Add(new GeneratorParameter("Shift Minutes", 0));
            paramss.Add(new GeneratorParameter("Shift Seconds", 0));
            paramss.Add(new GeneratorParameter("Shift Milliseconds", 0)); ;
            

            Generator gen = new Generator(GENERATOR_MinutesSeries, (n, p) =>
            {
                int h = int.Parse(GetParameterByName(p, "Shift Hours").ToString());
                int min = int.Parse(GetParameterByName(p, "Shift Minutes").ToString());
                int s = int.Parse(GetParameterByName(p, "Shift Seconds").ToString());
                int ms = int.Parse(GetParameterByName(p, "Shift Milliseconds").ToString());

                return Wrap(StartDate.AddDays(n).AddHours(h).AddMinutes(min).AddSeconds(s).AddMilliseconds(ms));
            }
                , paramss);
            return gen;
        }


        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.DateTime)]
        private static Generator CreateHoursSeriesGenerator()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("Shift Hours", 0));
            paramss.Add(new GeneratorParameter("Shift Minutes", 0));
            paramss.Add(new GeneratorParameter("Shift Seconds", 0));
            paramss.Add(new GeneratorParameter("Shift Milliseconds", 0));

            Generator gen = new Generator(GENERATOR_HoursSeries, (n, p) =>
            {
                int h = int.Parse(GetParameterByName(p, "Shift Hours").ToString());
                int min = int.Parse(GetParameterByName(p, "Shift Minutes").ToString());
                int s = int.Parse(GetParameterByName(p, "Shift Seconds").ToString());
                int ms = int.Parse(GetParameterByName(p, "Shift Milliseconds").ToString());

                return Wrap(StartDate.AddDays(n).AddHours(h).AddMinutes(min).AddSeconds(s).AddMilliseconds(ms));
            }
                , paramss);
            return gen;
        }

        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.DateTime)]
        private static Generator CreateDaysSeriesGenerator()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("Shift Hours", 0));
            paramss.Add(new GeneratorParameter("Shift Minutes", 0));
            paramss.Add(new GeneratorParameter("Shift Seconds", 0));
            paramss.Add(new GeneratorParameter("Shift Milliseconds", 0));
            
            Generator gen = new Generator(GENERATOR_DaysSeries, (n, p) =>
            {
                int h = int.Parse(GetParameterByName(p, "Shift Hours").ToString());
                int min = int.Parse(GetParameterByName(p, "Shift Minutes").ToString());
                int s = int.Parse(GetParameterByName(p, "Shift Seconds").ToString());
                int ms = int.Parse(GetParameterByName(p, "Shift Milliseconds").ToString());
                
                return Wrap(StartDate.AddDays(n).AddHours(h).AddMinutes(min).AddSeconds(s).AddMilliseconds(ms));
            }
                , paramss);
            return gen;
        }


        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.DateTime)]
        private static Generator CreateValueFromOtherDateTimeColumnGenerator()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            paramss.Add(new GeneratorParameter("Referenced Column", null));
            
            paramss.Add(new GeneratorParameter("Shift Hours", 0));
            paramss.Add(new GeneratorParameter("Shift Minutes", 0));
            paramss.Add(new GeneratorParameter("Shift Seconds", 0));
            paramss.Add(new GeneratorParameter("Shift Milliseconds", 0));
            

            Generator gen = new Generator(GENERATOR_ValueFromOtherDateTimeColumn, (n, p) =>
            {
                ColumnEntity otherColumn = GetParameterByName(p, "Referenced Column") as ColumnEntity;

                if (otherColumn != null && otherColumn.PreviouslyGeneratedValue != null)
                {
                    DateTime a;
                    if (!DateTime.TryParse(otherColumn.PreviouslyGeneratedValue.ToString().Replace("'", ""), out a))
                        return Wrap("NULL"); // TODO: What to do if it fails? Exception? Logg?

                    int h = int.Parse(GetParameterByName(p, "Shift Hours").ToString());
                    int min = int.Parse(GetParameterByName(p, "Shift Minutes").ToString());
                    int s = int.Parse(GetParameterByName(p, "Shift Seconds").ToString());
                    int ms = int.Parse(GetParameterByName(p, "Shift Milliseconds").ToString());

                    return Wrap(a.AddDays(n).AddHours(h).AddMinutes(min).AddSeconds(s).AddMilliseconds(ms));
                }


                return Wrap("NULL");
            }
                , paramss);
            return gen;
        }

    }
}
