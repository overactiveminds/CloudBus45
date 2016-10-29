using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Amazon.SQS.Model;

namespace CloudBus.Aws
{
    public class SnsEventMessageAdapter : IMessageAdapter
    {
        private readonly DataContractJsonSerializer serializer;

        public SnsEventMessageAdapter()
        {
            serializer = new DataContractJsonSerializer(typeof(SnsMessage));
        }
        public string GetMessageBody(Message message)
        {
            MemoryStream ms = new MemoryStream();
            using (var writer = new StreamWriter(ms))
            {
                writer.Write(message.Body);
                writer.Flush();
                ms.Position = 0;
                SnsMessage snsMessange = (SnsMessage) serializer.ReadObject(ms);
                return snsMessange.Message;
            }
        }
    }

    [DataContract]
    internal class SnsMessage
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string MessageId { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string Timestamp { get; set; }
    }
}
