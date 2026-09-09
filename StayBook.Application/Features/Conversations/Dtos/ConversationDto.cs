using StayBook.Application.Features.Auth.Dtos;

namespace StayBook.Application.Features.Conversations.Dtos;

public record ConversationDto(int Id, int BookingId, List<MessageDto> Messages, UserDto Guest, UserDto Host, DateTime CreatedAt);