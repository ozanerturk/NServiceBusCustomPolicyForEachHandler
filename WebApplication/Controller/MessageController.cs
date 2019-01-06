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
        public async Task<IActionResult> SendMessageWithOrWithoutCustomPolicy(string message, bool withCustomPolicy)
        {
            if (withCustomPolicy)
            {
                await rabbitMqEndpointInstance.SendLocal(new WithCustomPolicyMessage() { Message = message });
            }
            else
            {
                await rabbitMqEndpointInstance.SendLocal(new WithoutCustomPolicyMessage() { Message = message });
            }
            return Ok();
        }
    }
}