using EventsAssistant.Application.Contracts.Events;
using EventsAssistant.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventsAssistant.Api.Controllers;

[ApiController]
[Route("api/events")]
public sealed class EventsController(EventService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EventResponse>>> Get(CancellationToken ct)
    {
        var events = await service.GetUpcomingAsync(ct);
        return Ok(events.Select(x => new EventResponse(x.Id, x.Name, x.Description,
            x.StartsAt, x.Capacity, x.AvailableSeats)).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EventResponse>> GetById(int id, CancellationToken ct)
    {
        var x = await service.GetByIdAsync(id, ct);
        return x is null ? NotFound() : Ok(new EventResponse(x.Id, x.Name, x.Description,
            x.StartsAt, x.Capacity, x.AvailableSeats));
    }

    [HttpPost("{id:int}/bookings")]
    public async Task<IActionResult> Book(int id, BookingRequest request, CancellationToken ct)
    {
        var result = await service.BookAsync(id, request.AttendeeName, ct);
        if (!result.Success)
            return result.Message == "Event not found." ? NotFound(new { result.Message }) :
                BadRequest(new { result.Message });

        var booking = result.Booking!;
        var response = new BookingResponse(booking.Id, booking.EventId,
            booking.AttendeeName, booking.BookedAtUtc);
        return Created($"/api/events/{id}/bookings/{booking.Id}", response);
    }
}