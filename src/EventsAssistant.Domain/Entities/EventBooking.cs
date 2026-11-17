namespace EventsAssistant.Domain.Entities;

public sealed class EventBooking
{
    private EventBooking() { } // Required by EF Core.

    public EventBooking(int eventId, string attendeeName)
    {
        if (string.IsNullOrWhiteSpace(attendeeName))
            throw new ArgumentException("Attendee name is required.", nameof(attendeeName));

        EventId = eventId;
        AttendeeName = attendeeName.Trim();
        BookedAtUtc = DateTime.UtcNow;
    }

    public int Id { get; private set; }
    public int EventId { get; private set; }
    public string AttendeeName { get; private set; } = string.Empty;
    public DateTime BookedAtUtc { get; private set; }
    public Event? Event { get; private set; }
}
