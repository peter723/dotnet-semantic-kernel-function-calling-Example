using SemanticKernel.OpenRouter.FunctionCalling.Models;

namespace SemanticKernel.OpenRouter.FunctionCalling.Plugins;

public sealed class InMemoryEventRepository : IEventRepository
{
    private readonly List<Event> _events =
    [
        new(1, "Backend Meetup", "A practical meetup about modern backend development.",
            DateTime.Today.AddDays(3).AddHours(18), 12),
        new(2, "AI Engineering Workshop", "A hands-on workshop about RAG and AI tool calling.",
            DateTime.Today.AddDays(7).AddHours(16), 8),
        new(3, "Cloud & .NET Conference", "Talks about building and deploying .NET applications.",
            DateTime.Today.AddDays(14).AddHours(10), 25)
    ];

    private readonly object _lock = new();

    public IReadOnlyCollection<Event> GetUpcomingEvents()
    {
        lock (_lock)
            return _events.Where(e => e.StartsAt >= DateTime.Now).ToArray();
    }

    public Event? GetById(int eventId)
    {
        lock (_lock)
            return _events.FirstOrDefault(e => e.Id == eventId);
    }

    public bool TryBookSeat(int eventId, string attendeeName, out string confirmation)
    {
        lock (_lock)
        {
            var index = _events.FindIndex(e => e.Id == eventId);
            if (index < 0)
            {
                confirmation = "Event not found.";
                return false;
            }

            var current = _events[index];
            if (current.AvailableSeats <= 0)
            {
                confirmation = "There are no seats available for this event.";
                return false;
            }

            _events[index] = current with { AvailableSeats = current.AvailableSeats - 1 };
            confirmation = $"Booking confirmed for {attendeeName}. Event: {current.Name}.";
            return true;
        }
    }
}
