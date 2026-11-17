using EventsAssistant.Application.Abstractions;
using EventsAssistant.Application.Contracts.Chat;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace EventsAssistant.Infrastructure.SemanticKernel;

public sealed class SemanticKernelChatAssistant : IChatAssistant
{
    private readonly IChatCompletionService _chatCompletion;
    private readonly Kernel _kernel;

    public SemanticKernelChatAssistant(
        EventsPlugin eventsPlugin,
        IConfiguration configuration)
    {
        var apiKey = configuration["OpenRouter:ApiKey"]
            ?? throw new InvalidOperationException(
                "OpenRouter API key is missing. Set OpenRouter__ApiKey.");

        var model = configuration["OpenRouter:Model"] ?? "google/gemini-2.5-flash";
        var endpoint = configuration["OpenRouter:BaseUrl"] ?? "https://openrouter.ai/api/v1";

        var kernelBuilder = Kernel.CreateBuilder();
        kernelBuilder.AddOpenAIChatCompletion(
            modelId: model,
            apiKey: apiKey,
            endpoint: new Uri(endpoint));

        _kernel = kernelBuilder.Build();
        _kernel.Plugins.AddFromObject(eventsPlugin, "Events");
        _chatCompletion = _kernel.GetRequiredService<IChatCompletionService>();
    }

    public async Task<ChatResponse> SendAsync(ChatRequest request, CancellationToken cancellationToken)
    {
        var history = new ChatHistory(
            "You are an events assistant. Reply in the same language as the user. " +
            "Use the Events tools to answer questions about events and availability. " +
            "Only call book_event when the user clearly asks to book. " +
            "Never claim an operation succeeded unless the tool result confirms it. " +
            "Do not invent event IDs, dates, prices, availability, or booking confirmations.");

        foreach (var message in request.Messages)
        {
            if (message.Role.Equals("user", StringComparison.OrdinalIgnoreCase))
                history.AddUserMessage(message.Content);
            else if (message.Role.Equals("assistant", StringComparison.OrdinalIgnoreCase))
                history.AddAssistantMessage(message.Content);
        }

        var settings = new OpenAIPromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        var result = await _chatCompletion.GetChatMessageContentAsync(
            history,
            executionSettings: settings,
            kernel: _kernel,
            cancellationToken: cancellationToken);

        return new ChatResponse(result.Content ?? string.Empty);
    }
}
