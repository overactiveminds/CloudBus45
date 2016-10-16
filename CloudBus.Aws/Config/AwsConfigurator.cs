using System;
using System.Collections.Generic;
using System.Linq;
using CloudBus.Aws.Environment;
using CloudBus.Core;

namespace CloudBus.Aws.Config
{
    public class AwsConfigurator : ICloudBusConfiguration
    {
        public INamingConvention NamingConvention { get; private set; }

        public IAwsClientFactory ClientFactory { get; private set; }

        public AwsConfigurator()
        {
            NamingConvention = new NamingConvention();
        }

        public AwsConfigurator WithNamingConvention(INamingConvention namingConvention)
        {
            NamingConvention = namingConvention;
            return this;
        }

        public AwsConfigurator WithClientFactory(IAwsClientFactory clientFactory)
        {
            ClientFactory = clientFactory;
            return this;
        }

        public ICloudBusFactory Build(Configuration busConfig)
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
            Dictionary<Type, string> commandTypeAndQueueUri = allCommandTypes.ToDictionary(commandType => commandType, commandType => builder.CreateQueueIfDoesntExist(NamingConvention.GetQueueNameForCommand(commandType)));

            // Build a topic for each event type
            Dictionary<Type, string> eventTypeAndTopicArn = allEventTypes.ToDictionary(eventType => eventType, eventType => builder.CreateTopicIfDoesntExist(NamingConvention.GetTopicNameForEvent(eventType)));

            var awsConfig = new AwsBusConfig(commandTypeAndQueueUri, eventTypeAndTopicArn, NamingConvention, ClientFactory);

            return new AwsCloudBusFactory(busConfig, awsConfig);
        }
    }
}
