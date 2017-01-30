namespace CloudBus.ShadowBus.ExterenalServices
{
    public interface IExternalServicesHandler<TCommand>
    {
        void Handle(TCommand command);
    }
}
