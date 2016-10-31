namespace CloudBus.Azure.Config
{
    public class AzureWorkerConfiguration : IAzureWorkerConfiguration
    {
        public ISubscriptionNamingConvention SubscriptionNamingConvention { get; private set; }

        public AzureWorkerConfiguration()
        {
            SubscriptionNamingConvention = new HostNameSubscriptionNamingConvention();
        }

        public AzureWorkerConfiguration WithSubscriptionNamingConvention(ISubscriptionNamingConvention subscriptionNamingConvention)
        {
            SubscriptionNamingConvention = subscriptionNamingConvention;
            return this;
        }
    }
}
