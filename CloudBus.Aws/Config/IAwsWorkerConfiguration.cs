namespace CloudBus.Aws.Config
{
    public interface IAwsWorkerConfiguration
    {
        ISubscriptionQueueNamingConvention SubscriptionQueueNamingConvention { get; }
    }
}