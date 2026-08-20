using StayBook.Domain.Conversations;

namespace StayBook.Application.Interfaces;

public interface IConversationRepository
{
    Task<Conversation?> GetByIdAsync(int id, CancellationToken cancellationToken);
}