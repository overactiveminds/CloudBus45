# CloudBus

CloudBus is an abstraction around the main cloud providers IaaS bus implementations and supports a CQRS based approach where messages are either commands or events.  Commands are handled once, events work on a pub sub basis.  

We're planning to support Amazon Web Services, Microsoft Azure and Google Cloud Services. 

CloudBus aims to make using a bus on any of these platform as simple as possible with one interface for sending and receiving commands and events.  

The main goal of this project is to enable us to explore XCloud deployments where an application can be deployed accross multiple cloud environments - interesting!!!!

# RoadMap

1. Move to dotnet standard to with the aim of running on docker to allow XCloud code deployments.  
2. Add support for Google Cloud Services.
3. Add a "ShadowBus" that allows publishing events XCloud.  An event published on Azure can be pushed to AWS and / or Google Cloud Services via a "ShadowBus".
4. Explore the intricacies of XCloud code as far as things like only updating external services once, sending an email once, session affinity, combining events in a stream from multiple source etc etc.  

# To run the tests

1. Create an Azure Service Bus endpoint and copy your connection string into app.config.
2. Create a default profile for AWS or alter the test code to create your AwsClinetFactory with credietnials - see constructor in AwsClientFactory
3. Run BusSendAndReceiveTest to see the bus and workers in action.

# Known Issues

1. This code has not been profiled and is not production ready!!!!
2. Test coverage is not great whilst we're prototyping.
