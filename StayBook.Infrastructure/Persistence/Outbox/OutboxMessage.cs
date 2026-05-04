namespace StayBook.Infrastructure.Persistence.Outbox;

public class OutboxMessage
{
    public int Id { get; set; }
    public string Type { get; set; } = null!;
    public string Payload { get; set; } = null!;
    public DateTime OccurredOn { get; set; }
    public DateTime? ProcessedOn { get; set; }
}
