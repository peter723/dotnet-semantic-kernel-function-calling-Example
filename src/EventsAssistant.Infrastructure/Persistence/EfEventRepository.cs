using EventsAssistant.Application.Abstractions;
using EventsAssistant.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventsAssistant.Infrastructure.Persistence;

public sealed class EfEventRepository(AppDbContext dbContext) : IEventRepository
{
    public async Task<IReadOnlyList<Event>> GetUpcomingAsync(CancellationToken cancellationToken) =>
        await dbContext.Events
            .AsNoTracking()
            .Where(x => x.StartsAt > DateTime.Now)
            .OrderBy(x => x.StartsAt)
            .ToListAsync(cancellationToken);

    public Task<Event?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        dbContext.Events.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task AddBookingAsync(EventBooking booking, CancellationToken cancellationToken) =>
        dbContext.EventBookings.AddAsync(booking, cancellationToken).AsTask();

    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        dbContext.SaveChangesAsync(cancellationToken);
}
