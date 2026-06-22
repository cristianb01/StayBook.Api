using Microsoft.EntityFrameworkCore;
using StayBook.Domain.Bookings;
using StayBook.Domain.Common;
using StayBook.Domain.Properties;
using StayBook.Domain.Users;
using StayBook.Infrastructure.Persistence.Outbox;

namespace StayBook.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>().OwnsOne(b => b.DateRange, dr =>
        {
            dr.Property(p => p.StartDate).HasColumnName("StartDate");
            dr.Property(p => p.EndDate).HasColumnName("EndDate");
        });

        modelBuilder.Entity<Property>().HasData(
            new { Id = 1, HostId = 1, Name = "Seaside Villa", Description = "A beautiful villa with ocean views." },
            new { Id = 2, HostId = 1, Name = "Mountain Cabin", Description = "Cozy cabin surrounded by pine trees." },
            new { Id = 3, HostId = 2, Name = "City Loft", Description = "Modern loft in the heart of downtown." },
            new { Id = 4, HostId = 2, Name = "Country Cottage", Description = "Quiet cottage in the countryside." },
            new { Id = 5, HostId = 3, Name = "Lakehouse Retreat", Description = "Relaxing retreat by the lake." }
        );
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var aggregatesWithEvents = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(x => x.Entity.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = aggregatesWithEvents
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            OutboxMessages.Add(new OutboxMessage()
            {
                Type = domainEvent.GetType().Name!,
                Payload = System.Text.Json.JsonSerializer.Serialize(domainEvent),
                OccurredOn = DateTime.UtcNow
            });
        }
        
        var result = await  base.SaveChangesAsync(cancellationToken);

        foreach (var aggregate in aggregatesWithEvents)
        {
            aggregate.Entity.ClearDomainEvents();
        }
        
        return result;
    }
}