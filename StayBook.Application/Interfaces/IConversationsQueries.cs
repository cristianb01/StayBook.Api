using StayBook.Application.Features.Bookings.DTOs;
using StayBook.Application.Features.Conversations.Dtos;

namespace StayBook.Application.Interfaces;

public interface IConversationsQueries
{
    Task<ConversationDto?> GetByBookingId(int bookingId, int userId, CancellationToken cancellationToken);
}