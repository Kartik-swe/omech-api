using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using omech.Models;
using omech.Services;

namespace omech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiChatController : ControllerBase
    {
        private readonly IAiChatService _aiChatService;

        public AiChatController(IAiChatService aiChatService)
        {
            _aiChatService = aiChatService;
        }

        //[Authorize]
        [HttpPost("Ask")]
        public async Task<IActionResult> Ask([FromBody] AiChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
                return BadRequest(new { msgId = 400, msg = "Message is required." });

            if (string.IsNullOrWhiteSpace(request.SessionId))
                return BadRequest(new { msgId = 400, msg = "SessionId is required." });

            try
            {
                var result = await _aiChatService.AskAsync(request);
                return Ok(new { msgId = 200, msg = "Success", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msgId = 500, msg = $"AI chat error: {ex.Message}" });
            }
        }
    }
}
