using NServiceBus;
using System;
using System.Threading.Tasks;
using WebApplication1.CustomPolicy;

namespace WebApplication1.Handlers
{
    public class WithCustomPolicyHandler : IHandleMessages<WithCustomPolicyMessage>
    {
        public WithCustomPolicyHandler(ICustomRecoveryPolicyFactory factory)
        {
            factory.AddPolicy<WithCustomPolicyMessage>(immediateRetryCount: 1, delayedRetryCount: 2, delayedRetryTimeIncrease: 5);
        }
        public Task Handle(WithCustomPolicyMessage message, IMessageHandlerContext context)
        {
            throw new Exception();
        }
    }
}
