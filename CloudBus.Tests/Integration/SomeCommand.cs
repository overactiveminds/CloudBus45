using System;
using System.Runtime.Serialization;

namespace CloudBus.Tests.Integration
{
    [DataContract]
    public class SomeCommand : Command
    {
        [DataMember]
        public Guid Id { get; set; }
    }
}