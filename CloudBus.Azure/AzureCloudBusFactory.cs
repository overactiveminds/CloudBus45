using CloudBus.Azure.Config;
using CloudBus.Core;

namespace CloudBus.Azure
{
    public class AzureCloudBusFactory : ICloudBusFactory
    {
        private readonly IAzureWorkerConfiguration azureWorkerConfiguration;
        private readonly string subscriptionName;
        private readonly AzureBusConfig azureConfig;
        private readonly IConfiguration busConfig;

        public AzureCloudBusFactory(IConfiguration busConfig, AzureBusConfig azureConfig)
        {
            this.busConfig = busConfig;
            this.azureConfig = azureConfig;
        }

        public AzureCloudBusFactory(IConfiguration busConfig, AzureBusConfig azureConfig, IAzureWorkerConfiguration azureWorkerConfiguration, string subscriptionName)
            : this(busConfig, azureConfig)
        {
            this.azureWorkerConfiguration = azureWorkerConfiguration;
            this.subscriptionName = subscriptionName;
        }

        public IBus CreateBus()
        {
            return new AzureBus(busConfig, azureConfig);
        }

        public IWorker CreateWorker()
        {
            return new AzureWorker(busConfig, azureConfig, azureWorkerConfiguration, subscriptionName);
        }
    }
}
