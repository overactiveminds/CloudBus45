namespace CloudBus.Aws.Config
{
    public interface ISubscriptionQueueNamingConvention
    {
        string GetWorkerQueueName();
    }
}
