using NServiceBus;
using System;
using System.Threading.Tasks;
using WebApplication1.CustomPolicy;

namespace WebApplication1.Handlers
{
    public class MyMessageHandler : IHandleMessages<MyMessage>
    {
        public MyMessageHandler(ICustomRecoveryPolicyFactory factory)
        {
            factory.AddPolicy<MyMessage>(immediateRetryCount: 1, delayedRetryCount: 2, delayedRetryTimeIncrease: 5);
        }
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            throw new Exception();
        }
    }
}
