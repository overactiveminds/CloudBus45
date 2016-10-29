# CloudBus

Cloud bus is an abstraction around the main cloud providers IaaS bus implementations and supports a CQRS based approach where messages are either commands or events.  Commands are handled once, events are published to subscribing endpoints.  

We're planning to support Amazon Web Services, Microsoft Azure and Google Cloud Services. 

CloudBus aims to make using a bus on any of these platform as simple as possible with one interface for sending and receiving commands and events.  

The main goal of this project is to enable us to explore XCloud deployments where an application can be deployed accross multiple cloud environments.
