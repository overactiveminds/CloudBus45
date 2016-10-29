using CloudBus.Aws.Environment;

namespace CloudBus.Aws.Config
{
    public class AwsWorkerConfiguration : IAwsWorkerConfiguration
    {
        public ISubscriptionQueueNamingConvention SubscriptionQueueNamingConvention { get; private set; }

        public AwsWorkerConfiguration()
        {
            SubscriptionQueueNamingConvention = new HostNameSubscriptionQueueNamingConvention();
        }

        public AwsWorkerConfiguration(ISubscriptionQueueNamingConvention subscriptionQueueNamingConvention)
        {
            SubscriptionQueueNamingConvention = subscriptionQueueNamingConvention;
        }
    }
}