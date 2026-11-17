using EventsAssistant.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventsAssistant.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken = default)
    {
        // EnsureCreated keeps this learning sample easy to run without migration setup.
        // Use EF Core migrations instead for production applications.
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);

        if (await dbContext.Events.AnyAsync(cancellationToken))
            return;

        dbContext.Events.AddRange(
            new Event("Backend Meetup", "A practical meetup about modern backend development.",
                DateTime.Now.AddDays(3).Date.AddHours(18), 30),
            new Event("AI Engineering Workshop", "A hands-on workshop about RAG and AI tool calling.",
                DateTime.Now.AddDays(7).Date.AddHours(16), 20),
            new Event("Cloud & .NET Conference", "Talks about building and deploying .NET applications.",
                DateTime.Now.AddDays(14).Date.AddHours(10), 100));

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
