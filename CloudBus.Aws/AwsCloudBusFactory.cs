using CloudBus.Aws.Config;
using CloudBus.Core;

namespace CloudBus.Aws
{
    public class AwsCloudBusFactory : ICloudBusFactory
    {
        private readonly IAwsWorkerConfiguration workerConfiguration;
        private readonly string localEventQueueName;
        private readonly IConfiguration configuration;
        private readonly IAwsBusConfiguration awsBusConfig;

        public AwsCloudBusFactory(IConfiguration configuration, IAwsBusConfiguration awsBusConfig, IAwsWorkerConfiguration workerConfiguration, string localEventQueueName)
        {
            this.workerConfiguration = workerConfiguration;
            this.localEventQueueName = localEventQueueName;
            this.configuration = configuration;
            this.awsBusConfig = awsBusConfig;
        }

        public IBus CreateBus()
        {
            return new AwsBus(configuration, awsBusConfig);
        }

        public IWorker CreateWorker()
        {
            return new Worker(configuration, awsBusConfig, workerConfiguration, localEventQueueName);
        }
    }
}
