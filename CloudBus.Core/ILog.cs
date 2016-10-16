namespace CloudBus.Core
{
    public interface ILog
    {
        void Debug(params object[] messages);

        void Error(params object[] messages);

        void Fatal(params object[] messages);

        void Info(params object[] messages);

        void Trace(params object[] messages);

        void Warn(params object[] messages);
    }
}
