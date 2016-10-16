using System;

namespace CloudBus.Aws.Exceptions
{
    public class MultipleHandlersForCommandException : Exception
    {
        public MultipleHandlersForCommandException(Type type)
            : base($"Multiple handlers were found for type {type}.  When using send, only a single handler should be registered for that command type.")
        {
            
        }
    }
}