using System;

namespace CloudBus.Aws.Exceptions
{
    public class CommandHandlersNotRegisteredException : Exception
    {
        public CommandHandlersNotRegisteredException(Type type) 
            : base($"No handler is registered for command type {type}")
        {
            
        }
    }
}