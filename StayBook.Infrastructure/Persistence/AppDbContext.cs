using Microsoft.EntityFrameworkCore;
using StayBook.Domain.Bookings;
using StayBook.Domain.Common;
using StayBook.Domain.Conversations;
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
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>().OwnsOne(b => b.DateRange, dr =>
        {
            dr.Property(p => p.StartDate).HasColumnName("StartDate");
            dr.Property(p => p.EndDate).HasColumnName("EndDate");
        });

        const string seededPasswordHash = "$2a$11$MpFpbSFLtd5hKUoiTrSWsep.NlFQ6jVYcFX1pSnJ5SZMJPMjT.Wp6";

        modelBuilder.Entity<User>().HasData(
            new { Id = 1, UserName = "host-alice", PasswordHash = seededPasswordHash, Email = "alice@staybook.com", Role = UserRole.Host },
            new { Id = 2, UserName = "host-bruno", PasswordHash = seededPasswordHash, Email = "bruno@staybook.com", Role = UserRole.Host },
            new { Id = 3, UserName = "host-carmen", PasswordHash = seededPasswordHash, Email = "carmen@staybook.com", Role = UserRole.Host }
        );

        modelBuilder.Entity<Property>().HasData(
            new { Id = 1, HostId = 1, Name = "Seaside Villa", Description = "A beautiful villa with ocean views." },
            new { Id = 2, HostId = 1, Name = "Mountain Cabin", Description = "Cozy cabin surrounded by pine trees." },
            new { Id = 3, HostId = 2, Name = "City Loft", Description = "Modern loft in the heart of downtown." },
            new { Id = 4, HostId = 2, Name = "Country Cottage", Description = "Quiet cottage in the countryside." },
            new { Id = 5, HostId = 3, Name = "Lakehouse Retreat", Description = "Relaxing retreat by the lake." }
        );
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.HostId)
                .OnDelete(DeleteBehavior.Cascade);
        });
            

        modelBuilder.Entity<Message>(entity =>
        {
            entity.Property(m => m.Content)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(m => m.CreatedAt)
                .IsRequired();
            
            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasMany(c => c.Messages)
                .WithOne()
                .HasForeignKey("ConversationId")
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.Navigation<Message>(c => c.Messages)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasOne(b => b.Conversation)
                .WithOne()
                .HasForeignKey<Conversation>(c => c.BookingId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne<Property>(b => b.Property)
                .WithMany()
                .HasForeignKey(b => b.PropertyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne<User>(b => b.Guest)
                .WithMany()
                .HasForeignKey(b => b.GuestId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne<User>(b => b.Host)
                .WithMany()
                .HasForeignKey(b => b.HostId)
                .OnDelete(DeleteBehavior.Restrict);
        });
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