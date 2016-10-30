using CloudBus.Azure.Config;
using CloudBus.Core;
using Microsoft.ServiceBus.Messaging;

namespace CloudBus.Azure
{
    public class AzureBus : IBus
    {
        private readonly IConfiguration config;
        private readonly IAzureBusConfig azureConfig;

        public AzureBus(IConfiguration config, IAzureBusConfig azureConfig)
        {
            this.config = config;
            this.azureConfig = azureConfig;
        }

        public void Send<TCommand>(TCommand command)
        {
            string messageBody = config.MessageSerializer.Serialize(command);
            MessageEnvelope envelope = new MessageEnvelope
            {
                BodyType = typeof(TCommand).AssemblyQualifiedName,
                Body = messageBody
            };
            var queue = azureConfig.ClientFactory.CreateQueueClient(azureConfig.QueueAndTopicNamingConvention.GetQueueNameForCommand(typeof(TCommand)));
            BrokeredMessage message = new BrokeredMessage(envelope);
            queue.Send(message);
        }

        public void Publish<TEvent>(TEvent @event)
        {
            string messageBody = config.MessageSerializer.Serialize(@event);
            MessageEnvelope envelope = new MessageEnvelope
            {
                BodyType = typeof(TEvent).AssemblyQualifiedName,
                Body = messageBody
            };
            var topic = azureConfig.ClientFactory.CreateTopicClient(azureConfig.QueueAndTopicNamingConvention.GetTopicNameForEvent(typeof(TEvent)));
            BrokeredMessage message = new BrokeredMessage(envelope);
            topic.Send(message);
        }
    }
}
