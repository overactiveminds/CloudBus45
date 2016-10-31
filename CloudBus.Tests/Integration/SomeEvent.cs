using System;
using System.Runtime.Serialization;

namespace CloudBus.Tests.Integration
{
    [DataContract]
    public class SomeEvent : Event
    {
        [DataMember]
        public Guid Id { get; set; }
    }
}