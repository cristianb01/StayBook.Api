using StayBook.Application.Features.Auth.Dtos;
using StayBook.Application.Features.Conversations.Dtos;
using StayBook.Domain.Conversations;

namespace StayBook.Application.Features.Bookings.DTOs;

public record ConversationDto(int Id, int BookingId, List<MessageDto> Messages, UserDto Guest, UserDto Host, DateTime CreatedAt);