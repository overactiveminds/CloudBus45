using System;
using CloudBus.Azure.Config;
using CloudBus.Core;

namespace CloudBus.Azure
{
    public class AzureWorker : IWorker
    {
        private readonly AzureBusConfig azureConfig;
        private readonly IAzureWorkerConfiguration azureWorkerConfiguration;
        private readonly string subscriptionName;
        private readonly IConfiguration busConfig;


        public AzureWorker(IConfiguration busConfig, AzureBusConfig azureConfig, IAzureWorkerConfiguration azureWorkerConfiguration, string subscriptionName)
        {
            this.busConfig = busConfig;
            this.azureConfig = azureConfig;
            this.azureWorkerConfiguration = azureWorkerConfiguration;
            this.subscriptionName = subscriptionName;
        }

        public void Start()
        {
            foreach (var commandType in azureConfig.CommandTypeAndQueueName)
            {
                var queueClient = azureConfig.ClientFactory.CreateQueueClient(commandType.Value);
                queueClient.OnMessage(message =>
                {
                    Console.WriteLine("Command received");
                });
            }

            foreach (var eventType in azureConfig.EventTypeAndTopicName)
            {
                var subscriptionClient = azureConfig.ClientFactory.CreateSubscriptionClient(subscriptionName, eventType.Value);
                subscriptionClient.OnMessage(message =>
                {
                    Console.WriteLine("Event received");
                });
            }
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
