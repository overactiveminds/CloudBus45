using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Components.DictionaryAdapter;
using CloudBus.Core.Serialization;

namespace CloudBus.Core
{
    public class Configuration : IConfiguration
    {
        public Type CommandType { get; private set; }

        public Type EventType { get; private set; }

        public IHandlerResolver HandlerResolver { get; private set; }

        public IMessageSerializer MessageSerializer { get; private set; }

        public List<Assembly> AssembliesToScan { get; private set; }

        public ICloudBusConfiguration CloudBusConfiguration { get; private set; }

        public List<INotifiedOnMessage> MessageNotifiers { get; private set; }

        public Configuration()
        {
            MessageSerializer = new JsonDataContractSerializer();
            AssembliesToScan = AppDomain.CurrentDomain.GetAssemblies().ToList();
            MessageNotifiers = new EditableList<INotifiedOnMessage>();
        }

        public Configuration WithMessageSerializer(IMessageSerializer messageSerializer)
        {
            if (messageSerializer == null)
            {
                throw new ArgumentNullException(nameof(messageSerializer));
            }
            MessageSerializer = messageSerializer;
            return this;
        }

        public Configuration WithHandlerResolver(IHandlerResolver container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            HandlerResolver = container;
            return this;
        }

        public Configuration DifferentiateCommandsAs<TCommand>()
        {
            Type commandType = typeof (TCommand);
            if (!commandType.IsInterface && !commandType.IsAbstract)
            {
                throw new ArgumentException("TCommand must be either an abstract class or interface", nameof(TCommand));
            }
            CommandType = typeof (TCommand);
            return this;
        }

        public Configuration DifferentiateEventsAs<TEvent>()
        {
            Type eventType = typeof (TEvent);
            if (!eventType.IsInterface && !eventType.IsAbstract)
            {
                throw new ArgumentException("TEvent must be either an abstract class or interface", nameof(eventType));
            }
            EventType = typeof (TEvent);
            return this;
        }

        public Configuration WithBusConfiguration(ICloudBusConfiguration cloudBusConfiguration)
        {
            CloudBusConfiguration = cloudBusConfiguration;
            return this;
        }

        public Configuration AddAssemblyToScan(params Assembly[] assemblies)
        {
            foreach (var assemblyToAdd in assemblies.Where(assemblyToAdd => AssembliesToScan.All(x => x.FullName != assemblyToAdd.FullName)))
            {
                AssembliesToScan.Add(assemblyToAdd);
            }
            return this;
        }

        public Configuration WithAssebliesToScan(IEnumerable<Assembly> assemblies)
        {
            AssembliesToScan = assemblies.ToList();
            return this;
        }

        public Configuration WithMessageNotifier(INotifiedOnMessage notifier)
        {
            MessageNotifiers.Add(notifier);
            return this;
        }

        public ICloudBusFactory Build()
        {
            ValidateConfiguration();
            return CloudBusConfiguration.Build(this);
        }

        private void ValidateConfiguration()
        {
            // TODO: Validate
        }
    }
}
