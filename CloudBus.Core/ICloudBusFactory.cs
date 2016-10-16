namespace CloudBus.Core
{
    public interface ICloudBusFactory
    {
        IBus CreateBus();

        IWorker CreateWorker();
    }
}
