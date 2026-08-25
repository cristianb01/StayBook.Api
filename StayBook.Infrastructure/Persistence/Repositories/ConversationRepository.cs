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

    public Task<Conversation?> GetByBookingIdAsync(int bookingId, CancellationToken cancellationToken)
    {
        return _dbContext.Conversations
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.BookingId == bookingId, cancellationToken);
    }
}