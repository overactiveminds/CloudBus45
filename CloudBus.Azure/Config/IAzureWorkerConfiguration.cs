namespace CloudBus.Azure.Config
{
    public interface IAzureWorkerConfiguration
    {
        ISubscriptionNamingConvention SubscriptionNamingConvention { get; }
    }
}