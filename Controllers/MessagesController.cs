using BasicChatApi.Schemas;
using Microsoft.AspNetCore.Mvc;

namespace BasicChatApi.Controllers; 

[ApiController]
[Route("messages")]
public class MessagesController : ApiController {
    
    [HttpGet("{channel}")]
    public IEnumerable<Message> GetMessages(string channel, [FromQuery] int limit = 10, [FromQuery] int offset = 0, [FromQuery] string? name = null) {
        if (name != null) {
            StatusTracker.UserPinged(name, channel);
        }
        return Program.Storage.GetMessages(channel, limit, offset);
    }

    [HttpPost("{channel}")]
    public Message SendMessage([FromBody] IncomingMessage message, string channel) {
        Message msg = message.ToMessage();
        Program.Storage.CreateMessage(channel, msg);
        return msg;
    }

    [HttpGet("{channel}/online")]
    public IEnumerable<string> GetOnlineUsers(string channel) {
        List<string> users = StatusTracker.GetOnlineUsers(channel);
        return users;
    }
    
}