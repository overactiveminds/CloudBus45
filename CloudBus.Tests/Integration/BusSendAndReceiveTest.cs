using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using CloudBus.Aws.Config;
using CloudBus.Aws.Environment;
using CloudBus.Azure.Config;
using CloudBus.Azure.Environment;
using CloudBus.Core;
using CloudBus.Core.HandlerResolution;
using NUnit.Framework;
using Configuration = CloudBus.Core.Configuration;

namespace CloudBus.Tests.Integration
{
    public class BusSendAndReceiveTest
    {
        [Test]
        [TestCase("Aws")]
        [TestCase("Azure")]
        public async Task SendAndReceive(string cloudBusFactory)
        {
            // Arrange
            Configuration config = new Configuration();
            config.DifferentiateCommandsAs<Command>();
            config.DifferentiateEventsAs<Event>();

            const int expectedCommandsReceived = 1;
            const int expectedEventsReceived = 1;

            int commandsRecieved = 0;
            int eventsReceived = 0;

            config.WithHandlerResolver(new ActionHandlerResolver()
                .WithCommandHandler<SomeCommand>(command =>
                {
                    commandsRecieved++;
                })
                .WithEventHandler<SomeEvent>(@event =>
                {
                    eventsReceived++;
                }));

            var factory = cloudBusFactory == "Aws" ? CreateAwsBus(config) : CreateAzureBus(config);

            IBus bus = factory.CreateBus();

            // Act
            bus.Send(new SomeCommand
            {
                Id = Guid.NewGuid()
            });

            bus.Publish(new SomeEvent
            {
                Id = Guid.NewGuid()
            });

            // Assert
            var cancellationSource = new CancellationTokenSource();

            var worker = factory.CreateWorker();

            await Task.WhenAny(
                // Starts the worker and polls for messages
                worker.Start(cancellationSource.Token),

                // Early out if everything has worked
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        if (expectedCommandsReceived == commandsRecieved && expectedEventsReceived == eventsReceived)
                        {
                            break;
                        }
                    }
                }, cancellationSource.Token),

                // Only run for maximum of 5 seconds
                Task.Delay(TimeSpan.FromSeconds(5), cancellationSource.Token)
            );

            // Clean up any running tasks
            cancellationSource.Cancel();

            Assert.AreEqual(expectedCommandsReceived, commandsRecieved);
            Assert.AreEqual(expectedEventsReceived, eventsReceived);
        }

        private ICloudBusFactory CreateAzureBus(Configuration config)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["AzureServiceBusConnectionString"].ConnectionString;
            config.WithBusConfiguration(new AzureConfigurator()
                .WithClientFactory(new AzureClientFactory(connectionString))
                .WithWorkerConfig(new AzureWorkerConfiguration()));
            return config.Build();
        }

        private ICloudBusFactory CreateAwsBus(Configuration config)
        {
            config.WithBusConfiguration(new AwsConfigurator()
                    .WithClientFactory(new AwsClientFactory(RegionEndpoint.EUWest1))
                    .WithWorkerConfig(new AwsWorkerConfiguration())
                );
            return config.Build();
        }
    }
}
