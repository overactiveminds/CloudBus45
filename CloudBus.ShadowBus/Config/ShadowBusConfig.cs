using System;
using CloudBus.Core;

namespace CloudBus.ShadowBus.Config
{
    public static class ShadowBusConfig
    {
        public static void WithShadowBus(this Configuration config, IBus shadowBus)
        {
            config.AfterCommandActions.Add(command =>
            {
                try
                {
                    shadowBus.Send(command);
                }
                catch (Exception ex)
                {
                    // TODO: Add to dead letter queue for this shadow bus inside the calling bus's cloud
                }
            });
        }
    }
}
