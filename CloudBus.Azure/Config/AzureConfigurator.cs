using System;
using System.Collections.Generic;
using System.Linq;
using CloudBus.Azure.Environment;
using CloudBus.Core;
using CloudBus.Core.Assemblies;

namespace CloudBus.Azure.Config
{
    public class AzureConfigurator : ICloudBusConfiguration
    {
        private IAzureClientFactory clientFactory;
        private IAzureWorkerConfiguration azureWorkerConfiguration;
        private IQueueAndTopicNamingConvention queueAndTopicNamingConvention;

        public AzureConfigurator()
        {
            queueAndTopicNamingConvention = new DefaultQueueAndTopicNamingConvention();
        }

        public AzureConfigurator WithNamingConvention(IQueueAndTopicNamingConvention queueAndTopicNamingConvention)
        {
            this.queueAndTopicNamingConvention = queueAndTopicNamingConvention;
            return this;
        }

        public AzureConfigurator WithClientFactory(IAzureClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
            return this;
        }

        public ICloudBusConfiguration WithWorkerConfig(IAzureWorkerConfiguration azureWorkerConfiguration)
        {
            this.azureWorkerConfiguration = azureWorkerConfiguration;
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

            AzureEnvironmentBuilder builder = new AzureEnvironmentBuilder(clientFactory);

            // Build a queue for each command type
            Dictionary<Type, string> commandTypeAndQueueNames = new Dictionary<Type, string>();
            foreach (var commandType in allCommandTypes)
            {
                string queueName = queueAndTopicNamingConvention.GetQueueNameForCommand(commandType);
                builder.CreateQueueIfDoesntExist(queueName);
                commandTypeAndQueueNames.Add(commandType, queueName);
            }

            // Build a topic for each event type
            Dictionary<Type, string> eventTypeAndTopicNames = new Dictionary<Type, string>();
            foreach (var eventType in allEventTypes)
            {
                string topicName = queueAndTopicNamingConvention.GetTopicNameForEvent(eventType);
                builder.CreateTopicIfDoesntExist(topicName);
                eventTypeAndTopicNames.Add(eventType, topicName);
            }
            
            var azureConfig = new AzureBusConfig(commandTypeAndQueueNames, eventTypeAndTopicNames, queueAndTopicNamingConvention, clientFactory);
            if (azureWorkerConfiguration == null)
            {
                return new AzureCloudBusFactory(busConfig, azureConfig);
            }
            
            string subscriptionName = azureWorkerConfiguration.SubscriptionNamingConvention.GetSubscriptionName();
            foreach (var eventTypeAndTopicName in eventTypeAndTopicNames)
            {
                builder.CreateSubscriptionIfDoesntExist(subscriptionName, eventTypeAndTopicName.Value);
            }
            return new AzureCloudBusFactory(busConfig, azureConfig, azureWorkerConfiguration, subscriptionName);
        }
    }
}
