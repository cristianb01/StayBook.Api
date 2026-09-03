using StayBook.Application.Features.Auth.Dtos;
using StayBook.Domain.Conversations;

namespace StayBook.Application.Features.Conversations.Dtos;

public record ConversationDto(int Id, List<UserDto> Participants, List<MessageDto> Messages);