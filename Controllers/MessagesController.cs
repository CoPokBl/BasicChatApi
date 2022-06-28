using BasicChatApi.Schemas;
using Microsoft.AspNetCore.Mvc;

namespace BasicChatApi.Controllers; 

[ApiController]
[Route("messages/{channel}")]
public class MessagesController : ApiController {
    
    [HttpGet]
    public ActionResult<IEnumerable<Message>> GetMessages(
        string channel, 
        [FromQuery] int limit = 10, 
        [FromQuery] int offset = 0, 
        [FromQuery] string? name = null,
        [FromHeader] AuthorizationHeaderParams? authorization = null) {
        
        if (Program.Password != null) {
            if (authorization == null) {
                return Unauthorized();
            }
            // Check password
            if (authorization.GetPassword() != Program.Password) {
                return Unauthorized();
            }
        }
        
        if (name != null) {
            StatusTracker.UserPinged(name, channel);
        }

        return Program.Storage.GetMessages(channel, limit, offset);
    }

    [HttpPost]
    public ActionResult<Message> SendMessage(
        [FromBody] IncomingMessage message, 
        string channel, 
        [FromHeader] AuthorizationHeaderParams? authorization = null) {
        
        if (Program.Password != null) {
            if (authorization == null) {
                return Unauthorized();
            }
            // Check password
            if (authorization.GetPassword() != Program.Password) {
                return Unauthorized();
            }
        }
        
        Message msg = message.ToMessage();
        Program.Storage.CreateMessage(channel, msg);
        return msg;
    }

    [HttpGet("online")]
    public ActionResult<IEnumerable<string>> GetOnlineUsers(
        string channel, 
        [FromHeader] AuthorizationHeaderParams? authorization = null) {
        
        if (Program.Password != null) {
            if (authorization == null) {
                return Unauthorized();
            }
            // Check password
            if (authorization.GetPassword() != Program.Password) {
                return Unauthorized();
            }
        }
        
        List<string> users = StatusTracker.GetOnlineUsers(channel);
        return users;
    }
    
}