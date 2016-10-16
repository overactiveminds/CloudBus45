using CloudBus.Aws.Config;
using CloudBus.Core;

namespace CloudBus.Aws
{
    public class AwsCloudBusFactory : ICloudBusFactory
    {
        private readonly Configuration configuration;
        private readonly IAwsBusConfiguration awsBusConfigurator;

        public AwsCloudBusFactory(Configuration configuration, IAwsBusConfiguration awsBusConfigurator)
        {
            this.configuration = configuration;
            this.awsBusConfigurator = awsBusConfigurator;
        }

        public IBus CreateBus()
        {
            return new AwsBus(configuration, awsBusConfigurator);
        }

        public IWorker CreateWorker()
        {
            throw new System.NotImplementedException();
        }
    }
}
