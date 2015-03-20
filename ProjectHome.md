The goal of the SQL Data Producer is to have a tool that can inspect the design of the database and generate data for it's tables.

The tool is meant to handle data generation in the following scenarios:
  * To generate data needed for performance/index tuning. The data should be generated in a way that resembles the way applications will create data (random delay between rows, defined insert patterns, using foreign keys etc).
  * To generate data needed during development when table designs are changing frequently and the application developers require sample data in the tables for initial testing.
  * To generate data needed for unit testing.
  * To perform testing of insert patterns in a multithreaded system to see indications of deadlocks, other locking problems and to find missing indexes.

Key features:
  * Easy to start using.
  * Inserts data into the specified tables in the specified order
  * Each column can be configured to generate exactly the data that you wish to have
  * Supports foreign keys, identity inserts, use identity column from previous table as a value in another column.
  * Use the value generated from one column in another column.
  * Saving / Loading
  * Insert the data right away or generate a script that will insert the same data.
  * Scriptable API - Call the program from a Powershell script, your own application or anything you want
  * Loads of options and parameters, makes the application complex but makes it possible to create just the data you want

Column Data Generators:
  * Lots of different type of data generations for each sql datatype
  * Examples For Integer: Counting up, Random, Custom SQL Query
  * Examples for Datetime: Current Date, Hour Series(every row is one hour later than the previous row), Minutes series, Day Series etc.
  * Examples for varchar: Countries, Male and female names.