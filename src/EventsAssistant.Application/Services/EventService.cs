using EventsAssistant.Application.Abstractions;
using EventsAssistant.Domain.Entities;

namespace EventsAssistant.Application.Services;

public sealed class EventService(IEventRepository repository)
{
    public Task<IReadOnlyList<Event>> GetUpcomingAsync(CancellationToken cancellationToken) =>
        repository.GetUpcomingAsync(cancellationToken);

    public Task<Event?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        repository.GetByIdAsync(id, cancellationToken);

    public async Task<(bool Success, string Message, EventBooking? Booking)> BookAsync(
        int eventId,
        string attendeeName,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(attendeeName))
            return (false, "Attendee name is required.", null);

        var eventItem = await repository.GetByIdAsync(eventId, cancellationToken);
        if (eventItem is null)
            return (false, "Event not found.", null);

        if (!eventItem.TryReserveSeat())
            return (false, "No seats are available, or the event has already started.", null);

        var booking = new EventBooking(eventId, attendeeName);
        await repository.AddBookingAsync(booking, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return (true, "Booking confirmed.", booking);
    }
}
