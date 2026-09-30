using StayBook.Application.Features.Auth.Dtos;

namespace StayBook.Application.Features.Conversations.Dtos;

public record BookingConversationDto(
        int BookingId,
        UserDto Host,
        UserDto Guest,
        ConversationDto? Conversation
    );