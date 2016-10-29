using System;
using System.Runtime.Serialization;
using System.Threading;
using Amazon;
using CloudBus.Aws.Config;
using CloudBus.Aws.Environment;
using CloudBus.Core;
using CloudBus.Core.HandlerResolution;
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

            config.WithHandlerResolver(new ActionHandlerResolver()
                /*.WithCommandHandler<SomeCommand>(command =>
                {
                    Console.WriteLine($"SommeCommand Received: {command.Id}");
                })*/
                .WithEventHandler<SomeEvent>(@event =>
                {
                    Console.WriteLine("$SomeEvent Received:");  
                }));


            config.WithBusConfiguration(new AwsConfigurator()
                    .WithClientFactory(new AwsClientFactory(RegionEndpoint.EUWest1))
                    .WithWorkerConfig(new AwsWorkerConfiguration())
                );

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
            

            Thread.Sleep(10000);

        }

        [DataContract]
        public abstract class Command
        {

        }

        [DataContract]
        public abstract class Event
        {
            
        }

        [DataContract]
        public class SomeCommand : Command
        {
            [DataMember]
            public Guid Id { get; set; }
        }

        [DataContract]
        public class SomeEvent : Event
        {
            [DataMember]
            public Guid Id { get; set; }
        }
    }
}
