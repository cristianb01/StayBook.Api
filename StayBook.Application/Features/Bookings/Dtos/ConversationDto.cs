using StayBook.Domain.Conversations;

namespace StayBook.Application.Features.Bookings.DTOs;

public record ConversationDto(int Id, int BookingId, List<Message> Messages, DateTime CreatedAt);