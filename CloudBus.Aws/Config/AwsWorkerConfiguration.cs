namespace CloudBus.Aws.Config
{
    public class AwsWorkerConfiguration : IAwsWorkerConfiguration
    {
        public ISubscriptionQueueNamingConvention SubscriptionQueueNamingConvention { get; private set; }

        public AwsWorkerConfiguration(ISubscriptionQueueNamingConvention subscriptionQueueNamingConvention)
        {
            SubscriptionQueueNamingConvention = subscriptionQueueNamingConvention;
        }
    }
}