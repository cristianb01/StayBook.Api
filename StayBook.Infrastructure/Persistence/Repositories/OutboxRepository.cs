using StayBook.Application.Interfaces;
using StayBook.Application.Models;
using StayBook.Infrastructure.Persistence.Outbox;

namespace StayBook.Infrastructure.Persistence.Repositories;

public class OutboxRepository : IOutboxRepository
{
    private readonly AppDbContext _dbContext;
    
    public OutboxRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddAsync(OutboxEvent outboxEvent, CancellationToken cancellationToken)
    {
        var outbox = new OutboxMessage()
        {
            Type = outboxEvent.Type,
            Payload = outboxEvent.Payload,
            OccurredOn = outboxEvent.OccurredOn
        };
        
        await _dbContext.OutboxMessages.AddAsync(outbox, cancellationToken); 
    }

    public Task MarkAsProcessedAsync(int outboxMessageId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}