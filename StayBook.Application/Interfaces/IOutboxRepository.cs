using StayBook.Application.Models;

namespace StayBook.Application.Interfaces;

public interface IOutboxRepository
{
    Task AddAsync(OutboxEvent @outboxEvent, CancellationToken cancellationToken);
    Task MarkAsProcessedAsync(int outboxMessageId, CancellationToken cancellationToken);
}