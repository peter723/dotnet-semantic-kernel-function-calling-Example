namespace EventsAssistant.Application.Contracts.Events;

public sealed record EventResponse(
    int Id,
    string Name,
    string Description,
    DateTime StartsAt,
    int Capacity,
    int AvailableSeats);
