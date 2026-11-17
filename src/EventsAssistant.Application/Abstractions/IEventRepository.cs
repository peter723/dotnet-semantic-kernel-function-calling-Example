using EventsAssistant.Domain.Entities;

namespace EventsAssistant.Application.Abstractions;

public interface IEventRepository
{
    Task<IReadOnlyList<Event>> GetUpcomingAsync(CancellationToken cancellationToken);
    Task<Event?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddBookingAsync(EventBooking booking, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
