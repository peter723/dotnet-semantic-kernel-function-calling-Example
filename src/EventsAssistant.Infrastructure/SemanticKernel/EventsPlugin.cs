using System.ComponentModel;
using EventsAssistant.Application.Services;
using Microsoft.SemanticKernel;

namespace EventsAssistant.Infrastructure.SemanticKernel;

/// <summary>
/// The plugin is the bridge between the model and application use-cases.
/// Keep tool methods small; business rules stay in EventService/domain.
/// </summary>
public sealed class EventsPlugin(EventService eventService)
{
    [KernelFunction("get_upcoming_events")]
    [Description("Gets upcoming events with IDs, names, dates, descriptions, and available seats.")]
    public async Task<string> GetUpcomingEventsAsync()
    {
        var events = await eventService.GetUpcomingAsync(CancellationToken.None);

        if (events.Count == 0)
            return "No upcoming events were found.";

        return string.Join("\n", events.Select(item =>
            $"Id: {item.Id}; Name: {item.Name}; StartsAt: {item.StartsAt:O}; " +
            $"AvailableSeats: {item.AvailableSeats}; Description: {item.Description}"));
    }

    [KernelFunction("get_event_details")]
    [Description("Gets the details and remaining seats for one event by its numeric ID.")]
    public async Task<string> GetEventDetailsAsync(
        [Description("The numeric ID of the event.")] int eventId)
    {
        var item = await eventService.GetByIdAsync(eventId, CancellationToken.None);
        return item is null
            ? "Event not found."
            : $"Id: {item.Id}\nName: {item.Name}\nDescription: {item.Description}\n" +
              $"StartsAt: {item.StartsAt:O}\nCapacity: {item.Capacity}\nAvailableSeats: {item.AvailableSeats}";
    }

    [KernelFunction("book_event")]
    [Description("Books one seat for an attendee. Use only after the user has clearly requested a booking and you have the event ID and attendee name.")]
    public async Task<string> BookEventAsync(
        [Description("The numeric ID of the event to book.")] int eventId,
        [Description("The attendee's full name.")] string attendeeName)
    {
        var result = await eventService.BookAsync(eventId, attendeeName, CancellationToken.None);
        return result.Success
            ? $"Booking confirmed. BookingId: {result.Booking!.Id}; EventId: {eventId}; Attendee: {attendeeName}."
            : $"Booking failed: {result.Message}";
    }
}
