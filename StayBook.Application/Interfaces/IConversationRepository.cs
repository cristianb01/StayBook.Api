using StayBook.Domain.Conversations;

namespace StayBook.Application.Interfaces;

public interface IConversationRepository
{
    Task<Conversation?> GetByBookingIdAsync(int bookingId, CancellationToken cancellationToken);
}