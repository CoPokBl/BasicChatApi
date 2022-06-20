using System.Collections.Generic;
using BasicChatApi.Schemas;
using Microsoft.AspNetCore.Mvc;

namespace BasicChatApi.Controllers; 

[ApiController]
[Route("messages")]
public class MessagesController : ApiController {
    
    [HttpGet("{channel}")]
    public IEnumerable<Message> Get(string channel, [FromQuery] int limit = 10, [FromQuery] int offset = 0, [FromQuery] string? name = null) {
        if (name != null) {
            StatusTracker.UserPinged(name, channel);
        }
        return Program.Storage.GetMessages(channel, limit, offset);
    }

    [HttpPost("{channel}")]
    public void Post([FromBody] IncomingMessage message, string channel) 
        => Program.Storage.CreateMessage(channel, message.ToMessage());

    [HttpGet("{channel}/online")]
    public IEnumerable<string> GetOnline(string channel) {
        var users = StatusTracker.GetOnlineUsers(channel);
        return users;
    }
    
}