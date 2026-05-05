namespace StayBook.Domain.Events;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}