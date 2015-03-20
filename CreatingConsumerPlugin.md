# Introduction #

Describes how to implement a data consumer plugin


# Details #

  * Implement IDataConsumer fron the SQLDataProducer.DataConsumers assembly
  * You will need to add reference to the Entities assembly
  * Add the ConsumerMetaDataAttribute attribute on your dataconsumer class
  * Place your compiled plugin in the same folder as the rest of the application
  * The second parameter of the ConsumerMetaDataAttribute is a params array of names of the options that the class will use. These will be visible in the GUI