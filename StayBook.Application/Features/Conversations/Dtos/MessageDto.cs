namespace StayBook.Application.Features.Conversations.Dtos;

public record MessageDto(int Id, int SenderId, string Content, DateTime CreatedAt,  DateTime? ReadAt);