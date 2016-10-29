using Amazon.SQS.Model;

namespace CloudBus.Aws
{
    public class CommandMessageAdapter : IMessageAdapter
    {
        public string GetMessageBody(Message message)
        {
            return message.Body;
        }
    }
}
