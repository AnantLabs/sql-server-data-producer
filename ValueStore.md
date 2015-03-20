# Introduction #

The ValueStore is a dictionary of values that are supposed to be inserted to the database. Each key points to a single value.

The DataProducer will generate values to be inserted by inspecting each TableEntity that is supposed to have data generated. Deciding what tables to be generate data for is done by building a tree of ExecutionNode.