using Microsoft.AspNetCore.Mvc;

namespace BasicChatApi.Controllers; 

[ApiController]
[Route("/")]
public class RootController : ApiController {
    
    [HttpGet]
    public IActionResult Get() {
        return Ok(new {
            version = Program.Ver.ToString(),
            name = "Basic Chat API"
        });
    }
    
}