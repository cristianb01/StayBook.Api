namespace StayBook.Domain.Events;

public class DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; } =  DateTime.UtcNow;
}