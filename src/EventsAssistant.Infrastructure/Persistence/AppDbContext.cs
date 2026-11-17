using EventsAssistant.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventsAssistant.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Event> Events => Set<Event>();
    public DbSet<EventBooking> EventBookings => Set<EventBooking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("Events");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(160).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(2000).IsRequired();
            entity.HasCheckConstraint("CK_Events_Capacity", "\"Capacity\" > 0");
            entity.HasCheckConstraint("CK_Events_AvailableSeats", "\"AvailableSeats\" >= 0 AND \"AvailableSeats\" <= \"Capacity\"");
        });

        modelBuilder.Entity<EventBooking>(entity =>
        {
            entity.ToTable("EventBookings");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.AttendeeName).HasMaxLength(160).IsRequired();
            entity.HasOne(x => x.Event)
                .WithMany()
                .HasForeignKey(x => x.EventId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
