using EventsAssistant.Application.Abstractions;
using EventsAssistant.Application.Contracts.Chat;
using Microsoft.AspNetCore.Mvc;

namespace EventsAssistant.Api.Controllers;

[ApiController]
[Route("api/chat")]
public sealed class ChatController(IChatAssistant assistant) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ChatResponse>> Send(ChatRequest request, CancellationToken ct)
    {
        if (request.Messages is null || request.Messages.Count == 0)
            return BadRequest(new { message = "At least one message is required." });

        if (request.Messages.Any(m => string.IsNullOrWhiteSpace(m.Content) ||
            !(m.Role.Equals("user", StringComparison.OrdinalIgnoreCase) ||
              m.Role.Equals("assistant", StringComparison.OrdinalIgnoreCase))))
            return BadRequest(new { message = "Messages require content and role user or assistant." });

        return Ok(await assistant.SendAsync(request, ct));
    }
}