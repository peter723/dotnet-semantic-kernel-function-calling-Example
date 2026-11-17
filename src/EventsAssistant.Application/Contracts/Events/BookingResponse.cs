namespace EventsAssistant.Application.Contracts.Events;

public sealed record BookingResponse(int BookingId, int EventId, string AttendeeName, DateTime BookedAtUtc);
