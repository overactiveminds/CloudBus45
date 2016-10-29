using Amazon;
using CloudBus.Aws.Config;
using CloudBus.Aws.Environment;
using CloudBus.Core;
using NUnit.Framework;

namespace CloudBus.Tests.Integration.Aws
{
    
    public class AwsBus
    {
        [Test]
        public void CreateFromConfig()
        {
            // Arrange
            Configuration config = new Configuration();
            config.DifferentiateCommandsAs<Command>();
            config.DifferentiateEventsAs<Event>();
            config.WithBusConfiguration(new AwsConfigurator()
                    .WithClientFactory(new AwsClientFactory(RegionEndpoint.EUWest1))
                );

            // Act
            var busFactory = config.Build();
            IBus bus = busFactory.CreateBus();

            // Assert
            bus.Send(new SomeCommand());
        }


        public abstract class Command
        {

        }

        public abstract class Event
        {
            
        }

        public class SomeCommand : Command
        {
            
        }

        public class SomeEvent : Event
        {
            
        }
    }
}
