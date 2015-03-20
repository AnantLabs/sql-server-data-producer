# Introduction #

The purpose of the DataProducer is to consider each ColumnEntity in a TableEntity and request the values from each Generator and put those into the ValueStore.

For each field to be inserted to the database, there will be one entry in the ValueStore. When the field is marked to generate value at insert, the DataProducer will generate a key(a) and a value(null). In those cases it is up to the Consumer to produce the value of (a).



