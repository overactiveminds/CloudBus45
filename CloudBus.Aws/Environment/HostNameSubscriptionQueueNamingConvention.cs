using System.Linq;
using CloudBus.Aws.Config;

namespace CloudBus.Aws.Environment
{
    public class HostNameSubscriptionQueueNamingConvention : ISubscriptionQueueNamingConvention
    {
        public string GetWorkerQueueName()
        {
            string hostName = System.Net.Dns.GetHostName();
            return GetAlphaNumericOnly($"{hostName}");
        }

        private string GetAlphaNumericOnly(string text)
        {
            return text.ToCharArray().Where(c => char.IsLetterOrDigit(c) || c == '-').Aggregate("", (current, c) => current + c);
        }
    }
}
