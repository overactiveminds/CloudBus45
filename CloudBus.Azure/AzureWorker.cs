using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CloudBus.Azure.Config;
using CloudBus.Core;
using Microsoft.ServiceBus.Messaging;

namespace CloudBus.Azure
{
    public class AzureWorker : IWorker
    {
        private readonly AzureBusConfig azureConfig;
        private readonly IAzureWorkerConfiguration azureWorkerConfiguration;
        private readonly string subscriptionName;
        private readonly IConfiguration config;


        public AzureWorker(IConfiguration busConfig, AzureBusConfig azureConfig, IAzureWorkerConfiguration azureWorkerConfiguration, string subscriptionName)
        {
            this.config = busConfig;
            this.azureConfig = azureConfig;
            this.azureWorkerConfiguration = azureWorkerConfiguration;
            this.subscriptionName = subscriptionName;
        }

        public Task Start(CancellationToken token)
        {
            List<Task> tasks = new List<Task>();
            foreach (var commandType in azureConfig.CommandTypeAndQueueName)
            {
                var queueClient = azureConfig.ClientFactory.CreateQueueClient(commandType.Value);
                tasks.Add(Task.Run(() =>
                {
                    while (true)
                    {
                        token.ThrowIfCancellationRequested();
                        var message = queueClient.Receive(TimeSpan.FromSeconds(5));
                        if (message == null) continue;
                        HandleMessage(message);
                    }
                    // ReSharper disable once FunctionNeverReturns
                }, token));
            }
            foreach (var eventType in azureConfig.EventTypeAndTopicName)
            {
                var subscriptionClient = azureConfig.ClientFactory.CreateSubscriptionClient(subscriptionName, eventType.Value);
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        token.ThrowIfCancellationRequested();
                        var message = subscriptionClient.Receive(TimeSpan.FromSeconds(5));
                        if (message == null) continue;
                        HandleMessage(message);
                    }
                    // ReSharper disable once FunctionNeverReturns
                }, token));
            }
            return Task.WhenAll(tasks.ToArray());
        }

        private void HandleMessage(BrokeredMessage message)
        {
            // TODO: Rely on azure's serialization for MessageEnvelope?
            MessageEnvelope envelope = message.GetBody<MessageEnvelope>();
            Type bodyType = Type.GetType(envelope.BodyType);
            object body = config.MessageSerializer.Deserialize(bodyType, envelope.Body);

            // Get handlers
            var handler = config.HandlerResolver.ResolveHandlerForMessage(bodyType);

            // Invoke
            try
            {
                handler(body);

                // Handle complete, remove from queue
                message.Complete();
            }
            catch (Exception ex)
            {
                // TODO: NLog
                Console.WriteLine($"Error {ex.Message} handling message {message.MessageId}");
            }
        }
    }
}
