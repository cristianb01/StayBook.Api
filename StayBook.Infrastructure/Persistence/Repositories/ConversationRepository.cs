using Microsoft.EntityFrameworkCore;
using StayBook.Application.Interfaces;
using StayBook.Domain.Conversations;

namespace StayBook.Infrastructure.Persistence.Repositories;

public class ConversationRepository : IConversationRepository
{
    private readonly AppDbContext _dbContext;

    public ConversationRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Conversation?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _dbContext.Conversations.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
}