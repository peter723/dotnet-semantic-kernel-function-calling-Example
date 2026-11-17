using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace SemanticKernel.OpenRouter.FunctionCalling.Plugins;

public sealed class EventsPlugin(IEventRepository repository)
{
    [KernelFunction("get_upcoming_events")]
    [Description("Returns the list of upcoming events with their IDs, dates, descriptions, and available seats.")]
    public string GetUpcomingEvents()
    {
        var events = repository.GetUpcomingEvents();

        if (events.Count == 0)
            return "There are no upcoming events.";

        return string.Join(
            Environment.NewLine,
            events.Select(e =>
                $"ID: {e.Id} | Name: {e.Name} | Starts: {e.StartsAt:yyyy-MM-dd HH:mm} | " +
                $"Available seats: {e.AvailableSeats} | Description: {e.Description}"));
    }

    [KernelFunction("get_event_details")]
    [Description("Gets the full details of one event by its numeric event ID.")]
    public string GetEventDetails(
        [Description("The numeric ID of the event.")] int eventId)
    {
        var item = repository.GetById(eventId);

        return item is null
            ? "Event not found."
            : $"ID: {item.Id}\nName: {item.Name}\nDescription: {item.Description}\n" +
              $"Starts: {item.StartsAt:yyyy-MM-dd HH:mm}\nAvailable seats: {item.AvailableSeats}";
    }

    [KernelFunction("book_event")]
    [Description("Books one available seat for a person at an event. Call only after you know the event ID and attendee name.")]
    public string BookEvent(
        [Description("The numeric ID of the event to book.")] int eventId,
        [Description("The attendee's full name.")] string attendeeName)
    {
        if (string.IsNullOrWhiteSpace(attendeeName))
            return "Booking failed: attendee name is required.";

        return repository.TryBookSeat(eventId, attendeeName.Trim(), out var confirmation)
            ? confirmation
            : $"Booking failed: {confirmation}";
    }
}
