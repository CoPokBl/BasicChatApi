using BasicChatApi.Schemas;
using Microsoft.AspNetCore.Mvc;

namespace BasicChatApi.Controllers; 

[ApiController]
[Route("messages")]
public class MessagesController : ApiController {
    
    [HttpGet]
    public IEnumerable<Message> Get([FromQuery] int limit = 10, [FromQuery] int offset = 0) 
        => Program.Storage.GetMessages(limit, offset);
    
    [HttpPost]
    public void Post([FromBody] IncomingMessage message) 
        => Program.Storage.CreateMessage(message.ToMessage());
    
}