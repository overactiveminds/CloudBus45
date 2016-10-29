using Amazon.SQS.Model;

namespace CloudBus.Aws
{
    public interface IMessageAdapter
    {
        string GetMessageBody(Message message);
    }
}
