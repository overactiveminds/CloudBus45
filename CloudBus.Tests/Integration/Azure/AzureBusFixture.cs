using System;
using System.Configuration;
using System.Threading;
using CloudBus.Azure.Config;
using CloudBus.Azure.Environment;
using CloudBus.Core;
using CloudBus.Core.HandlerResolution;
using NUnit.Framework;
using Configuration = CloudBus.Core.Configuration;

namespace CloudBus.Tests.Integration.Azure
{
    public class AzureBusFixture
    {
        [Test]
        public void CreateFromConfig()
        {
            // Arrange
            Configuration config = new Configuration();
            config.DifferentiateCommandsAs<Command>();
            config.DifferentiateEventsAs<Event>();

            config.WithHandlerResolver(new ActionHandlerResolver()
                .WithCommandHandler<SomeCommand>(command =>
                {
                    Console.WriteLine($"SommeCommand Received: {command.Id}");
                })
                .WithEventHandler<SomeEvent>(@event =>
                {
                    Console.WriteLine("$SomeEvent Received:");
                }));

            string connectionString = ConfigurationManager.ConnectionStrings["AzureServiceBusConnectionString"].ConnectionString;

            config.WithBusConfiguration(new AzureConfigurator()
                .WithClientFactory(new AzureClientFactory(connectionString))
                .WithWorkerConfig(new AzureWorkerConfiguration()));

            // Act
            var busFactory = config.Build();
            IBus bus = busFactory.CreateBus();

            // Assert
            bus.Send(new SomeCommand
            {
                Id = Guid.NewGuid()
            });

            bus.Publish(new SomeEvent
            {
                Id = Guid.NewGuid()
            });

            var worker = busFactory.CreateWorker();
            worker.Start();
            Thread.Sleep(1000);
            bus.Send(new SomeCommand
            {
                Id = Guid.NewGuid()
            });

            Thread.Sleep(10000);

        }
    }
}
