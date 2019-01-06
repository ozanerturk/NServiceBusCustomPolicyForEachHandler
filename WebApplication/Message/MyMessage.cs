using NServiceBus;

namespace WebApplication1.Handlers
{
    public class MyMessage : ICommand
    {
        public string Message { get; set; }
    }
}
