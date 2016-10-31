using System;
using System.Collections.Generic;
using System.Reflection;

namespace CloudBus.Core
{
    public interface IConfiguration
    {
        Type CommandType { get; }
        Type EventType { get; }
        IHandlerResolver HandlerResolver { get; }
        IMessageSerializer MessageSerializer { get; }
        List<Assembly> AssembliesToScan { get; }
        List<Action<object>> AfterCommandActions { get; }
        List<Action<object>> AfterEventActions { get; }  
    }
}