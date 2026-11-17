using EventsAssistant.Application.Contracts.Chat;

namespace EventsAssistant.Application.Abstractions;

public interface IChatAssistant
{
    Task<ChatResponse> SendAsync(ChatRequest request, CancellationToken cancellationToken);
}
