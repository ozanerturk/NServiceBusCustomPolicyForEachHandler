using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using System.Threading.Tasks;
using WebApplication1.Handlers;

namespace WebApplication1.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private IEndpointInstance rabbitMqEndpointInstance;

        public MessageController(IEndpointInstance instance)
        {
            this.rabbitMqEndpointInstance = instance;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string message, bool withCustomPolicy)
        {
            await rabbitMqEndpointInstance.SendLocal(new MyMessage() { Message = message });
            return Ok();
        }
    }
}