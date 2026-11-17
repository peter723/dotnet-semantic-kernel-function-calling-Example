namespace SemanticKernel.OpenRouter.FunctionCalling.Models;

public sealed record Event(
    int Id,
    string Name,
    string Description,
    DateTime StartsAt,
    int AvailableSeats);
