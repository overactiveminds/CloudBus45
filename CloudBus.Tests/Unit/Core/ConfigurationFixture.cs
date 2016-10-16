using System;
using System.Collections.Generic;
using System.Reflection;
using CloudBus.Core;
using NUnit.Framework;

namespace CloudBus.Tests.Unit.Core
{
    public class ConfigurationFixture
    {
        [Test]
        public void WithMessageSerializer()
        {
            // Arrange
            Configuration config = new Configuration();
            IMessageSerializer serializer = new StubMessageSerializer();
            // Act
            var configReturned = config.WithMessageSerializer(serializer);

            // Assert
            Assert.AreEqual(serializer, config.MessageSerializer);
            Assert.AreEqual(config, configReturned);
        }

        [Test]
        public void WithMessageSerializerThrowsIfNull()
        {
            // Arrange
            Configuration config = new Configuration();

            // Act / Asset
            Assert.Throws<ArgumentNullException>(() => config.WithMessageSerializer(null));
        }

        [Test]
        public void WithHandlerResolver()
        {
            // Arrange
            Configuration config = new Configuration();
            IHandlerResolver container = new StubHandlerResolver();

            // Act
            var configReturned = config.WithHandlerResolver(container);

            // Assert
            Assert.AreEqual(container, config.HandlerResolver);
            Assert.AreEqual(config, configReturned);
        }

        [Test]
        public void WithHandlerResolverThrowsIfNull()
        {
            // Arrange
            Configuration config = new Configuration();

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => config.WithHandlerResolver(null));
        }

        [Test]
        public void DifferentiateCommandsAsAbstract()
        {
            // Arrange
            Configuration config = new Configuration();

            // Act
            var configReturned = config.DifferentiateCommandsAs<Command>();

            // Assert
            Assert.AreEqual(typeof(Command), config.CommandType);
            Assert.AreEqual(config, configReturned);
        }

        [Test]
        public void DifferentiateCommandsAsInterface()
        {
            // Arrange
            Configuration config = new Configuration();

            // Act
            var configReturned = config.DifferentiateCommandsAs<ICommand>();

            // Assert
            Assert.AreEqual(typeof(ICommand), config.CommandType);
            Assert.AreEqual(config, configReturned);
        }

        [Test]
        public void DifferentiateCommandsAsThrowsIfNotAbstractOrInterface()
        {
            // Arrange
            Configuration config = new Configuration();

            // Act
            Assert.Throws<ArgumentException>(() => config.DifferentiateCommandsAs<NotAbstractOrInterfaceClass>());
        }

        [Test]
        public void DifferentiateEventsAsAbstract()
        {
            // Arrange
            Configuration config = new Configuration();

            // Act
            var configReturned = config.DifferentiateEventsAs<Event>();

            // Assert
            Assert.AreEqual(typeof(Event), config.EventType);
            Assert.AreEqual(config, configReturned);
        }

        [Test]
        public void DiscriminateEventsAsInterface()
        {
            // Arrange
            Configuration config = new Configuration();

            // Act
            var configReturned = config.DifferentiateEventsAs<IEvent>();

            // Assert
            Assert.AreEqual(typeof(IEvent), config.EventType);
            Assert.AreEqual(config, configReturned);
        }

        [Test]
        public void AddAssemblyToScan()
        {
            // Act
            Configuration config = new Configuration();
            Assembly assemblyToAdd = Assembly.GetCallingAssembly();

            // Arrange
            config.AddAssemblyToScan(assemblyToAdd);

            // Assert
            Assert.IsTrue(config.AssembliesToScan.Contains(assemblyToAdd));
        }

        public class NotAbstractOrInterfaceClass
        {
            
        }


        public abstract class Command
        {

        }

        public abstract class Event
        {

        }

        public interface ICommand
        {

        }

        public interface IEvent
        {

        }

        public class StubHandlerResolver : IHandlerResolver
        {
            public void BeginLifetimeScope()
            {
                
            }

            public void EndLifetimeScope()
            {
                
            }

            public IEnumerable<Action<TMessage>> ResolveHandlersForMessage<TMessage>()
            {
                return new List<Action<TMessage>> { Handle };
            }

            public void Handle<TMessage>(TMessage message)
            {

            }
        }

        public class StubMessageSerializer : IMessageSerializer
        {
            public string Serialize<TMessage>(TMessage message)
            {
                return string.Empty;
            }

            public TMessage Deserialize<TMessage>(string data)
            {
                return default(TMessage);
            }
        }
    }
}
