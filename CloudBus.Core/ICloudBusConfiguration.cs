namespace CloudBus.Core
{
    public interface ICloudBusConfiguration
    {
        ICloudBusFactory Build(IConfiguration busConfig);
    }
}
