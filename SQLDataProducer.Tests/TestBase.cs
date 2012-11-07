using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDataProducer.DataAccess;

namespace SQLDataProducer.RandomTests
{
    public class TestBase
    {
        public TestBase()
        {
            string sql = @"

if exists (select 1 from INFORMATION_SCHEMA.tables where TABLE_NAME = 'AnotherTable' and TABLE_SCHEMA = 'Person')
	drop table Person.AnotherTable;

if exists (select 1 from INFORMATION_SCHEMA.tables where TABLE_NAME = 'NewPerson' and TABLE_SCHEMA = 'Person')
	drop table Person.NewPerson;
	
	
create table Person.NewPerson(
	NewPersonId int identity(1, 1) primary key,
	Name varchar(500) not null,
	BitColumn bit not null, 
	DecimalColumn decimal(10, 4) not null,
	BigintColumn bigint not null, 
	VarcharMaxColumn varchar(max)  not null,
	FloatColumn float not null,
	DateTime2Column datetime2 not null,
	DateTimeColumn datetime not null,
	NCharFiveColumn nchar(5) not null,
	DateColumn date not null, 
	TimeColumn time not null,
	SmallIntColumn smallint not null,
	SmallDateTimeColumn smalldatetime not null,
	SmallMoneyColumn smallmoney  not null
);

create table Person.AnotherTable(
	NewPersonId int foreign key references Person.NewPerson(NewPersonId),
	AnotherColumn char(1),
    ThirdColumn char(1) not null
);";
            AdhocDataAccess adhd = new AdhocDataAccess(Connection());
            adhd.ExecuteNonQuery(sql);

        }
        public static string Connection()
        {
            return "Data Source=localhost;Initial Catalog=AdventureWorks;Integrated Security=True";
        }
    }
}
