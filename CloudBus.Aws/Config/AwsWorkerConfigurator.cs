using CloudBus.Aws.Environment;

namespace CloudBus.Aws.Config
{
    public class AwsWorkerConfigurator
    {
        public ISubscriptionQueueNamingConvention SubscriptionQueueNamingConvention { get; private set; }
        public AwsWorkerConfigurator()
        {
            SubscriptionQueueNamingConvention = new HostNameSubscriptionQueueNamingConvention();
        }

        public AwsWorkerConfigurator WithSubscriptionQueueNamingConvention(ISubscriptionQueueNamingConvention convention)
        {
            SubscriptionQueueNamingConvention = convention;
            return this;
        }

        public IAwsWorkerConfiguration Build()
        {
            return new AwsWorkerConfiguration(SubscriptionQueueNamingConvention);
        }
    }
}
