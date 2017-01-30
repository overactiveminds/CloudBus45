using CloudBus.Aws.Config;
using CloudBus.Core;

namespace CloudBus.Aws
{
    public class AwsCloudBusFactory : ICloudBusFactory
    {
        private readonly IAwsWorkerConfiguration workerConfiguration;
        private readonly string eventSubscriptionQueueUrl;
        private readonly IConfiguration configuration;
        private readonly IAwsBusConfiguration awsBusConfig;

        public AwsCloudBusFactory(IConfiguration configuration, IAwsBusConfiguration awsBusConfig, IAwsWorkerConfiguration workerConfiguration, string eventSubscriptionQueueUrl)
            : this(configuration, awsBusConfig)
        {
            this.workerConfiguration = workerConfiguration;
            this.eventSubscriptionQueueUrl = eventSubscriptionQueueUrl;
        }

        public AwsCloudBusFactory(IConfiguration configuration, IAwsBusConfiguration awsBusConfig)
        {
            this.configuration = configuration;
            this.awsBusConfig = awsBusConfig;
        }

        public IBus CreateBus()
        {
            return new AwsBus(configuration, awsBusConfig);
        }

        public IWorker CreateWorker()
        {
            return new Worker(configuration, awsBusConfig, workerConfiguration, eventSubscriptionQueueUrl);
        }
    }
}
