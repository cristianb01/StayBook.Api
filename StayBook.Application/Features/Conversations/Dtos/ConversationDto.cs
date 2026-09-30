using StayBook.Application.Features.Auth.Dtos;

namespace StayBook.Application.Features.Conversations.Dtos;

public record ConversationDto(
        int Id,
        List<MessageDto> Messages,
        DateTime CreatedAt
    );