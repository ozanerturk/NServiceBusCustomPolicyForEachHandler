using NServiceBus;
using System;
using System.Threading.Tasks;
using WebApplication1.CustomPolicy;

namespace WebApplication1.Handlers
{
    public class WithoutCustomPolicyHandler : IHandleMessages<WithCustomPolicyMessage>
    {
        public WithoutCustomPolicyHandler()
        {
        }
        public Task Handle(WithCustomPolicyMessage message, IMessageHandlerContext context)
        {
            throw new Exception();
        }
    }
}
