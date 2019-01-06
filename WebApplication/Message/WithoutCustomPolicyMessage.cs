using NServiceBus;

namespace WebApplication1.Handlers
{
    public class WithoutCustomPolicyMessage : ICommand
    {
        public string Message { get; set; }
    }
}
