using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SemanticKernel.OpenRouter.FunctionCalling.Plugins;

var apiKey = Environment.GetEnvironmentVariable("OPENROUTER_API_KEY");
if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.WriteLine("Missing OPENROUTER_API_KEY environment variable.");
    Console.WriteLine("Set it to your OpenRouter API key, then run the app again.");
    return;
}

var model = Environment.GetEnvironmentVariable("OPENROUTER_MODEL")
            ?? "google/gemini-2.5-flash";

var builder = Kernel.CreateBuilder();

builder.AddOpenAIChatCompletion(
    modelId: model,
    apiKey: apiKey,
    endpoint: new Uri("https://openrouter.ai/api/v1"));

var kernel = builder.Build();
kernel.Plugins.AddFromObject(new EventsPlugin(new InMemoryEventRepository()), "Events");

var chatService = kernel.GetRequiredService<IChatCompletionService>();
var history = new ChatHistory(
    "You are a helpful events assistant. Answer in the user's language. " +
    "Use the Events tools whenever the user asks about available events, event details, or booking. " +
    "Never claim a booking succeeded unless the booking tool confirms it.");

var executionSettings = new OpenAIPromptExecutionSettings
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};

Console.WriteLine($"Events assistant is ready (model: {model}).");
Console.WriteLine("Try: What events are coming up? / احجزلي مكان في أول Event");
Console.WriteLine("Type 'exit' to quit.\n");

while (true)
{
    Console.Write("You: ");
    var input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
        continue;

    if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    history.AddUserMessage(input);

    try
    {
        var response = await chatService.GetChatMessageContentAsync(
            history,
            executionSettings,
            kernel);

        history.AddAssistantMessage(response.Content ?? string.Empty);
        Console.WriteLine($"Assistant: {response.Content}\n");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Request failed: {ex.Message}\n");
    }
}
