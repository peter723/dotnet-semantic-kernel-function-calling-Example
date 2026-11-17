namespace EventsAssistant.Domain.Entities;

public sealed class Event
{
    private Event() { } // Required by EF Core.

    public Event(string name, string description, DateTime startsAt, int capacity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Event name is required.", nameof(name));
        if (capacity < 1)
            throw new ArgumentOutOfRangeException(nameof(capacity));

        Name = name.Trim();
        Description = description.Trim();
        StartsAt = startsAt;
        Capacity = capacity;
        AvailableSeats = capacity;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime StartsAt { get; private set; }
    public int Capacity { get; private set; }
    public int AvailableSeats { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public bool TryReserveSeat()
    {
        if (AvailableSeats <= 0 || StartsAt <= DateTime.Now)
            return false;

        AvailableSeats--;
        return true;
    }
}
