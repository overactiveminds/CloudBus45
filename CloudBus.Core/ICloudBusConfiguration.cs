namespace CloudBus.Core
{
    public interface ICloudBusConfiguration
    {
        ICloudBusFactory Build(Configuration busConfig);
    }
}
