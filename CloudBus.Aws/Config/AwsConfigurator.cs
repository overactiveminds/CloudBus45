using System;
using System.Collections.Generic;
using System.Linq;
using CloudBus.Aws.Environment;
using CloudBus.Core;

namespace CloudBus.Aws.Config
{
    public class AwsConfigurator : ICloudBusConfiguration
    {
        public IQueueAndTopicNamingConvention QueueAndTopicNamingConvention { get; private set; }

        public IAwsClientFactory ClientFactory { get; private set; }

        public IAwsWorkerConfiguration WorkerConfig { get; set; }

        public Dictionary<Type, string> CommandQueueNamesAndUrls { get; private set; }
        
        public Dictionary<Type, string> EventTopicsAndArns { get; private set; }  

        public AwsConfigurator()
        {
            QueueAndTopicNamingConvention = new QueueAndTopicNamingConvention();
        }

        public AwsConfigurator WithNamingConvention(IQueueAndTopicNamingConvention queueAndTopicNamingConvention)
        {
            QueueAndTopicNamingConvention = queueAndTopicNamingConvention;
            return this;
        }

        public AwsConfigurator WithClientFactory(IAwsClientFactory clientFactory)
        {
            ClientFactory = clientFactory;
            return this;
        }

        public AwsConfigurator WithWorkerConfig(IAwsWorkerConfiguration workerConfiguration)
        {
            WorkerConfig = workerConfiguration;
            return this;
        }

        

        public ICloudBusFactory Build(IConfiguration busConfig)
        {
            AssemblyScanner scanner = new AssemblyScanner();

            // Find all command types
            List<Type> allCommandTypes = busConfig.CommandType.IsAbstract ?
                scanner.FindAllTypesExtending(busConfig.CommandType, busConfig.AssembliesToScan).ToList() :
                scanner.FindAllTypesImplementing(busConfig.CommandType, busConfig.AssembliesToScan).ToList();

            // Find all event types
            List<Type> allEventTypes = busConfig.EventType.IsAbstract ?
                scanner.FindAllTypesExtending(busConfig.EventType, busConfig.AssembliesToScan).ToList() :
                scanner.FindAllTypesImplementing(busConfig.EventType, busConfig.AssembliesToScan).ToList();

            AwsEnvironmentBuilder builder = new AwsEnvironmentBuilder(ClientFactory);

            // Build a queue for each command type
            Dictionary<Type, string> commandTypeAndQueueUri = allCommandTypes.ToDictionary(commandType => commandType, commandType => builder.CreateQueueIfDoesntExist(QueueAndTopicNamingConvention.GetQueueNameForCommand(commandType)));

            // Build a topic for each event type
            Dictionary<Type, string> eventTypeAndTopicArns = allEventTypes.ToDictionary(eventType => eventType, eventType => builder.CreateTopicIfDoesntExist(QueueAndTopicNamingConvention.GetTopicNameForEvent(eventType)));

            var awsConfig = new AwsBusConfig(commandTypeAndQueueUri, eventTypeAndTopicArns, QueueAndTopicNamingConvention, ClientFactory);
            if (WorkerConfig == null)
            {
                // No need to doanything else
                return new AwsCloudBusFactory(busConfig, awsConfig, WorkerConfig, null);
            }

            // As we have a worker config, create subscriptions for our events
            string queueName = WorkerConfig.SubscriptionQueueNamingConvention.GetWorkerQueueName();
            builder.CreateQueueIfDoesntExist(queueName);
            foreach (var eventTypeAndTopicArn in eventTypeAndTopicArns)
            {
                builder.SubscribeQueueToTopic(queueName, eventTypeAndTopicArn.Value);
            }
            return new AwsCloudBusFactory(busConfig, awsConfig, WorkerConfig, queueName);
        }
    }
}
