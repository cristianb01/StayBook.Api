using StayBook.Application.Features.Bookings.DTOs;

namespace StayBook.Application.Interfaces;

public interface IConversationsQueries
{
    Task<ConversationDto?> GetByBookingId(int bookingId, int userId, CancellationToken cancellationToken);
}