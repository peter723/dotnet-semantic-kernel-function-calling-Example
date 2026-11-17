namespace EventsAssistant.Application.Contracts.Chat;

public sealed record ChatRequest(IReadOnlyList<ChatMessage> Messages);
