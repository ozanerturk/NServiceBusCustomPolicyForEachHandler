using NServiceBus;

namespace WebApplication1.Handlers
{
    public class WithCustomPolicyMessage : ICommand
    {
        public string Message { get; set; }
    }
}
