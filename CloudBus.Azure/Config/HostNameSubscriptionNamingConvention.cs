using System.Net;

namespace CloudBus.Azure.Config
{
    public class HostNameSubscriptionNamingConvention : ISubscriptionNamingConvention
    {
        public string GetSubscriptionName()
        {
            return Dns.GetHostName().ToLower();
        }
    }
}
