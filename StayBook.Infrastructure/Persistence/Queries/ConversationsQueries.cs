using Microsoft.EntityFrameworkCore;
using StayBook.Application.Features.Auth.Dtos;
using StayBook.Application.Features.Conversations.Dtos;
using StayBook.Application.Interfaces;
using ConversationDto = StayBook.Application.Features.Bookings.DTOs.ConversationDto;

namespace StayBook.Infrastructure.Persistence.Queries;

public class ConversationsQueries : IConversationsQueries
{
    private readonly AppDbContext _dbContext;

    public ConversationsQueries(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ConversationDto?> GetByBookingId(int bookingId, int userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Bookings
            .AsNoTracking()
            .Where(b => b.Id == bookingId) 
            .Select(b => new ConversationDto
                (
                    b.Conversation!.Id,
                    bookingId,
                    b.Conversation.Messages
                        .Select(m => new MessageDto
                        (
                            m.Id,
                            m.SenderId,
                            m.Content,
                            m.CreatedAt,
                            m.ReadAt
                        )).ToList(),
                    new UserDto(b.Guest.Id, b.Guest.UserName, b.Guest.Email, b.Guest.Role),
                    new UserDto(b.Host.Id, b.Host.UserName, b.Host.Email, b.Host.Role),
                    b.Conversation.CreatedAt
                )
            ).SingleOrDefaultAsync(cancellationToken);
    }
}