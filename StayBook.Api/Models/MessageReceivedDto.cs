namespace StayBook.Api.Models;

public record MessageReceivedDto(
    int Id, 
    int BookingId,
    int SenderId,
    string Content,
    DateTime CreatedAtUtc
    );